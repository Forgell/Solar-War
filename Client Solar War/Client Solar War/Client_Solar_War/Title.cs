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
		private TitleButton start, join;
		private Texture2D solar_text , war_text;
		private Texture2D planet_text;
		private Rectangle solar_rect, war_rect, planet_rect , planet_source;
		private Rectangle screen;
		private int timer;
		public TitleScreen(int screenWidth, int screenHeight)
		{
			screen = new Rectangle(0 , 0 , screenWidth , screenHeight);
			timer = 0;
		}

		public void Load(ContentManager Content)
		{
			solar_text = Content.Load<Texture2D>("Sprites/text/solar2");
			war_text = Content.Load<Texture2D>("Sprites/text/war2");
			planet_text = Content.Load<Texture2D>("Sprites/planets/planet-0");
			solar_rect = new Rectangle( screen.Width/10 , screen.Height/10 , screen.Width / 2  , (int)((screen.Width / (2.0 * solar_text.Width)) * solar_text.Height) );
			start = new TitleButton( Content.Load<Texture2D>("Sprites/text/start") , new Rectangle(screen.Width/2 - 75 , screen.Height/2 , 150 ,30));
			planet_rect = new Rectangle(solar_rect.X + solar_rect.Width/5 , solar_rect.Y , solar_rect.Width/5 , solar_rect.Height);
			planet_source = new Rectangle(0, 0  , 180 , 180);
			war_rect = new Rectangle(solar_rect.X + solar_rect.Width + 120 ,solar_rect.Y ,solar_rect.Width /2 , solar_rect.Height);
		}

		public bool update(int x, int y, bool pressed, GameTime gameTime)
		{
			timer++;
			if (timer >= 30)
			{
				planet_source.X += 180;
				if (planet_source.X >= planet_text.Width)
				{
					planet_source.X = 0;
				}
				timer = 0;
			}
			return start.pressed(x, y) && pressed;
		}

		public void draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			spriteBatch.Draw(solar_text , solar_rect , Color.White);
			spriteBatch.Draw(planet_text ,planet_rect , planet_source , Color.White);
			spriteBatch.Draw(war_text , war_rect , Color.White);
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

