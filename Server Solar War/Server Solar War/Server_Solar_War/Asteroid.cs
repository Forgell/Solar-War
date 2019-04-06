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
        protected Vector2 origin;
        private int radius,time,timer, speed;
        private double angle;
         
        public Asteroid(Texture2D img ,Vector2 origin,int radius )
        {
            image = img;
            this.origin = origin;
            this.radius = radius;
            rand = new Random();
            
            angle = Math.PI / 180.0 * 5.0; 
            //bool = ran
            rect = new Rectangle(rand.Next((int)origin.X, (int)origin.X + radius+2)
                                    , rand.Next((int)origin.Y, (int)origin.Y + radius), 50, 50);
        }
        private Asteroid()
        {
            angle = Math.PI / 180.0 * 5.0;
            speed = rand.Next(1, 5);
            double distance = angle * Math.Pow(radius, 2);
            time = (int)(distance / speed);
        }
        private void Orbit()
        {
            
            double x = (origin.X + Math.Cos(angle) * radius);
            double y = (origin.Y + Math.Sin(angle) * radius);
            rect.X = (int)x;
            rect.Y = (int)y;


        }
        public Boolean hit(Rectangle pos)
        {
            if (rect.Intersects(pos)
                return true;
            return false;
        }
        public void Update(GameTime gameTime)
        {
            Orbit();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, rect, Color.White);
        }

        
    }
}
