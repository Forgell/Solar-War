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
		Texture2D animation_text;
		public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferHeight = 1000;
			graphics.PreferredBackBufferWidth  = 1800;
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
				int j = i - (i/2);
				animation_rects[i] = new Rectangle((graphics.PreferredBackBufferWidth / 2) + (j * (width + (width/2))) , graphics.PreferredBackBufferHeight / 2 , width , width);
			}
			base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
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
			red_button    = new Button(Content.Load<Texture2D>("Sprites/text/red") , new Rectangle(graphics.PreferredBackBufferWidth /2 , graphics.PreferredBackBufferHeight / 2   + 0, 180 , 60) , Color.White);
			blue_button   = new Button(Content.Load<Texture2D>("Sprites/text/blue"), new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2   + (85*1), 60*4, 60), Color.White);
			green_button  = new Button(Content.Load<Texture2D>("Sprites/text/green"), new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2  + (85*2), 60*5, 60), Color.White);
			purple_button = new Button(Content.Load<Texture2D>("Sprites/text/purple"), new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 + (85*3), 60*6, 60), Color.White);

			animation_text = Content.Load<Texture2D>("Sprites/text/numbers");
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
						game.setPlayer(Color.Red);
					}
					else
						player_number_label.updateText("Player 1 is already taken, try again!");
					//Find a way to send server a "taken" message for all
				}
				else if (blue_button.pressed(Mouse.GetState()))
				{
					network.send("take2");
					temp = network.getMessage();
					if (!temp.Equals("taken"))
					{
						player_number = 2;
						player_number_label.updateText("You are player number: " + player_number);
						game.setPlayer(Color.Blue);
					}
					else
						player_number_label.updateText("Player 2 is already taken, try again!");
				}
				else if (green_button.pressed(Mouse.GetState()))
				{
					network.send("take3");
					temp = network.getMessage();
					if (!temp.Equals("taken"))
					{
						player_number = 3;
						player_number_label.updateText("You are player number: " + player_number);
						game.setPlayer(Color.Green);
					}
					else
						player_number_label.updateText("Player 3 is already taken, try again!");

				}
				else if (purple_button.pressed(Mouse.GetState()))
				{
					network.send("take4");
					temp = network.getMessage();
					if (!temp.Equals("taken"))
					{
						player_number = 4;
						player_number_label.updateText("You are player number: " + player_number);
						game.setPlayer(Color.Purple);
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
						case 1: temp = Color.Red; break;
						case 2: temp = Color.Blue; break;
						case 3: temp = Color.Green; break;
						case 4: temp = Color.Purple; break;
					}
					//game = new Game(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, Content, temp);
					//game.Load(Content.ServiceProvider);
					break;
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
		}

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
			// Get raw keyboard input
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
			if (console.IsKeyDown(Keys.E) && !old.IsKeyDown(Keys.E) && state == State.WAITING_FOR_ALL_PLAYERS)
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
						update_waiting_animation();
                    
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
					title.draw(spriteBatch, gameTime);
					starfield.draw(spriteBatch);
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
						player_number_label.Draw(spriteBatch);
						red_button.Draw(spriteBatch);
						blue_button.Draw(spriteBatch);
						green_button.Draw(spriteBatch);
						purple_button.Draw(spriteBatch);
					}
					else
					{
						// draw animation for until game starts
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
				spriteBatch.Draw(animation_text ,animation_rects[i] , new Rectangle(0 , 0 , 60 , 60) , Color.White);
			}
		}
	}
}
