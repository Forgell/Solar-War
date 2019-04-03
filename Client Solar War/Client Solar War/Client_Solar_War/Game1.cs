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

namespace Client_Solar_War
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
		//Networking varibles
		public static readonly Socket ClientSocket = new Socket(AddressFamily.InterNetwork , SocketType.Stream, ProtocolType.Tcp);
		public static readonly int PORT = 100;
		bool isConnecting;
		//ArrayList ip_address_list;
		//Vector2 position_of_ip_text_box;
		// Other varibles that might be useful that is not networking
		KeyboardState old;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
		TextBox text_box;
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
			isConnecting = true;
			//ip_address_list = new ArrayList();
			// gneric varibles
			old = Keyboard.GetState();
			IsMouseVisible = true;
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
			if (isConnecting)
			{
				getConnectingInput(console);
			}
			// update input feed
			old = console;
            base.Update(gameTime);
        }

		public void closeStream()
		{
			if (ClientSocket.IsBound)
			{
				byte[] exit_message_as_bytes = System.Text.Encoding.ASCII.GetBytes("exit");
				ClientSocket.Send(exit_message_as_bytes , 0 , exit_message_as_bytes.Length , SocketFlags.None);
				ClientSocket.Close();
			}
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
				isConnecting = false;
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
		public void getConnectingInput(KeyboardState console)
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here
			spriteBatch.Begin();
			if(isConnecting) 
				text_box.Draw(spriteBatch);
			spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
