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
    class Asteroid
    {
        Random rand;
        private Texture2D image;
        private Rectangle rect;
        private Vector2 speed;
        protected Vector2 origin;
        private int radius;
         
        public Asteroid(Texture2D img ,Vector2 origin,int radius )
        {
            image = img;
            this.origin = origin;
            this.radius = radius;
            rand = new Random();
            speed = new Vector2(rand.Next(1, 5), rand.Next(1, 5));
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, rect, Color.White);
        }

        
    }
}
