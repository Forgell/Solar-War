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
using System.Threading;

namespace Client_Solar_War
{
	class Background
	{
		private Star_Background[] stars;

		public Background()
		{
			stars = new Star_Background[1];
			for(int i = 0; i < stars.Length; i++)
			{
				stars[i] = new Star_Background( 100 , 100);
			}
		}

		public void Load(ContentManager Content)
		{
			for(int i = 0; i < stars.Length; i++)
			{
				stars[i].Load(Content);
			}
		}

		public void Update()
		{
			for (int i = 0; i < stars.Length; i++)
			{
				stars[i].Update();
			}
		}

		public void Draw(SpriteBatch spritebatch)
		{
			for (int i = 0; i < stars.Length; i++)
			{
				stars[i].Draw(spritebatch);
			}
		}

		private class Star_Background
		{
			private Rectangle pos;
			public Rectangle position
			{
				get { return pos; }
			}

			private Texture2D[] textures;
			private int timer;
			public Star_Background(int x,  int y)
			{
				pos = new Rectangle(x , y , 1 , 1);
				timer = 0;
				textures = new Texture2D[6];
			}

			public void Load(ContentManager Content)
			{
				for(int i = 0; i < textures.Length; i++)
				{
					textures[i] = Content.Load<Texture2D>("Sprites/Star/Star-" + i);
				}
				pos.Width = textures[0].Width;
				pos.Height = textures[0].Height;
			}

			public void Update()
			{
				timer++;
				if(timer >= 6000)
				{
					timer = 0;
				}
			}

			public void Draw(SpriteBatch spritebatch)
			{
				spritebatch.Draw(textures[timer / 1000], pos, Color.White);
			}
		}
	}
	
}
