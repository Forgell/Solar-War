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
	class Line
	{
		private Texture2D text;
		private Rectangle rect;
		public Line()
		{

		}

		public void Load(ContentManager Content)
		{
			text = Content.Load<Texture2D>("Star");
		}

		public void Draw(SpriteBatch spritebatch , int x1, int y1, int x2 , int y2)
		{
			int dist = (int)Math.Sqrt(Math.Pow(x1 - x2 , 2) + Math.Pow(y1 - y2 , 2));
			float deltaX = x2 - x1;
			float deltaY = y2 - y1;
			float rotation = (float)Math.Atan(deltaY / deltaX);
			if (deltaX < 0)
			{
				rotation += (float)Math.PI;
			}
			spritebatch.Draw(text, new Rectangle(x1 , y1 , dist , dist), new Rectangle(0 , 0 , 1 , 1), Color.White, 0.0f, new Vector2(x1, y1), SpriteEffects.None, 0.0f);
		}

		
	}
}
