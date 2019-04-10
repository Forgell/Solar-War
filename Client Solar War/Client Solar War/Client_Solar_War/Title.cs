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

namespace Client_Solar_War
{
	class TitleScreen
	{
		TitleButton start, join;
		Texture2D textBanner;
		Rectangle rectBanner;

		public TitleScreen(Texture2D textBanner, SpriteFont font, int screenWidth, int screenHeight, GraphicsDevice graphics)
		{

			this.textBanner = textBanner;
			int x = screenWidth / 4;
			int y = screenHeight / 2;
			this.rectBanner = new Rectangle((screenWidth * 4) / 5 - ((screenWidth * 5) / 8), screenHeight / 4 - (screenHeight / 8), (screenWidth * 2) / 3, screenHeight / 4);
			Console.WriteLine();
			Console.WriteLine(rectBanner.Y);
			Console.WriteLine();
			start = new TitleButton("Start", new Vector2(x, y), font);
		}

		public bool update(int x, int y, bool pressed, GameTime gameTime)
		{
			return start.pressed(x, y) && pressed;
		}

		public void draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			spriteBatch.Draw(textBanner, rectBanner, Color.White);
			start.Draw(spriteBatch);
		}
	}
	class TitleButton
	{
		string text;
		Texture2D image;
		Rectangle rect;
		bool isImage;
		SpriteFont font;
		Vector2 pos;
		int width, height;
		Rectangle temp;
		Rectangle temp2;

		public TitleButton(string text, Vector2 pos, SpriteFont font)
		{
			Vector2 dems = font.MeasureString(text);
			this.text = text;
			this.pos = pos;
			isImage = false;
			this.font = font;
			this.width = (int)dems.X;
			this.height = (int)dems.Y;
			temp = new Rectangle();
			temp2 = new Rectangle();
		}

		public TitleButton(Texture2D image, Rectangle rect)
		{
			this.image = image;
			this.rect = rect;
			isImage = true;
			temp = new Rectangle();
		}

		public Boolean pressed(int x, int y)
		{
			if (isImage)
			{
				temp.X = x;
				temp.Y = y;
				temp.Width = 1;
				temp.Height = 1;
				return temp.Intersects(rect);
			}
			// if text
			temp.X = (int)(pos.X);
			temp.Y = (int)pos.Y;
			temp.Width = width;
			temp.Height = height;
			temp2.X = x;
			temp2.Y = y;
			temp2.Width = 1;
			temp2.Height = 1;
			return temp.Intersects(temp2);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (isImage)
			{
				spriteBatch.Draw(image, rect, Color.White);
			}
			else
			{
				spriteBatch.DrawString(font, text, pos, Color.White);
			}
		}
	}
}

