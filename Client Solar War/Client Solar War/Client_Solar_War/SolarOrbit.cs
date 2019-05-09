using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client_Solar_War
{
	class SoloarOrbit
	{
		public List<Planet> Planets
		{
			get { return planets; }
		}
		private List<Planet> planets;

		public SoloarOrbit(List<Planet> planets)
		{
			this.planets = planets;
		}

		public SoloarOrbit(int radius, double angular_speed, int number_of_planets, int screenWidth, int screenHeight, bool is_player_start_ring)
		{
			planets = new List<Planet>();
			double angle_between = 360.0 / number_of_planets;
			double currnet_angle = 0.0;
			Vector2 orgin = new Vector2(screenWidth / 2f, screenHeight / 2f);
			int player_start_positions_as_player_numbers = 1;
			for (int i = 0; i < number_of_planets; i++)
			{
				if (i % 2 == 0 && is_player_start_ring)
				{
					Color temp = Color.Black;
					switch (player_start_positions_as_player_numbers)
					{
						case 1: temp = Color.OrangeRed; break;
						case 2: temp = Color.Blue; break;
						case 3: temp = Color.Green; break;
						case 4: temp = Color.Purple; break;
					}
					planets.Add(new Planet("planet-" + player_start_positions_as_player_numbers, orgin, radius, angular_speed, 2, temp));
					player_start_positions_as_player_numbers++;
				}
				else
				{
					planets.Add(new Planet("planet-5", orgin, radius, angular_speed, 2, Color.Black));
				}

				planets[i].setAngle(currnet_angle);
				currnet_angle += angle_between;
			}
		}


		public Planet getPlanet(Rectangle position)
		{
			foreach (Planet planet in planets)
			{
				if (planet.position.Intersects(position))
				{
					return planet;
				}
			}
			return null;
		}

        //chack for win
        public Color isWin()
        {
            if (isRedWin())
                return Color.OrangeRed;
            else if (isBlueWin())
                return Color.Blue;
            else if (isGreenWin())
                return Color.Green;
            else if (isPurpleWin())
                return Color.Purple;
            //else neutral color has "won"
            return Color.Black;
        }

        //check if player orangeRed, blue, green, or purple has won
        private bool isRedWin()
        {
            foreach (Planet planet in planets)
            {
                if (planet.faction != Color.OrangeRed)
                    return false;
            }
            return true;
        }
        private bool isBlueWin()
        {
            foreach (Planet planet in planets)
            {
                if (planet.faction != Color.Blue)
                    return false;
            }
            return true;
        }
        private bool isGreenWin()
        {
            foreach (Planet planet in planets)
            {
                if (planet.faction != Color.Green)
                    return false;
            }
            return true;
        }
        private bool isPurpleWin()
        {
            foreach (Planet planet in planets)
            {
                if (planet.faction != Color.Purple)
                    return false;
            }
            return true;
        }

        public void UpdateInput(MouseState m)
		{
			foreach(Planet p in planets)
			{
				p.UpdateInput(m);
			}
		}

		public void Update_As_Bytes(byte[] map)
		{
			for(int i = 0; i < planets.Count; i++)
			{
				byte[] temp = new byte[5];
				for(int j = 0; j < temp.Length; j++)
				{
					temp[j] = map[i * 5 + j];
				}
				planets[i].Update_As_Bytes(temp);
			}
		}

		public void Load(IServiceProvider server)
		{
			foreach (Planet planet in planets)
			{
				planet.Load(server);
			}
		}


		public void Draw(SpriteBatch spritebatch)
		{
			foreach (Planet planet in planets)
			{
				planet.Draw(spritebatch);
			}
		}
	}
}
