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

namespace Server_Solar_War
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


		private int index;
		private Vector2 offset;
        //radius for the planet to invade
        private Texture2D Radius_Tex;
        private Rectangle Radius_rect, mouse_Rect;
        private Boolean Raddis;

        //There is also a ship class, but this is the number of ships at this planet.
        private int[] ships;
        private Vector2[] shipPositions;
        private int color; //team that the planet is for {0 = orange, 1 = green, 2 = purple, 3 = blue, 4 = neutral}
        private int incrementShipTimer;
        private int timer;
		private String fileName;
		private int owner;
		private int radius;
		//can someone look at this class bc i need to kknow what to implement for some method
		/*public Planet()
        {
            r = new Random();
            //invade_Capacity = rand 
        }
		*/
		public Planet(string fileName, Vector2 origin,int radius , double angular_speed ,int scaler ,ContentManager Contents, int team) // input degress
        {
			owner = 0;
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
            ships = new int[4]; //{orange, green, purple, blue}
            shipPositions = new Vector2[4];
            color = team; //{0 = orange, 1 = green, 2 = purple, 3 = blue, 4 = neutral}
            mouse_Rect = new Rectangle(0, 0, 5, 5);
            

        }

		public void setAngle(double angle) // input degeres
		{
			this.angle = MathHelper.ToRadians((float)angle);
		}
		
        public  void Load(IServiceProvider serve )
        {
            //read file and load 
            ContentManager content = new ContentManager(serve, "Content");
            //tex = content.Load<Texture2D>(name);
            fileName = "Sprites/planets/" + fileName + "/";
            string[] file = Directory.GetFiles("Content/"+fileName);
            tex = new Texture2D[file.Length];
            for (int i =0; i<file.Length; i++)
            {
                tex[i] = content.Load<Texture2D>(file[i].Substring( 8 , file[i].Length - 4 - 8));
            }

			/*tex = new Texture2D[1];
			tex[0] = Content.Load<Texture2D>("" + fileName);
			*/
			
			int width = tex[0].Width / scaler;
			int height = tex[0].Height / scaler;
			offset = new Vector2(width  / 2.0f, height / 2.0f);

			pos = new Rectangle((int)origin.X + radius - (int)offset.X , (int)origin.Y - (int)offset.Y, width, height);

            //load SpriteFont
            font = content.Load<SpriteFont>("SpriteFont1");
            //radius tex
            Radius_Tex = content.Load<Texture2D>("Sprites/white-circle");

		}


        /**  Radiusa position movement **/
        private void Radius()
        {
            int X = (pos.X - 50) - pos.Width/2;
            int y = (pos.Y - 50)-pos.Height/2;
            Radius_rect = new Rectangle(X, y, 150+pos.Width/2, 150+pos.Height/2);

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
        public void add(int team, int amnt) //some class or paerameter like color
        {
			ships[team] += amnt;
        }
        private void attack()
        {
			int[] newShips = new int[5];
			
			for (int i = 0; i < ships.Length; i++)
			{
				int enemy = 0;
				for (int x = 0; x < ships.Length; x++)
				{
					if (owner == i)
						continue;
					enemy += ships[i];
				}
				double chance = ships[i] / enemy * 50;
				//int num = r.Next((int)chance);
				/*switch (num)
				{
					case 0:
						newShips[i] = ships[i] - 5;
						break;
					case 1:
						newShips[i] = ships[i] - 4;
						break;
					case 2:
						newShips[i] = ships[i] - 3;
						break;
					case 3:
						newShips[i] = ships[i] - 2;
						break;
					case 4:
						newShips[i] = ships[i] - 1;
						break;
				}*/

				
				
			}
			ships = newShips;
		}

        public bool checkRadius()//check radius to be able to move troops
        {
            //will use x and y directions then check triangle a squared plus b squared = c squared
            return false;
        }
        public void Update(GameTime gt, MouseState m)
        {

            //   radius display
            //if (mouse shown in planet)
            mouse_Rect.X = m.X;
            mouse_Rect.Y = m.Y;
              if (pos.Intersects(mouse_Rect))
              
                {
                Radius();
                Raddis = true;
                //  if(m.LeftButton == ButtonState.Pressed)
            }
            else
                Raddis = false;

            Update(gt);


        }
        public void Update(GameTime gameTime)
        {
            MouseState m = Mouse.GetState();
            incrementShipTimer++;
            timer++;
                angle += angular_speed;
                Orbit();
			if (timer == 10)
			{
				index++;
				timer = 0;
			}
			if (index == tex.Length)
			{
				index = 0;
			}
			//temp_rects.Add(new Rectangle(pos.X + (int)offset.X , pos.Y + (int)offset.Y , 1 , 1));
           
            //attack();
            if(incrementShipTimer == 60)//increase number of ships each second
            {
                incrementShips();
                incrementShipTimer = 0;
            }

        }
        //color = (0 = orange, 1 = green, 2 = purple, 3 = blue, 4 = neutral)
        private void incrementShips()
        {
            if (color == 4)
            {
                //nothing happens because planet is neutral
            }
            else
            {
                ships[color] += 1;
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(tex[index], pos, Color.White);
            DrawShips(spritebatch);
            //radius
            if(Raddis)
                spritebatch.Draw(Radius_Tex,Radius_rect,Color.Yellow);
			//foreach(Rectangle rect in temp_rects)
			//{
			//	spritebatch.Draw(tex[index] , rect , Color.Black);
			//}
        }

        //draw the number of ships at a planet
        private void DrawShips(SpriteBatch spritebatch)
        {
            if(ships[0] != 0)
                DrawOrangeShips(spritebatch);
            if (ships[1] != 0)
                DrawGreenShips(spritebatch);
            if (ships[2] != 0)
                DrawPurpleShips(spritebatch);
            if (ships[3] != 0)
                DrawBlueShips(spritebatch);
        }
        //ships = {orange, green, purple, blue}
        //draw the int number of ships at the planet
        private void DrawOrangeShips(SpriteBatch spritebatch)
        {
            //display to the left of the planet
            shipPositions[0] = new Vector2(pos.X - 25, pos.Y);
            spritebatch.DrawString(font, "" + ships[0], shipPositions[0], Color.Orange);
        }
        private void DrawGreenShips(SpriteBatch spritebatch)
        {
            //display above the planet
            shipPositions[1] = new Vector2(pos.X + 10, pos.Y - 22);
            spritebatch.DrawString(font, "" + ships[1], shipPositions[1], Color.Green);
        }
        private void DrawPurpleShips(SpriteBatch spritebatch)
        {
            //display to the right of the planet
            shipPositions[2] = new Vector2(pos.X + 35, pos.Y);
            spritebatch.DrawString(font, "" + ships[2], shipPositions[2], Color.Purple);
        }
        private void DrawBlueShips(SpriteBatch spritebatch)
        {
            //display below the planet
            shipPositions[3] = new Vector2(pos.X + 10, pos.Y + 25);
            spritebatch.DrawString(font, "" + ships[3], shipPositions[3], Color.Blue);
        }

    }
}
