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
		ArrayList ip_address_list;
		Vector2 position_of_ip_text_box;
		// Other varibles that might be useful that is not networking
		KeyboardState old;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
		SpriteFont font;
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
			ip_address_list = new ArrayList();
			position_of_ip_text_box = new Vector2(10 , 10);
			// gneric varibles
			old = Keyboard.GetState();
			IsMouseVisible = true;
			
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
			font = Content.Load<SpriteFont>("font");
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

			// TODO: Add your update logic here
			if (isConnecting)
			{
				getConnectingInput();
			}

            base.Update(gameTime);
        }

		public void getConnectingInput()
		{
			KeyboardState console = Keyboard.GetState();
			//Keys[] current_list = console.GetPressedKeys();
			//Keys[] old_list = old.GetPressedKeys();
			if(console.IsKeyDown(Keys.NumPad0) && !old.IsKeyDown(Keys.NumPad0))
			{
				ip_address_list.Add('0');
			}
			else if (console.IsKeyDown(Keys.NumPad1) && !old.IsKeyDown(Keys.NumPad1))
			{
				ip_address_list.Add('1');
			}
			else if(console.IsKeyDown(Keys.NumPad2) && !old.IsKeyDown(Keys.NumPad2))
            {
				ip_address_list.Add('2');
			}
			else if (console.IsKeyDown(Keys.NumPad3) && !old.IsKeyDown(Keys.NumPad3))
			{
				ip_address_list.Add('3');
			}
			else if (console.IsKeyDown(Keys.NumPad4) && !old.IsKeyDown(Keys.NumPad4))
			{
				ip_address_list.Add('4');
			}
			else if (console.IsKeyDown(Keys.NumPad5) && !old.IsKeyDown(Keys.NumPad5))
			{
				ip_address_list.Add('5');
			}
			else if (console.IsKeyDown(Keys.NumPad6) && !old.IsKeyDown(Keys.NumPad6))
			{
				ip_address_list.Add('6');
			}
			else if (console.IsKeyDown(Keys.NumPad7) && !old.IsKeyDown(Keys.NumPad7))
			{
				ip_address_list.Add('7');
			}
			else if (console.IsKeyDown(Keys.NumPad8) && !old.IsKeyDown(Keys.NumPad8))
			{
				ip_address_list.Add('8');
			}
			else if (console.IsKeyDown(Keys.NumPad9) && !old.IsKeyDown(Keys.NumPad9))
			{
				ip_address_list.Add('9');
			}else

			if (console.IsKeyDown(Keys.D0) && !old.IsKeyDown(Keys.D0)) 
			{
				ip_address_list.Add('0');
			}
			else if (console.IsKeyDown(Keys.D1) && !old.IsKeyDown(Keys.D1))
			{
				ip_address_list.Add('1');
			}
			else if (console.IsKeyDown(Keys.D2) && !old.IsKeyDown(Keys.D2))
			{
				ip_address_list.Add('2');
			}
			else if (console.IsKeyDown(Keys.D3) && !old.IsKeyDown(Keys.D3))
			{
				ip_address_list.Add('3');
			}
			else if (console.IsKeyDown(Keys.D4) && !old.IsKeyDown(Keys.D4))
			{
				ip_address_list.Add('4');
			}
			else if (console.IsKeyDown(Keys.D5) && !old.IsKeyDown(Keys.D5))
			{
				ip_address_list.Add('5');
			}
			else if (console.IsKeyDown(Keys.D6) && !old.IsKeyDown(Keys.D6))
			{
				ip_address_list.Add('6');
			}
			else if (console.IsKeyDown(Keys.D7) && !old.IsKeyDown(Keys.D7))
			{
				ip_address_list.Add('7');
			}
			else if (console.IsKeyDown(Keys.D8) && !old.IsKeyDown(Keys.D8))
			{
				ip_address_list.Add('8');
			}
			else if (console.IsKeyDown(Keys.D9) && !old.IsKeyDown(Keys.D9))
			{
				ip_address_list.Add('9');
			}else
				if (console.IsKeyDown(Keys.OemPeriod) && !old.IsKeyDown(Keys.OemPeriod) || console.IsKeyDown(Keys.Decimal) && !old.IsKeyDown(Keys.Decimal))
			{
				ip_address_list.Add('.');
			}else

			if (console.IsKeyDown(Keys.Enter) && !old.IsKeyDown(Keys.Enter))
			{
				
				// Now it is the time to connect to the server
				string string_of_ip_adress = "";
				for(int i = 0; i < ip_address_list.Count; i++)
				{
					string_of_ip_adress += ip_address_list[i];
				}
				string[] parts_of_ip_address = string_of_ip_adress.Split('.');
				byte[] ip_adress_as_byte_array = new byte[parts_of_ip_address.Length];
				for (int i = 0; i < parts_of_ip_address.Length; i++)
				{
					ip_adress_as_byte_array[i] = Byte.Parse(parts_of_ip_address[i]);
				}
				try {
					ClientSocket.Connect(new IPEndPoint(new IPAddress(ip_adress_as_byte_array), PORT));
					isConnecting = false;
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);

				}
			}else if (console.IsKeyDown(Keys.Back) && !old.IsKeyDown(Keys.Back))
			{
				if (ip_address_list.Count != 0)
				{
					ip_address_list.RemoveAt(ip_address_list.Count - 1);
				}
			}

			old = console;

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
			if (isConnecting)
			{
				string string_of_ip_adress = "";
				for (int i = 0; i < ip_address_list.Count; i++)
				{
					string_of_ip_adress += ip_address_list[i];
				}
				spriteBatch.DrawString( font, string_of_ip_adress , position_of_ip_text_box , Color.White);
			}

			spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
