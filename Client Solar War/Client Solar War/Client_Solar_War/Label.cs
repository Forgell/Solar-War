using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client_Solar_War
{
	class Label
	{
		public string Text
		{
			get { return text; }
		}
		private string text;

		public Vector2 Position
		{
			get { return position; }
		}
		private Vector2 position;

		public SpriteFont Font
		{
			get { return font; }
		}
		private SpriteFont font;

		public Color Color
		{
			get { return Color; }
		}
		private Color color;

		public Label(string text, Vector2 position, Color color, SpriteFont font)
		{
			this.text = text;
			this.position = position;
			this.font = font;
			this.color = color;
		}

		public void updateText(string new_text)
		{
			text = new_text;
		}

		public void updatePosition(int x, int y)
		{
			position.X = x;
			position.Y = y;
		}

		public void updateColor(Color color)
		{
			this.color = color;
		}
		public void Draw(SpriteBatch spritebatch)
		{
			spritebatch.DrawString(font, text, position, color);
		}
	}
}
