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
    class Asteroid
    {
        Random rand;
        private Texture2D image;
        private Texture2D[] sheet;
        private Texture2D explode;
        private Rectangle rect;
        protected Vector2 origin;
        private int radius,time,timer, speed;
        private double angle;

        public ContentManager Content
        {

            get { return content; }
        }
        ContentManager content;


        public Asteroid(Vector2 origin,int radius )
        {
            
            this.origin = origin;
            this.radius = radius;
            rand = new Random();
            
            angle = Math.PI / 180.0 * 5.0;
            //bool = ran
            position();
            speed = rand.Next(50, 100);
            timer = 0;
            double distance = angle * Math.Pow(radius, 2);
            time = (int)(distance / speed);
        }
        private void position()
        {
            Vector2 pos = new Vector2(rand.Next((int)origin.X - radius, (int)origin.X + radius), rand.Next((int)origin.Y - radius, (int)origin.Y + radius));
            double dis = Math.Sqrt(Math.Pow(origin.X - pos.X, 2) + Math.Pow(origin.Y + pos.Y, 2));
            int d = radius - (int)dis;
            rect = new Rectangle(d + (int)dis, d + (int)dis, rand.Next(25, 50), rand.Next(25, 50));
            Console.WriteLine(rect);
                
        }
        public void Load(IServiceProvider server,String fileName)
        {
            content = new ContentManager(server, "Content");
            String[] file = Directory.GetFiles("Content/Sprites/astroid");
            sheet = new Texture2D [ file.Length];
           // for (int i = 0; i < file.Length; i++)
            //explode = content.Load<Texture2D>("");
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
            if (rect.Intersects(pos))
            {
                explosion();
                return true;
            }
            return false;
        }
        private void explosion()
        {
           // image = explode;
        }
        public void Update(GameTime gameTime)
        {
            timer++;
            if (timer % time == 0)
            {
                angle += .55;
                Orbit();
                //Console.WriteLine("Aster time: " + timer);
            }


        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, rect, Color.White);
        }

        
    }
}
