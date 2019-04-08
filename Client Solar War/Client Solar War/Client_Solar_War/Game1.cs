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
namespace Client_Solar_War
{
	enum State
	{
		CONNECTING , WAITING_FOR_ALL_PLAYERS , CLOSING
	}


    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
		//Networking varibles
		public static readonly Socket ClientSocket = new Socket(AddressFamily.InterNetwork , SocketType.Stream, ProtocolType.Tcp);
		public static readonly int PORT = 100;
		//bool isConnecting;
		State state;
		//ArrayList ip_address_list;
		//Vector2 position_of_ip_text_box;
		// Other varibles that might be useful that is not networking
		KeyboardState old;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
		TextBox text_box;
		Label player_number_label;
		// To see what player you are when all the players are connecting
		int player_number;

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
            

            state = State.CONNECTING;
			//waiting for all computer to connect
			// gneric varibles
			old = Keyboard.GetState();
			IsMouseVisible = true;
			player_number = 0;
			//Console.WriteLine("\n\n\n" + (char)(0) + "\n\n\n");
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
			text_box = new TextBox(new Vector2(10, 10), Content.Load<SpriteFont>("font"));
			player_number_label = new Label("No number assigned", new Vector2(10 , 10) , Color.White ,Content.Load<SpriteFont>("font"));
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
				closeStream();
				this.Exit();
			}

			// Still connecting to ip adress


			switch (state)
			{
				case State.CONNECTING:
					getConnectingInput(console, old);
					break;
				case State.WAITING_FOR_ALL_PLAYERS:
                    //recieveServerMessage();
                    recieveServerMessage();
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

		public void recieveServerMessage()
		{
			byte[] server_message_as_bytes = new byte[100];
			ClientSocket.Receive(server_message_as_bytes); //(server_message_as_bytes, 0, server_message_as_bytes.Length, SocketFlags.None);
			server_message_as_bytes = server_message_as_bytes.Where(val => val != 0).ToArray();
            string server_message_as_string = Encoding.ASCII.GetString(server_message_as_bytes);
			if (!server_message_as_string.Equals(""))
			{
				//Console.WriteLine(server_message_as_string);
				if (server_message_as_string.Contains("You are player: "))
				{
					player_number = server_message_as_string[server_message_as_string.Length - 1] - '0';
					player_number_label.updateText("You are player: " + player_number);
				}

                ClientSocket.Send(Encoding.ASCII.GetBytes("buffer"));
            }
			
		}

		public void closeStream()
		{
			if (ClientSocket.IsBound)
			{
				byte[] exit_message_as_bytes = Encoding.ASCII.GetBytes("exit");
				ClientSocket.Send(exit_message_as_bytes , 0 , exit_message_as_bytes.Length , SocketFlags.None);
				ClientSocket.Close();
			}
			state = State.CLOSING;
		}

		/// <summary>
		/// is run only when the enter key is pressed and attempts to connect to the ip that is inputed by th custom text box
		/// </summary>
		public void attemptConnectionToServer()
		{
			string[] parts_of_ip_address = text_box.Text.Split('.');
			byte[] ip_adress_as_byte_array = new byte[parts_of_ip_address.Length];
			for (int i = 0; i < parts_of_ip_address.Length; i++)
			{
				ip_adress_as_byte_array[i] = Byte.Parse(parts_of_ip_address[i]);
			}
			try
			{
				ClientSocket.Connect(new IPEndPoint(new IPAddress(ip_adress_as_byte_array), PORT));
				state = State.WAITING_FOR_ALL_PLAYERS;
                
            }
			catch (Exception e)
			{
				Console.WriteLine(e.Message);

			}

		}
		/// <summary>
		/// handels  the input from the KeyboardHelper class
		/// </summary>
		/// <param name="console"></param>
		public void getConnectingInput(KeyboardState console , KeyboardState old)
		{
			char input_as_char = KeyboardHelper.getIPInput(console , old);
			switch ((int)input_as_char)
			{
				case 000: break; // nothing is pressed
				case 004: // is transmitted if enter key is pressed
					attemptConnectionToServer();
					break;

				default:
					text_box.updateText(input_as_char);
					break;


			}

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
