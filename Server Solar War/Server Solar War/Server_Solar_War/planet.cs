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

		//There is also a ship class, but this is the number of ships at this planet.
		private int[] ships;
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
		public Planet(string fileName, Vector2 origin,int radius , double angular_speed ,int scaler ,ContentManager Contents) // input degress
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
            double distance = angle * Math.Pow(radius, 2);
            this.angular_speed = MathHelper.ToRadians((float)angular_speed);
			this.index = 0;
			this.scaler = scaler;
			//temp_rects = new List<Rectangle>();
			//temp_rects.Add(new Rectangle((int)origin.X , (int)origin.Y , 1 , 1));

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

        public void Update(GameTime gameTime)
        {
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
        }
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(tex[index], pos, Color.White);
			//foreach(Rectangle rect in temp_rects)
			//{
			//	spritebatch.Draw(tex[index] , rect , Color.Black);
			//}
        }



    }
}
