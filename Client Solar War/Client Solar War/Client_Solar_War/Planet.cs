using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace Client_Solar_War
{
	class Planet
	{
		//-1 will be replace with some resonable value
		//private Random r;
		private Texture2D[][] tex;

		private SpriteFont font;
		public Rectangle position
		{

			get { return pos; }
		}
		private Rectangle pos;

		public double Angle
		{
			get { return angle; }
		}
		private double angle;


		public Vector2 Orgin
		{
			get { return origin; }
		}
		private Vector2 origin;


		public double Angular_Speed
		{
			get { return angular_speed; }
		}
		private double angular_speed;



		public int Scaler
		{
			get { return scaler; }
		}
		private int scaler;

		public Color Color
		{
			get { return faction_color; }
		}

		public int Ships
		{
			get
			{
				return ships;
			}
		}

		public int ID
		{
			get { return id; }
		}
		private int id;

		private IServiceProvider server;

		private Label ship_label;


		public static int Max_AMOUNT_OF_SHIPS_ON_PLANET = 99;
		public static int TRAVEL_RADIUS = 270 / 2;
		public static int TOTAL_TIME_TO_CAPTURE = 1200; // as in 60 frames
		public static int TOTAL_AMOUNT_OF_PLANETS = 0;

		private int index;
		private Vector2 offset;
		//radius for the planet to invade
		private Texture2D Radius_Tex;
		private Rectangle Radius_rect, mouse_Rect;
		private Boolean Raddis;

		//There is also a ship class, but this is the number of ships at this planet.
		private int ships;
		private Vector2[] shipPositions;
		//private int color; //team that the planet is for {0 = orange, 1 = blue, 2 = green, 3 = purple, 4 = neutral}
		Label label;
		private bool isAttacked;
		private int incrementShipTimer;
		private int timer;
		private String fileName;
		private int radius;

		private Color faction_color;
		private int faction_num;
		private bool selected;
		private Rectangle selected_rect;
		private bool is_being_taken_over;
		private double capture_timer;
		private Label capture_label;
		private Color ships_color;
		public Planet(string fileName, Vector2 origin, int radius, double angular_speed, int scaler, Color faction_color) // input degress
		{
			this.origin = origin;
			this.radius = radius;
			this.fileName = fileName;
			id = ++TOTAL_AMOUNT_OF_PLANETS;
			//how to set the size depending on the planet
			angle = Math.PI / 180.0 * 5.0;
			timer = 0;
			incrementShipTimer = 0;
			double distance = angle * Math.Pow(radius, 2);
			this.angular_speed = MathHelper.ToRadians((float)angular_speed);
			this.index = 0;
			this.scaler = scaler;
			//temp_rects = new List<Rectangle>();
			//temp_rects.Add(new Rectangle((int)origin.X , (int)origin.Y , 1 , 1));
			ships = 0;
			shipPositions = new Vector2[4];

			this.faction_color = faction_color;
			mouse_Rect = new Rectangle(0, 0, 5, 5);
			//travel_radius = 100;
			selected = false;
			is_being_taken_over = false;
			capture_timer = 0;
			ships_color = faction_color;
			
		}

		public void setAngle(double angle) // input degeres
		{
			this.angle = MathHelper.ToRadians((float)angle);
		}

		public void Load(IServiceProvider serve)
		{
			this.server = serve;
			//read file and load
			ContentManager content = new ContentManager(serve, "Content");
			//tex = content.Load<Texture2D>(name);
			fileName = "Sprites/planets/" + fileName + "/";
			string[] file = Directory.GetFiles("Content/" + fileName);
			tex = new Texture2D[5][];

			for(int j = 0; j < 5; j++)
			{
				tex[j] = new Texture2D[3];
				for (int i = 0; i < tex[j].Length; i++)
				{
					tex[j][i] = content.Load<Texture2D>("Sprites/planets/planet-" + (j + 1) + "/planet-" + ( j + 1) + "-" + (i + 1));
				}
			}
			

			/*tex = new Texture2D[1];
			tex[0] = Content.Load<Texture2D>("" + fileName);
			*/

			int width = tex[0][0].Width / scaler;
			int height = tex[0][0].Width / scaler;
			offset = new Vector2(width / 2.0f, height / 2.0f);

			pos = new Rectangle((int)origin.X + radius - (int)offset.X, (int)origin.Y - (int)offset.Y, width, height);
			int diff = 7;
			selected_rect = new Rectangle(pos.X - diff, pos.Y - diff, width + (diff * 2), height + (diff * 2));
			//load SpriteFont
			font = content.Load<SpriteFont>("SpriteFont1");
			//radius tex
			Radius_Tex = content.Load<Texture2D>("Sprites/white-circle");
			ship_label = new Label("" + ships, new Vector2(pos.X, pos.Y), faction_color, font);
			capture_label = new Label("" + capture_timer, new Vector2(pos.X, pos.Y), Color.Black, font);

		}


		/**  Radiusa position movement **/
		private void Radius()
		{
			int X = (pos.X - (pos.Width * 4)); //- pos.Width/2;
			int y = (pos.Y - (pos.Height * 4)); //- pos.Height/2;
												//Console.WriteLine(pos.Width * 9);
			Radius_rect = new Rectangle(X, y, TRAVEL_RADIUS * 2, TRAVEL_RADIUS * 2);
		}
		/**  animation */
		private void Orbit()
		{

			double x = (origin.X + Math.Cos(angle) * radius) - offset.X;
			double y = (origin.Y + Math.Sin(angle) * radius) - offset.Y;
			pos.X = (int)x;
			pos.Y = (int)y;

		}


		
		public void tranfer_troops(Planet source, int amount)
		{
			source.ships -= amount;
			if (source.faction_color == this.faction_color)
			{
				// peacful tranfer
				this.ships += amount;
			}
			else
			{
				// it is attacking
				if (this.ships >= amount)
				{
					// the faction holds
					this.ships -= amount;
				}
				else
				{
					// stop producing
					// start a timer
					is_being_taken_over = true;
					this.ships = amount - this.ships;

					ships_color = source.faction_color;

				}

			}
		}



		private void update_radius(MouseState m)
		{
			mouse_Rect.X = m.X;
			mouse_Rect.Y = m.Y;
			if (pos.Intersects(mouse_Rect))

			{
				Radius();
				Raddis = true;
			}
			else
				Raddis = false;
		}

		public void UpdateInput(MouseState m)
		{
			if (id > 18)
			{
				Console.WriteLine(id);
			}
			update_radius(m);
			timer++;
			if (timer == 10)
			{
				index++;
				timer = 0;
			}
			if (index == tex[0].Length)
			{
				index = 0;
			}
			int diff = 7;
			selected_rect.X = pos.X - diff;
			selected_rect.Y = pos.Y - diff;//, width + diff, height + diff);
		}

		private void updateShips()
		{
			incrementShipTimer++;
			timer++;
			if (timer == 10)
			{
				index++;
				timer = 0;
			}
			if (index == tex[0].Length)
			{
				index = 0;
			}

			if (incrementShipTimer == 60)//increase number of ships each second
			{

				incrementShips();
				incrementShipTimer = 0;
			}
		}

		public void updateCapture(GameTime gametime)
		{
			capture_timer++;
			capture_label.updatePosition(pos.X, pos.Y + pos.Width);
			capture_label.updateText("" + Math.Round((capture_timer / (TOTAL_TIME_TO_CAPTURE * 1.0)) * 100) + "%");
			if (capture_timer >= TOTAL_TIME_TO_CAPTURE)
			{
				// then lets tranfer the planets contoll
				capture_timer = 0;
				this.faction_color = ships_color;
				//switch()
				if (faction_color == Color.Red)
				{
					fileName = "planet-1";
				}
				if (faction_color == Color.Blue)
				{
					fileName = "planet-2";
				}
				if (faction_color == Color.Green)
				{
					fileName = "planet-3";
				}
				if (faction_color == Color.Purple)
				{
					fileName = "planet-4";
				}

				Load(server);

				is_being_taken_over = false;
			}
		}

		public void Update_As_Bytes(byte[] map)
		{
			int id = (map[0] & 248) >> 3;
			this.id = id;
			pos.X = ((map[0] & 7) << 8) | map[1];
			pos.Y = (map[2] << 3) | ((map[3] & 224) >> 5);
			ships = ((map[3] & 31) << 2) | ((map[4] & 192) >> 6);
			int byte_color = map[4] & 7;
			Color temp = Color.Black;
			faction_num = byte_color;
			switch (byte_color)
			{
				case 0: temp = Color.Red;fileName = "planet-1"; break;
				case 1: temp = Color.Blue; fileName = "planet-2"; break;
				case 2: temp = Color.Green; fileName = "planet-3"; break;
				case 3: temp = Color.Purple; fileName = "planet-4"; break;
				case 4: temp = Color.Black; fileName = "planet-5"; break;
			}
			if (temp != faction_color)
			{
				faction_color = temp;
				Load(server);
			}

			int ships_color_as_bytes = (map[4] & 56) >> 3;
			switch (ships_color_as_bytes)
			{
				case 0: ships_color = Color.Red; break;
				case 1: ships_color = Color.Blue; break;
				case 2: ships_color = Color.Green; break;
				case 3: ships_color = Color.Purple; break;
				case 4: ships_color = Color.Black; break;
			}
			ship_label.updateText("" + ships);
			ship_label.updatePosition(pos.X - 25, pos.Y);
			ship_label.updateColor(ships_color);
		}

		private void incrementShips()
		{
			if (faction_color == Color.Black)
			{
				//nothing happens because planet is neutral
			}
			else
			{
				if (ships < Max_AMOUNT_OF_SHIPS_ON_PLANET && !is_being_taken_over)
				{
					ships += 1;
				}
			}
		}



		public void select()
		{
			selected = true;
		}

		public void deselect()
		{
			selected = false;
		}

	

		public void Draw(SpriteBatch spritebatch)
		{
			try
			{
				spritebatch.Draw(tex[faction_num][index], pos, Color.White);
			}
			catch (ArgumentNullException e) { }
			DrawShips(spritebatch);
			//radius
			if (Raddis)
				if(faction_color != Color.Black)
					spritebatch.Draw(Radius_Tex, Radius_rect, faction_color);
				else
					spritebatch.Draw(Radius_Tex, Radius_rect, Color.White);
			if (selected)
			{
				spritebatch.Draw(Radius_Tex, selected_rect, Color.White);
			}
			if (is_being_taken_over)
			{
				capture_label.Draw(spritebatch);
			}

		}

		//draw the number of ships at a planet
		private void DrawShips(SpriteBatch spritebatch)
		{
			ship_label.Draw(spritebatch);
			try {
				//ship_label.Draw(spritebatch);
			}catch(Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(ship_label + " " + spritebatch);
				Console.WriteLine();
			}

		}
	}
}
