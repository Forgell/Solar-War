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

		public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
			text_box = new TextBox(new Vector2(10, 10), sf);
			player_number_label = new Label("No number assigned", new Vector2(10 , 10) , Color.White , sf);
			logo = this.Content.Load<Texture2D>("logo");
			title = new TitleScreen(logo, sf, screenWidth, screenHeight, GraphicsDevice);
			// TODO: use this.Content to load your game content here
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
				if (old.IsKeyDown(Keys.D1))
				{
					network.send("take1");
					temp = network.getMessage();
					if (!temp.Equals("taken"))
					{
						player_number = 1;
						player_number_label.updateText("You are player number: " + player_number);
					}
					else
						player_number_label.updateText("Player 1 is already taken, try again!");
					//Find a way to send server a "taken" message for all
				}
				else if (old.IsKeyDown(Keys.D2))
				{
					network.send("take2");
					temp = network.getMessage();
					if (!temp.Equals("taken"))
					{
						player_number = 2;
						player_number_label.updateText("You are player number: " + player_number);
					}
					else
						player_number_label.updateText("Player 2 is already taken, try again!");
				}
				else if (old.IsKeyDown(Keys.D3))
				{
					network.send("take3");
					temp = network.getMessage();
					if (!temp.Equals("taken"))
					{
						player_number = 3;
						player_number_label.updateText("You are player number: " + player_number);
					}
					else
						player_number_label.updateText("Player 3 is already taken, try again!");

				}
				else if (old.IsKeyDown(Keys.D4))
				{
					network.send("take4");
					temp = network.getMessage();
					if (!temp.Equals("taken"))
					{
						player_number = 4;
						player_number_label.updateText("You are player number: " + player_number);
					}
					else
						player_number_label.updateText("Player 4 is already taken, try again!");
				}
			}

			string message = network.getMessage();
			if (message.Equals(""))
			{
				return;
			}
			if (message.Equals("Game Start!"))
			{
				state = State.PLAYING;
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
				this.Exit();
				networking_thread.Abort();
			}
			if (state == State.START)
			{
				MouseState m = Mouse.GetState();
				if (title.update(m.X, m.Y, m.LeftButton == ButtonState.Pressed, gameTime) || console.IsKeyDown(Keys.Enter))
					state = State.CONNECTING;
			}
			// Still connecting to ip adress


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

                    
                    break;
				case State.PLAYING:
					// playing the game all of the players are connected
					update_game(console);
					break;
				case State.CLOSING:
					// do nothing as the program is closing
					return;
			}
            //update starfield
            starfield.update(graphics);
			

			// update input feed
			this.old = console;
            base.Update(gameTime);
        }

		
		public void update_game(KeyboardState console)
		{
			// get the players game input
			Keys input = KeyboardHelper.getKeyboardGameInput(console , old);
			MouseState mouse = Mouse.GetState();
			// update the game screen

			// send the server  
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
            starfield.draw(spriteBatch);

            switch (state)
			{
				case State.START:
					title.draw(spriteBatch, gameTime);
					break;
				case State.CONNECTING:
					text_box.Draw(spriteBatch);
					break;
				case State.WAITING_FOR_ALL_PLAYERS:
					player_number_label.Draw(spriteBatch);
					break;
				default: break;
			}
            
			spriteBatch.End();
			
            base.Draw(gameTime);
        }
		
	}
}
