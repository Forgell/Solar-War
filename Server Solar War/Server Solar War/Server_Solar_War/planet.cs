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
        private int color; //team that the planet is for {0 = orange, 1 = blue, 2 = green, 3 = purple, 4 = neutral}
        Label label;
        private bool isAttacked;
        private int incrementShipTimer;
        private int timer;
		private String fileName;
		private int owner;
		private int radius;
        private int travel_radius;

        private Color faction_color;

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
            ships = new int[4]; //{0 = orange, 1 = blue, 2 = green, 3 = purple}
            shipPositions = new Vector2[4];
            color = team; //{0 = orange, 1 = blue, 2 = green, 3 = purple, 4 = neutral}
            switch (team)
            {
                case 0: faction_color = Color.Orange;break;
                case 1: faction_color = Color.Blue; break;
                case 2: faction_color = Color.Green; break;
                case 3: faction_color = Color.Purple; break;
                case 4: faction_color = Color.Black; break;
            }
            mouse_Rect = new Rectangle(0, 0, 5, 5);
            travel_radius = 100;

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
            int X = (pos.X - (pos.Width * 4)); //- pos.Width/2;
            int y = (pos.Y - (pos.Height * 4)); //- pos.Height/2;
            Radius_rect = new Rectangle(X, y, (pos.Width * 9), (pos.Height * 9));
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
        //color = {0 = orange, 1 = blue, 2 = green, 3 = purple, 4 = neutral}
        private void incrementShips()
        {
            if (color == 4)
            {
                //nothing happens because planet is neutral
            }
            else
            {
                if(ships[color] < 99)
                {
                    ships[color] += 1;
                }
            }
        }

        private void playerAction(MouseState mouse)
        {

        }

        //If ships of a diferent color than planet is at planet, then ships attack defending ships
        private void attacked()
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
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(tex[index], pos, Color.White);
            DrawShips(spritebatch);
            //radius
            if(Raddis)
                spritebatch.Draw(Radius_Tex,Radius_rect, faction_color);
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
                DrawBlueShips(spritebatch);
            if (ships[2] != 0)
                DrawGreenShips(spritebatch);
            if (ships[3] != 0)
                DrawPurpleShips(spritebatch);

        }
        //ships = {0 = orange, 1 = blue, 2 = green, 3 = purple}
        //draw the int number of ships at the planet
        private void DrawOrangeShips(SpriteBatch spritebatch)
        {
            //display to the left of the planet
            shipPositions[0] = new Vector2(pos.X - 25, pos.Y);
            label = new Label("" + ships[0], shipPositions[0], Color.Orange, font);
            label.Draw(spritebatch);
            //spritebatch.DrawString(font, "" + ships[0], shipPositions[0], Color.Orange);
        }
        private void DrawBlueShips(SpriteBatch spritebatch)
        {
            //display below the planet
            shipPositions[1] = new Vector2(pos.X + 10, pos.Y + 25);
            label = new Label("" + ships[1], shipPositions[1], Color.Blue, font);
            label.Draw(spritebatch);
            //spritebatch.DrawString(font, "" + ships[1], shipPositions[1], Color.Blue);
        }
        private void DrawGreenShips(SpriteBatch spritebatch)
        {
            //display to the right of the planet
            shipPositions[2] = new Vector2(pos.X + 35, pos.Y);
            label = new Label("" + ships[2], shipPositions[2], Color.Green, font);
            label.Draw(spritebatch);
            //spritebatch.DrawString(font, "" + ships[2], shipPositions[2], Color.Green);
        }
        private void DrawPurpleShips(SpriteBatch spritebatch)
        {
            //display above the planet
            shipPositions[3] = new Vector2(pos.X + 10, pos.Y - 22);
            label = new Label("" + ships[3], shipPositions[3], Color.Purple, font);
            label.Draw(spritebatch);
            //spritebatch.DrawString(font, "" + ships[3], shipPositions[3], Color.Purple);
        }
        

    }
}
