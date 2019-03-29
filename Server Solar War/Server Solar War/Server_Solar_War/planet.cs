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


namespace Server_Solar_War
{
    class Planet
    {
        //-1 will be replace with some resonable value
        private Random r;
        private Texture2D tex;
        private Rectangle pos;
		private int owner;
		private int[] ships;
        public ContentManager Content
        {
          
            get { return content; }
        }
        ContentManager content;
        

        //can someone look at this class bc i need to kknow what to implement for some method
        public Planet()
        {
            r = new Random();
            //invade_Capacity = rand 
        }
        public Planet(string name, Rectangle pos, IServiceProvider d)
        {
            this.pos = pos;
			owner = 0;
            load(d, name);
        }
        
        public Planet(string name,Rectangle position, Color faction,IServiceProvider serve)
        {
            //this.image = image;
            this.pos = position;
			owner = 0;
            //i want too implement a class to this bc makes it efficient to proccess invasion and capture 
            load(serve,name);
        }
       
        private void load(IServiceProvider serve , string name)
        {
            content = new ContentManager(serve, "Content");
            tex = content.Load<Texture2D>(name);
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

				
			}
			ships = newShips;
		}

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(tex, pos, Incontrol);
        }



    }
}
