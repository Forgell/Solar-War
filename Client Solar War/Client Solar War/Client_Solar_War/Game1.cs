using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace Client_Solar_War
{
	enum State
	{
		CONNECTING, WAITING_FOR_ALL_PLAYERS , CLOSING , NULL, START , PLAYING
	}


    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //Networking varibles
        State state;
        Networking network;
		// Threading for the multi threading
		Thread networking_thread;
		// Other varibles that might be useful that is not networking
		KeyboardState old;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
		TextBox text_box;
		Label player_number_label;
		// To see what player you are when all the players are connecting
		int player_number;
		int screenHeight, screenWidth;
		SpriteFont sf;
		Texture2D logo;
		TitleScreen title;
		Starfield starfield;
		Button red_button;
		Button blue_button;
		Button green_button;
		Button purple_button;
		// for playing the game
		Game game;
		//Soloar//orbit //orbit;
		Texture2D game_text_text , code_text;
		Rectangle game_rect, code_rect;

		double aimation_rads;
		Rectangle[] animation_rects;
		Color[] animation_color;
		Texture2D animation_text;
		int animatioin_timer;
		string players_joined;
		Button animation_button;
		int iconState;
		string iconColor;
		public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferHeight = 1000;
			graphics.PreferredBackBufferWidth = 1800;
			Window.Title = "Solar War";
			//graphics.IsFullScreen = true;
			graphics.ApplyChanges();
			Content.RootDirectory = "Content";
			
		}

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
			// instancience of network varibles
			// star field varibles 
			iconColor = "null";
			iconState = 0;
			starfield = new Starfield(GraphicsDevice, this.Content.Load<Texture2D>("Star"));
			networking_thread = new Thread(network_communication);
            network = new Networking();
            state = State.START;
			//waiting for all computer to connect
			// gneric varibles
			old = Keyboard.GetState();
			IsMouseVisible = true;
			player_number = 0;
			screenHeight = GraphicsDevice.Viewport.Height;
			screenWidth = GraphicsDevice.Viewport.Width;
			game = new Game(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, Content, Color.Green);
			//orbit = new Soloar//orbit(200, 1 / 1.0f, 4, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, false);
			//game.Load(Content.ServiceProvider);
			game_rect = new Rectangle(50 , 50 , 240 , 60);
			code_rect = new Rectangle(game_rect.X + game_rect.Width  + 60, game_rect.Y , game_rect.Width , game_rect.Height);
			// animation of waiting
			aimation_rads = 0.0;
			animation_rects = new Rectangle[10];
			for(int i = 0; i < animation_rects.Length; i++)
			{
				int width = 20;
				int j = i - (animation_rects.Length/2);
				animation_rects[i] = new Rectangle((graphics.PreferredBackBufferWidth / 2) + (j * (width + (width/2))) , graphics.PreferredBackBufferHeight / 2 , width , width);
			}
			animation_color = new Color[animation_rects.Length];
			for(int i = 0; i < animation_color.Length; i++)
			{
				animation_color[i] = Color.White;
			}
			players_joined = "";
			base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
			// Create a new SpriteBatch, which can be used to draw textures.
			System.Windows.Forms.Form  f = new System.Windows.Forms.Form();
            spriteBatch = new SpriteBatch(GraphicsDevice);
			sf = this.Content.Load<SpriteFont>("font");
			text_box = new TextBox(new Vector2(10, 10), Content.Load<Texture2D>("Sprites/text/numbers"));
			player_number_label = new Label("No number assigned", new Vector2(10 , 10) , Color.White , sf);
			logo = this.Content.Load<Texture2D>("logo");
			title = new TitleScreen(screenWidth, screenHeight);
			title.Load(Content);
			game.Load(Content.ServiceProvider);

			game_text_text = Content.Load<Texture2D>("Sprites/text/game_final");
			code_text = Content.Load<Texture2D>("Sprites/text/code");
			//orbit.Load(Content.ServiceProvider);
			// TODO: use this.Content to load your game content here
			Texture2D[] texts = new Texture2D[28];
			for(int i = 0; i < texts.Length; i++)
			{
				if ( i < 10)
				{
					texts[i] = Content.Load<Texture2D>("Sprites/text/red_button_animation/red0" + i);
				}
				else
				{
					texts[i] = Content.Load<Texture2D>("Sprites/text/red_button_animation/red" + i);
				}
				
			}
			red_button    = new Button(texts , new Rectangle(graphics.PreferredBackBufferWidth /2 - (180/2) , graphics.PreferredBackBufferHeight / 4   + 0, 180 , 60) , Color.White);
			texts = new Texture2D[28];
			for (int i = 0; i < texts.Length; i++)
			{
				if (i < 10)
				{
					texts[i] = Content.Load<Texture2D>("Sprites/text/blue_button_animation/blue0" + i);
					continue;
				}
				texts[i] = Content.Load<Texture2D>("Sprites/text/blue_button_animation/blue" + i);
			}
			blue_button   = new Button(texts, new Rectangle(graphics.PreferredBackBufferWidth / 2 - (60*4 / 2), graphics.PreferredBackBufferHeight / 4   + (85*1), 60*4, 60), Color.White);
			texts = new Texture2D[28];
			for (int i = 0; i < texts.Length; i++)
			{
				if (i < 10)
				{
					texts[i] = Content.Load<Texture2D>("Sprites/text/green_button_animation/green0" + i);
					continue;
				}
				texts[i] = Content.Load<Texture2D>("Sprites/text/green_button_animation/green" + i);
			}
			green_button  = new Button(texts, new Rectangle(graphics.PreferredBackBufferWidth / 2 - (60*5 / 2), graphics.PreferredBackBufferHeight / 4  + (85*2), 60*5, 60), Color.White);
			texts = new Texture2D[27];
			for (int i = 0; i < texts.Length; i++)
			{
				if(i < 10)
				{
					texts[i] = Content.Load<Texture2D>("Sprites/text/purple_button_animation/purple0" + i);
					continue;
				}
				texts[i] = Content.Load<Texture2D>("Sprites/text/purple_button_animation/purple" + i);
			}
			purple_button = new Button(texts, new Rectangle(graphics.PreferredBackBufferWidth / 2 - (60*6 / 2), graphics.PreferredBackBufferHeight / 4 + (85*3), 60*6, 60), Color.White);

			animation_text = Content.Load<Texture2D>("Sprites/text/numbers");


			texts = new Texture2D[82];
			for (int i = 0; i < texts.Length; i++)
			{
				if(i < 10)
				{
					texts[i] = Content.Load<Texture2D>("Sprites/text/begin_button_images/begin0" + i);
					continue;
				}
				texts[i] = Content.Load<Texture2D>("Sprites/text/begin_button_images/begin" + i);
			}
			animation_button = new Button(texts , new Rectangle(graphics.PreferredBackBufferWidth/2 - (150 / 2) , 200 , 150 , 30)  , Color.White );
		}

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


		public void network_communication()
		{
			while (player_number == 0)
			{
				
				string temp;
				if (red_button.pressed(Mouse.GetState()))
				{
					network.send("take1");
					temp = network.getMessage();
					if (!temp.Equals("taken"))
					{
						player_number = 1;
						player_number_label.updateText("You are player number: " + player_number);
						game.setPlayer(Color.OrangeRed);
					}
					else
						player_number_label.updateText("Player 1 is already taken, try again!");
					//Find a way to send server a "taken" message for all
				}
				else if (blue_button.pressed(Mouse.GetState()))
				{
					network.send("take2");
					temp = network.getMessage();
					if (temp.Contains("You are player: "))
					{
						player_number = 2;
						player_number_label.updateText("You are player number: " + player_number);
						game.setPlayer(Color.Blue);
						iconColor = "blue";
					}
					else
						player_number_label.updateText("Player 2 is already taken, try again!");
				}
				else if (green_button.pressed(Mouse.GetState()))
				{
					network.send("take3");
					temp = network.getMessage();
					if (temp.Contains("You are player: "))
					{
						player_number = 3;
						player_number_label.updateText("You are player number: " + player_number);
						game.setPlayer(Color.Green);
						iconColor = "green";
					}
					else
						player_number_label.updateText("Player 3 is already taken, try again!");

				}
				else if (purple_button.pressed(Mouse.GetState()))
				{
					network.send("take4");
					temp = network.getMessage();
					if (temp.Contains("You are player: "))
					{
						player_number = 4;
						player_number_label.updateText("You are player number: " + player_number);
						game.setPlayer(Color.Purple);
						iconColor = "purple";
					}
					else
						player_number_label.updateText("Player 4 is already taken, try again!");
				}
			}
			// waiting for the game to start
			Thread.Sleep(2);
			while (true)
			{
				string message = network.getMessage();
				if (message.Equals("Game Start!"))
				{
					state = State.PLAYING;
					Color temp = Color.Black;
					switch (player_number)
					{
						case 1: temp = Color.OrangeRed; break;
						case 2: temp = Color.Blue; break;
						case 3: temp = Color.Green; break;
						case 4: temp = Color.Purple; break;
					}
					//game = new Game(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, Content, temp);
					//game.Load(Content.ServiceProvider);
					break;
				}
				else
				{
					Console.WriteLine("received: " + message);
					// then the message is the players how joined as an int
					if (players_joined.Equals(""))
					{
						network.send("players?");
					}
					foreach (Char c in players_joined)
					{
						if (!message.Contains(c)) // then a player disconnected and needs to be removed
						{
							int player_as_num = c - '0';
							Color player_as_color = Color.Black;
							switch (player_as_num)
							{
								case 1: player_as_color = Color.Red; break;
								case 2: player_as_color = Color.Blue; break;
								case 3: player_as_color = Color.Green; break;
								case 4: player_as_color = Color.Purple; break;
							}
							for (int i = 0; i < animation_color.Length; i++)
							{
								if (animation_color[i] == player_as_color)
								{
									animation_color[i] = Color.White;
								}
							}
						}
					}

					foreach (Char c in message)
					{
						if (!players_joined.Contains(c)) // then this is a new player
						{
							players_joined += c;
							int player_as_num = c - '0';
							Color player_as_color = Color.Black;
							switch (player_as_num)
							{
								case 1: player_as_color = Color.Red; break;
								case 2: player_as_color = Color.Blue; break;
								case 3: player_as_color = Color.Green; break;
								case 4: player_as_color = Color.Purple; break;
							}
							// insert the new color into the animation
							int temp_counter = 0;
							for(int i = 0; i < animation_color.Length; i++)
							{
								if (animation_color[i] == Color.White)
								{
									temp_counter++;
								}
								else
								{
									temp_counter = 0;
								}
								if (temp_counter == 2)
								{
									animation_color[i] = player_as_color;
									break;
								}
							}
			 			}
					}
					
				}
				
				Thread.Sleep(5);
			}



			//game loop 
			while (true)
			{
				byte[] map = network.GetMap();
				if(map[99] == 29)
				{

					game.Update_as_Bytes(map);
					
					Thread.Sleep(10);
				}
				
			}

		}

		private void update_waiting_animation()
		{
			aimation_rads += Math.PI / 100;
			int amp = 20;
			for(int i = 0; i < animation_rects.Length; i++)
			{
				animation_rects[i].Y =(int) (Math.Sin(aimation_rads + ((Math.PI/ 2)) * i) * amp) + (graphics.PreferredBackBufferHeight/2);
			}
			if (players_joined.Equals(""))
			{
				network.send("players?"); // need to get an updated list of the players for some reason if nno playes were received
			}
			animatioin_timer++;
			if(animatioin_timer >= 20)
			{
				// load the shift the colors
				Color[] temp = new Color[animation_color.Length];
				for (int i = 0; i < animation_color.Length; i++)
				{
					temp[i] = animation_color[i];
				}
				for (int i = 1; i < animation_color.Length; i++)
				{
					animation_color[i] = temp[i - 1];
				}
				animation_color[0] = temp[animation_color.Length - 1];
				animatioin_timer = 0;
			}
			
		}

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
			// Get raw keyboard input
			if (f != null)
			{
				f.FormClosing += f_FormClosing;
			}
			KeyboardState console = Keyboard.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || console.IsKeyDown(Keys.Escape))
			{
				if (player_number != 0)
					network.closeStream(player_number);
				else
					network.closeStream();
				networking_thread.Abort();
				this.Exit();
			}
			if (state == State.START)
			{
				MouseState m = Mouse.GetState();
				if (title.update(m.X, m.Y, m.LeftButton == ButtonState.Pressed, gameTime) || console.IsKeyDown(Keys.Enter))
					state = State.CONNECTING;
			}
			// Still connecting to ip adress
			//OVERIDE
			if (animation_button.pressed(Mouse.GetState()) && state == State.WAITING_FOR_ALL_PLAYERS)
			{
				network.send("overide");
			}

			switch (state)
			{
				case State.CONNECTING:
					State temp_state = network.getConnectingInput(console, old , text_box);
                    if(temp_state != State.NULL)
                    {
                        state = temp_state;
						if (state == State.WAITING_FOR_ALL_PLAYERS)
						{
							networking_thread.Start();
						}
                    }
					break;
				case State.WAITING_FOR_ALL_PLAYERS:
					//multi threading should take care of the rest
					if (player_number != 0)
					{
						update_waiting_animation();
						animation_button.animate();
					}
					else
					{
						if (red_button.hovering(Mouse.GetState()))
						{
							red_button.animate();
						}else
						{
							red_button.stand_by();
						}
						if (blue_button.hovering(Mouse.GetState()))
						{
							blue_button.animate();
						}else
						{
							blue_button.stand_by();
						}
						if (green_button.hovering(Mouse.GetState()))
						{
							green_button.animate();
						}
						else
						{
							green_button.stand_by();
						}
						if (purple_button.hovering(Mouse.GetState()))
						{
							purple_button.animate();
						}
						else
						{
							purple_button.stand_by();
						}
					}

                    break;
				case State.PLAYING:
					if (starfield.isOnScreen)
					{
						starfield.animate(graphics);
					}
					
					string message = game.Update(gameTime);
					game.Update_Input();
					if (!message.Equals(""))
					{
						network.send(message);
					}

					break;
				case State.CLOSING:
					// do nothing as the program is closing
					return;
			}
			//update starfield
			if (state != State.PLAYING)
			{
				starfield.update(graphics);
			}
			// update input feed
			if((gameTime.TotalGameTime.Ticks % 120) == 0)
			{
				switch (iconState)
				{
					case 0:
						((System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(Window.Handle)).Icon = new System.Drawing.Icon(@"Content\Icon\" + iconColor + "2.ico");
						iconState = 1;
						break;
					case 1:
						((System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(Window.Handle)).Icon = new System.Drawing.Icon(@"Content\Icon\" + iconColor + "3.ico");
						iconState = 2;
						break;
					case 2:
						((System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(Window.Handle)).Icon = new System.Drawing.Icon(@"Content\Icon\" + iconColor + "1.ico");
						iconState = 0;
						break;
				}
			}
			this.old = console;
            base.Update(gameTime);
        }

		
		

		
		

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

			// TODO: Add your drawing code here
			spriteBatch.Begin();
			//draw starfield
			
			
			switch (state)
			{
				case State.START:
					starfield.draw(spriteBatch);
					title.draw(spriteBatch, gameTime);
					break;
				case State.CONNECTING:
					starfield.draw(spriteBatch);
					text_box.Draw(spriteBatch , code_rect.X + code_rect.Width + 60 , code_rect.Y);
										spriteBatch.Draw(game_text_text , game_rect , Color.White);
					spriteBatch.Draw(code_text, code_rect, Color.White);
					break;
				case State.WAITING_FOR_ALL_PLAYERS:
					if (player_number == 0)
					{
						starfield.draw(spriteBatch);
						red_button.Draw(spriteBatch);
						blue_button.Draw(spriteBatch);
						green_button.Draw(spriteBatch);
						purple_button.Draw(spriteBatch);
					}
					else
					{
						// draw animation for until game starts
						starfield.draw(spriteBatch);
						draw_waiting_animation(spriteBatch);
					}
					break;
				case State.PLAYING:
					if (starfield.isOnScreen)
					{
						starfield.draw(spriteBatch);
					}
					game.Draw(spriteBatch);
					break;
				default: break;
			}

			//orbit.Draw(spriteBatch);
			spriteBatch.End();
            base.Draw(gameTime);
        }
		

		private void draw_waiting_animation(SpriteBatch spritebatch)
		{
			// TODO finish the animation
			for(int i = 0; i < animation_rects.Length; i++)
			{
				spriteBatch.Draw(animation_text ,animation_rects[i] , new Rectangle(0 , 0 , 60 , 60) , animation_color[i]);
			}
			animation_button.Draw(spritebatch);
		}
		void f_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			try
			{
				if (player_number != 0)
					network.closeStream(player_number);
				else
					network.closeStream();
				networking_thread.Abort();
				this.Exit();
			}
			catch (Exception e1) { this.Exit(); }
		}
	}
}
