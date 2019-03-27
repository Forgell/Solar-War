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


namespace planetary
{
    class planet
    {
        //-1 will be replace with some resonable value
        private Random rand = new Random();
        private Texture2D image;
        private Rectangle position;
        private bool capture;
        private Color Incontrol;
        private int invade_Capacity = 1; // need to -use random
        private int Ships_settle = -1;//temp wanting to implement a class
        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;



        public planet(string name, Rectangle pos)
        {
            position = pos;
            capture = false;
            Incontrol = Color.Gray;
            //load
        }
        
        public planet(string name,Rectangle position, Color faction)
        {
            //this.image = image;
            this.position = position;
            Incontrol = faction;
            capture = true; 
            //load(Iservis,name);
        }     

        private void load(IServiceProvider _serviceProvider , string name)
        {
            content = new ContentManager(_serviceProvider, "Content");
            image = content.Load<Texture2D>(name);
            

        }



        //if it being invaded by a a 
        public void invade() //some class or paerameter like color
        {
            if (capture)
            {
                //fight and determine the winner
            }
            else
                invasion(-1);
                
        }
        private Boolean invasion(int numS)
        {
            if(numS >= invade_Capacity)
            {
                Incontrol = Color.Gray;
                Ships_settle = numS / -1;  //aplly statistic or probility depending of number dead or lost...
                   return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(image, position, Incontrol);
        }



    }
}
