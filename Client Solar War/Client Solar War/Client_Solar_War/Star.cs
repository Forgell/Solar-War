using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client_Solar_War
{
	public class Star
	{
		private Rectangle starRect;
		private Texture2D starTex;
		private Color starColor;
		private Vector2 starVel, starPos;
		public static Texture2D defaultTex;
		public Star()
		{
			starRect = Rectangle.Empty;
			starTex = defaultTex;
			starColor = Color.White;
			starPos = Vector2.Zero;
			starVel = Vector2.Zero;
		}
		public Star(Rectangle sr, Texture2D st, Color sc, Vector2 sp, Vector2 sv)
		{
			starRect = sr;
			starTex = st;
			starColor = sc;
			starPos = sp;
			starVel = sv;
		}
		public Star(Rectangle sr, Color sc, Vector2 sp, Vector2 sv)
		{
			starRect = sr;
			starTex = defaultTex;
			starColor = sc;
			starPos = sp;
			starVel = sv;
		}
		public Rectangle Rect
		{
			get
			{
				return starRect;
			}
			set
			{
				this.starRect = value;
			}
		}
		public Texture2D Tex
		{
			get
			{
				return starTex;
			}
			set
			{
				this.starTex = value;
			}
		}
		public Color Col
		{
			get
			{
				return starColor;
			}
			set
			{
				this.starColor = value;
			}
		}
		public Vector2 Pos
		{
			get
			{
				return starPos;
			}
			set
			{
				this.starPos = value;
			}
		}
		public Vector2 Vel
		{
			get
			{
				return starVel;
			}
			set
			{
				this.starVel = value;
			}
		}
		public void move()
		{
			starPos.X += (int)starVel.X;
			starPos.Y += (int)starVel.Y;
		}

		public void animate()
		{
			starPos.X += (int)(starVel.Y/10.0);
			starPos.Y += (int)(starVel.X / 4.0);
		}
	}
}
