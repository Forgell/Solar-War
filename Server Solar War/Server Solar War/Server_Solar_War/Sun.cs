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
    class Sun
    {
        private Texture2D display;
        private Texture2D[] img;
        private Rectangle rect;
        private int radiusH;
        private Boolean change;

        public Sun(int screenWidth,int ScreeHeight)
        {
            rect = new Rectangle(screenWidth/2,ScreeHeight/2, 550, 550);
            radiusH = 20;
            change = true;
            //change the radius 
        }

        public ContentManager Content
        {

            get { return content; }
        }
        ContentManager content;


        public void Load(IServiceProvider server)
        {
            content = new ContentManager(server, "Content");
            img = new Texture2D[2];
            img[0] = content.Load<Texture2D>("Sun/sun");
            img[1] = content.Load<Texture2D>("Sun/sun2");

        }

        private void animaton()
        {


            if (change)
            {
                change = false;
                display = img[0];
            }
            else
            {
                change = true;
                display = img[1];
            }
        }

        public void Update(GameTime gt)
        {
                animaton();

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(display, rect, Color.White);
        }
    }

}
