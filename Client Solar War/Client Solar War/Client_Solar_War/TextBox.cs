using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client_Solar_War
{
	class TextBox
	{
		public Vector2 Position
		{
			get { return position; }
		}
		private Vector2 position;

		public string Text
		{
			get { return text; }
		}
		private string text;

		public SpriteFont Font
		{
			get { return font; }
		}
		private SpriteFont font;

		/// <summary>
		/// this is the only contructor as both the position and the font is needed to be loaded for the draw function to work
		/// </summary>
		/// <param name="position"></param>
		/// <param name="font"></param>
		public TextBox(Vector2 position , SpriteFont font)
		{
			this.position = position;
			this.font = font;
			text = "";
		}
		public TextBox(Vector2 position, SpriteFont font, string str)
		{
			this.position = position;
			this.font = font;
			text = "" + str;
		}

		/// <summary>
		/// replaces the text with the parameter
		/// </summary>
		/// <param name="new_text"></param>
		public void updateText(string new_text)
		{
			text = new_text;
		}

		/// <summary>
		/// will append the letter that is entered but if it is the delte charicter than it will remove the last charicter
		/// </summary>
		/// <param name="letter"></param>
		public void updateText(char letter)
		{
			if ((char)letter == 127)
			{
				if (text.Length != 0)
				{
					text = text.Substring(0, text.Length - 1);
				}
			}
			else
			{
				text += letter;
			}
		}

		/// <summary>
		/// Draws the message of text
		/// </summary>
		/// <param name="spritebatch"></param>
		public void Draw(SpriteBatch spritebatch)
		{
			spritebatch.DrawString(font , text , position, Color.White);
		}


	}
}
