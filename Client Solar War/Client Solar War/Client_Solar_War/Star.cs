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
		private float translation;
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
			translation = starVel.X;
		}
		public Star(Rectangle sr, Color sc, Vector2 sp, Vector2 sv)
		{
			starRect = sr;
			starTex = defaultTex;
			starColor = sc;
			starPos = sp;
			starVel = sv;
			translation = starVel.X;
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
			starPos.X += starVel.X;
			starPos.Y += starVel.Y;
		}

		public void animate(Rectangle screen) 
		{
			// - acceleration then when Vx = 0 translate
			starPos.X += starVel.X;
			starPos.Y += starVel.Y;
			if (starVel.Y != translation) {

				if (starVel.X <= 0)
				{
					starVel.X = 0;
					starVel.Y += 0.01f;
				}
				else
				{
					starVel.X += -0.09f;
				}
				if (starPos.X >= screen.Width)
				{
					starPos.X = starPos.X - screen.Width;
				}
			}
		}
	}
}
