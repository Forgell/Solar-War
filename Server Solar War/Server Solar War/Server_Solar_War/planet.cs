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
        private Random r;
        private Texture2D[] tex;
        public Rectangle position
        {

            get { return pos; }
        }
        private Rectangle pos;
        private String fileName;
        private double speed;
        private int owner;
        private int radius;
        private double angle; 
        private Vector2 origin;
        //There is also a ship class, but this is the number of ships at this planet.
		private int[] ships;
        private int time, timer;
        public ContentManager Content
        {
          
            get { return content; }
        }
        ContentManager content;
        private double angular_speed;
        

        //can someone look at this class bc i need to kknow what to implement for some method
        public Planet()
        {
            r = new Random();
            //invade_Capacity = rand 
        }
        public Planet(string fileName, Vector2 origin,int radius , double angular_speed) // input degress
        {
			owner = 0;
            this.origin = origin;
            this.radius = radius;
            pos = new Rectangle((int)origin.X + radius, (int)origin.Y, tex[0].Width, tex[0].Height);
            //how to set the size depending on the planet
            angle = Math.PI / 180.0 * 5.0;
            speed = -1; //how to determine, the speed
            timer = 0;
            double distance = angle * Math.Pow(radius, 2);
            time = (int)(distance / speed);
            this.angular_speed = MathHelper.ToRadians((float)angular_speed);

        }

        public Planet(string fileName,Rectangle position, Color faction)
        {
            //this.image = image;
            this.pos = position;
			owner = 0;
            //i want too implement a class to this bc makes it efficient to proccess invasion and capture 
        }
       
        public  void Load(IServiceProvider serve )
        {
            //read file and load 
            content = new ContentManager(serve, "Content");
            //tex = content.Load<Texture2D>(name);
            string[] file = Directory.GetFiles("Content/"+fileName);
            tex = new Texture2D[file.Length];
            for (int i =0; i<fileName.Length; i++)
            {
                tex[i] = content.Load<Texture2D>(fileName + "/"+file[i]);
            }
        }


        /**  animation */
        private void Orbit()
        {

            double x = (origin.X + Math.Cos(angle) * radius);
            double y = (origin.Y + Math.Sin(angle) * radius);
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
				int num = r.Next((int)chance);
				switch (num)
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
				}

				
				
			}
			ships = newShips;
		}

        public void Update(GameTime gameTime)
        {
            timer++;
            if (timer % time == 0)
            {
                angle += angular_speed;
                Orbit();
            }
            //somecondition or collision is true
            attack();
        }
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(tex[0], new Rectangle(200,200,50,50), Color.White);
        }



    }
}
