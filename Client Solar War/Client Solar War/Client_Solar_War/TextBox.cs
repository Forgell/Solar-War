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

		public Texture2D Font
		{
			get { return font; }
		}
		private Texture2D font;

		/// <summary>
		/// this is the only contructor as both the position and the font is needed to be loaded for the draw function to work
		/// </summary>
		/// <param name="position"></param>
		/// <param name="font"></param>
		public TextBox(Vector2 position , Texture2D font)
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
		public void Draw(SpriteBatch spritebatch , int x , int y)
		{
			Rectangle rect = new Rectangle(x , y , 60 , 60);
			Rectangle dest = new Rectangle( 0 , 0, 60 , 60);
			foreach(Char c in text)
			{
				if (c != '.')
				{
					int num = c - '0';
					dest.X = num * 60;
					spritebatch.Draw(font , rect , dest , Color.White );
				}
				else
				{
					dest.X = 60*10;
					spritebatch.Draw(font, rect, dest, Color.White);
				}
				rect.X += rect.Width;
			}
			
		}


	}
}
