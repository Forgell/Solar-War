using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Client_Solar_War
{
    class Starfield
    {
        Star[] stars;
        Random r;
        int x;
        Rectangle screen;

        public Starfield(GraphicsDevice graphics, Texture2D texture)
        {
            screen = new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height);
            r = new Random();
            x = 0;
            Star.defaultTex = texture;
            stars = new Star[150];
        }

        public void update(GraphicsDeviceManager graphics)
        {

            if (x < stars.Length)
            {
                for (int i = 0; i < 25 && i + x < stars.Length; i++)
                    stars[x + i] = newStar(graphics.GraphicsDevice);
                x += 25;
            }
            //if (!graphics.IsFullScreen && Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                graphics.ToggleFullScreen();
            }
            for (int i = 0; i < stars.Length; i++)
            {
                try
                {
                    stars[i].move();
                }
                catch (NullReferenceException)
                {
                    break;
                }
                if (!screen.Contains(new Point((int)stars[i].Pos.X, (int)stars[i].Pos.Y)))
                {
                    stars[i] = newStar(graphics.GraphicsDevice);
                }
            }
        }

		public void animate()
		{
			foreach (Star star in stars)
			{
				star.animate(screen);
			}
		}

        public void draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                try
                {
                    spriteBatch.Draw(stars[i].Tex, stars[i].Pos, stars[i].Rect, stars[i].Col);
                }
                catch (ArgumentNullException)
                {
                    stars[i].Tex = Star.defaultTex;
                }
                catch (NullReferenceException)
                {
                    break;
                }
            }
        }

        protected Star newStar(GraphicsDevice graphics)
        {
            //Star temp = new Star(new Rectangle(0, 0, 6, 6), new Color(r.Next(255), r.Next(255), r.Next(255)), new Vector2(r.Next(GraphicsDevice.Viewport.Width - 50), r.Next(GraphicsDevice.Viewport.Height - 50)), new Vector2(r.Next(20) - 10, r.Next(20) - 10));
            return new Star(new Rectangle(0, 0, 3, 3), new Color(r.Next(255), r.Next(255), r.Next(255), r.Next(255)), new Vector2(r.Next(0), r.Next(graphics.Viewport.Height + 1)), new Vector2(r.Next(17) + 5, 0));
            //if (temp.Vel.Equals(Vector2.Zero))
            //{
            //	temp.Vel = new Vector2(1, 1);
            //}
            //return temp;
        }
    }
}
