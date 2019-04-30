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
		private Texture2D[] tex;
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

		private IServiceProvider server;

		private Label ship_label;


		public static int Max_AMOUNT_OF_SHIPS_ON_PLANET = 99;
		public static int TRAVEL_RADIUS = 270 / 2;
		public static int TOTAL_TIME_TO_CAPTURE = 1200; // as in 60 frames

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

			if (tex != null)
			{
				pos = new Rectangle((int)origin.X + radius, (int)origin.Y, tex[0].Width, tex[0].Height);
			}

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
			tex = new Texture2D[file.Length];
			for (int i = 0; i < file.Length; i++)
			{
				tex[i] = content.Load<Texture2D>(file[i].Substring(8, file[i].Length - 4 - 8));
			}

			/*tex = new Texture2D[1];
			tex[0] = Content.Load<Texture2D>("" + fileName);
			*/

			int width = tex[0].Width / scaler;
			int height = tex[0].Height / scaler;
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


		//if it being invaded by a a 
		/*public void add(Color faction, int amnt) //some class or paerameter like color
        {
			if (faction == faction_color)
			{
				ships += amnt;
				if (ships > 99)
				{
					ships = 99;
				}
			}
			else
			{
				//battle(faction , amnt);
			}
        }*/

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


		/*public bool checkRadius()//check radius to be able to move troops
        {
            //will use x and y directions then check triangle a squared plus b squared = c squared
            return false;
        }
		*/
		/*public void Update(GameTime gt, MouseState m)
        {

            //   radius display
            mouse_Rect.X = m.X;
            mouse_Rect.Y = m.Y;
              if (pos.Intersects(mouse_Rect))
              
                {
                Radius();
                Raddis = true;
            }
            else
                Raddis = false;

            //Update(gt);


        }
		*/

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
		private void updateShips()
		{
			incrementShipTimer++;
			timer++;
			if (timer == 10)
			{
				index++;
				timer = 0;
			}
			if (index == tex.Length)
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
		public void Update(GameTime gameTime, MouseState m)
		{
			// see if the mouse is hovering to show the radius of travel
			update_radius(m);

			updateShips();

			if (is_being_taken_over)
			{
				updateCapture(gameTime);
			}

			angle += angular_speed;
			Orbit();

			if (is_being_taken_over)
			{
				capture_timer++;
				if (capture_timer == 180) // hopefully around 2 secounds
				{
					changeFaction();
				}
			}
			int diff = 7;
			selected_rect.X = pos.X - diff;
			selected_rect.Y = pos.Y - diff;//, width + diff, height + diff);
			ship_label.updateText("" + ships);
			ship_label.updatePosition(pos.X - 25, pos.Y);
			ship_label.updateColor(ships_color);
		}

		private void changeFaction()
		{

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

		//If ships of a diferent color than planet is at planet, then ships attack defending ships
		/*private void attacked()
        {
            isAttacked = false;
            for(int i = 0; i < 4; i++) 
            //making sure that that the ships in the planet have not started incrementing and that there is another player attacking
            {
                
                if(i == color)
                {
                    //do nothing
                }
                else
                {
                    if (ships[i] > 0)
                        isAttacked = true;
                }
            }
            if(isAttacked)
            {
                for(int i = 0; i < 4; i++) //each color
                {
                    if(i != color)
                    {
                        for(int j = ships[i]; j > 0 && ships[color] > 0; j--)//attacking or defending groups of ships have not run out of ships
                        {
                            ships[color] = ships[color] - 1;
                            ships[i] = ships[i] - 1;
                        }
                        if(ships[color] == 0)
                        {
                            //planet has become neutral
                            color = 4; //4 = neutral
                        }
                    }
                }
            }
        }*/


		public void Draw(SpriteBatch spritebatch)
		{
			spritebatch.Draw(tex[index], pos, Color.White);
			DrawShips(spritebatch);
			//radius
			if (Raddis)
				spritebatch.Draw(Radius_Tex, Radius_rect, faction_color);
			if (selected)
			{
				spritebatch.Draw(Radius_Tex, selected_rect, Color.Black);
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

		}
	}
}
