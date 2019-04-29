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

namespace Client_Solar_War
{
	class KeyboardHelper
	{
		public static char getIPInput(KeyboardState console , KeyboardState old)
		{
			if (console.IsKeyDown(Keys.NumPad0) && !old.IsKeyDown(Keys.NumPad0))
			{
				return '0';
			}
			else if (console.IsKeyDown(Keys.NumPad1) && !old.IsKeyDown(Keys.NumPad1))
			{
				return '1';
			}
			else if (console.IsKeyDown(Keys.NumPad2) && !old.IsKeyDown(Keys.NumPad2))
			{
				return '2';
			}
			else if (console.IsKeyDown(Keys.NumPad3) && !old.IsKeyDown(Keys.NumPad3))
			{
				return '3';
			}
			else if (console.IsKeyDown(Keys.NumPad4) && !old.IsKeyDown(Keys.NumPad4))
			{
				return '4';
			}
			else if (console.IsKeyDown(Keys.NumPad5) && !old.IsKeyDown(Keys.NumPad5))
			{
				return '5';
			}
			else if (console.IsKeyDown(Keys.NumPad6) && !old.IsKeyDown(Keys.NumPad6))
			{
				return '6';
			}
			else if (console.IsKeyDown(Keys.NumPad7) && !old.IsKeyDown(Keys.NumPad7))
			{
				return '7';
			}
			else if (console.IsKeyDown(Keys.NumPad8) && !old.IsKeyDown(Keys.NumPad8))
			{
				return '8';
			}
			else if (console.IsKeyDown(Keys.NumPad9) && !old.IsKeyDown(Keys.NumPad9))
			{
				return '9';
			}
			else

			if (console.IsKeyDown(Keys.D0) && !old.IsKeyDown(Keys.D0))
			{
				return '0';
			}
			else if (console.IsKeyDown(Keys.D1) && !old.IsKeyDown(Keys.D1))
			{
				return '1';
			}
			else if (console.IsKeyDown(Keys.D2) && !old.IsKeyDown(Keys.D2))
			{
				return '2';
			}
			else if (console.IsKeyDown(Keys.D3) && !old.IsKeyDown(Keys.D3))
			{
				return '3';
			}
			else if (console.IsKeyDown(Keys.D4) && !old.IsKeyDown(Keys.D4))
			{
				return '4';
			}
			else if (console.IsKeyDown(Keys.D5) && !old.IsKeyDown(Keys.D5))
			{
				return '5';
			}
			else if (console.IsKeyDown(Keys.D6) && !old.IsKeyDown(Keys.D6))
			{
				return '6';
			}
			else if (console.IsKeyDown(Keys.D7) && !old.IsKeyDown(Keys.D7))
			{
				return '7';
			}
			else if (console.IsKeyDown(Keys.D8) && !old.IsKeyDown(Keys.D8))
			{
				return '8';
			}
			else if (console.IsKeyDown(Keys.D9) && !old.IsKeyDown(Keys.D9))
			{
				return '9';
			}
			else
				if (console.IsKeyDown(Keys.OemPeriod) && !old.IsKeyDown(Keys.OemPeriod) || console.IsKeyDown(Keys.Decimal) && !old.IsKeyDown(Keys.Decimal))
			{
				return '.';
			}
			else

			if (console.IsKeyDown(Keys.Enter) && !old.IsKeyDown(Keys.Enter))
			{
				return (char)4; // end of transmission
			}
			else if (console.IsKeyDown(Keys.Back) && !old.IsKeyDown(Keys.Back))
			{
				return (char)127; // delete charicter
			}
			return (char)0; // null
		}

		public static Keys getKeyboardGameInput(KeyboardState console , KeyboardState old)
		{
			Keys key;
			for(int i = 0; i < console.GetPressedKeys().Length; i++)
			{
				key = console.GetPressedKeys()[i];
				if (old.IsKeyUp(key))
				{
					return key;
				}
			}
			
			return Keys.LeftAlt;
		}
    }
}

