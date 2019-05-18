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

namespace Server
{
	class Game
	{
		//information for the specific game
		//List<Planet> planets;
		//List<Player> players;
		//contains all planets and players
		//contains all asteroids
		List<Asteroid> asteroids;
		//list of SolarOrbits 
		List<SoloarOrbit> soloar_orbits;
		//used to create planet
		//IServiceProvider d;
		//sun  object
		//Sun sun;

		//bool planet_is_selected;
		//Planet selected_planet;
		//MouseState old_mouse;		private Color player_faction;

		//Label launching_ships;
		public bool isOver // if all the ships are the same color excluding black
		{
			get
			{
				bool flag = true;
				Color color = Color.Black;
				foreach(SoloarOrbit s in soloar_orbits)
				{
					foreach(Planet p in s.Planets)
					{
						if (p.ShipColor != Color.Black)
						{
							color = p.ShipColor;
							break;
						}
					}
				}
				foreach (SoloarOrbit s in soloar_orbits)
				{
					foreach (Planet p in s.Planets)
					{
						if (p.ShipColor != color)
						{
							flag = false;
							break;
						}
					}
				}
				return flag;
			}
		}
		public Game(int screenWidth, int screenHeight)
		{
			//planets = new List<Planet>();
			//players = new List<Player>();
			asteroids = new List<Asteroid>();

			soloar_orbits = new List<SoloarOrbit>();
			int radius = 200;
			double angular_speed = 1 / 50.0;
			int number_of_planets = 4;
			for (int i = 0; i < 3; i++)
			{
				soloar_orbits.Add(new SoloarOrbit(radius, angular_speed, number_of_planets, screenWidth, screenHeight, i == 2));
				number_of_planets += 2;
				angular_speed = -angular_speed;
				radius += 100;
			}
			//selected_planet = null;
			//planet_is_selected = false;
			//old_mouse = Mouse.GetState();
			//sun
			//sun = new Sun((screenWidth/2)-100, (screenHeight/2)-100);
			number_of_planets = 0;
			//presentage_of_launching_ships = 1;
		}

		public Planet getPlanet(Rectangle pos)
		{
			// this methoud is out of date all planets are now held within the solar obrits list
			for (int i = 0; i < soloar_orbits.Count; i++)
			{
				Planet temp_planet = soloar_orbits[i].getPlanet(pos);
				if (temp_planet != null)
				{
					return temp_planet;
				}
			}


			// will return null if no planet matches that position
			return null;

		}

		public void Input(string message)// all messages are required to be a length of 5
		{
			string[] split = message.Split(' ');
			int planet_id_souce = int.Parse(split[0]);
			int planet_id_target = int.Parse(split[1]);
			int amount = int.Parse(split[2]);
			transfer_troops(getPlanetById(planet_id_souce) , getPlanetById(planet_id_target) , amount);
		}

		public byte[] Encode()
		{
			byte[] map = new byte[100];
			//each planet gets around 5 bytes each
			int index = 0;
			foreach(SoloarOrbit orbit in soloar_orbits)
			{
				byte[] temp = orbit.Encode();
				for (int i = index; i < temp.Length + index; i++) {
					map[i] = temp[i - index];
				}
				index += temp.Length;
			}
			return map;
		}

		private Planet getPlanetById(int id)
		{
			foreach(SoloarOrbit orbit in soloar_orbits)
			{
				Planet temp = orbit.getPlanetById(id);
				if (temp != null) { return temp; }

			}
			return null;
		}


		/*public void handelPlayerInput(MouseState mouse)
		{
			if (mouse.LeftButton == ButtonState.Pressed && !(old_mouse.LeftButton == ButtonState.Pressed))
			{
				Planet planet_at_position = getPlanet(new Rectangle(mouse.X, mouse.Y, 1, 1));
				if (planet_at_position != null)
				{
					// then this planet has been clicked
					//planet_at_position.select();
					if (!planet_is_selected && planet_at_position.Color == player_faction)
					{
						planet_at_position.select();
						selected_planet = planet_at_position;
						planet_is_selected = true;
					}
					else if (planet_is_selected)
					{

						if (planet_at_position != selected_planet)
						{
							// now if there is a secound planet clicked it signifies that the player
							// is to move his trops or attack
							// check and see if the planets are within radius of each other 
							// (x - h)^2 + (y - k)^ 2 = r^2 // cirlce equations but lets use distanc equations instead
							int offset = (planet_at_position.position.Width / 2); // to calcuale form the center of the planet
							Vector2 pos_1 = new Vector2(planet_at_position.position.X + offset, planet_at_position.position.Y + offset);
							Vector2 pos_2 = new Vector2(selected_planet.position.X + offset, selected_planet.position.Y + offset);
							double dist = Math.Sqrt(Math.Pow(pos_1.X - pos_2.X, 2) + Math.Pow(pos_1.Y - pos_2.Y, 2));
							if (dist <= Planet.TRAVEL_RADIUS)
							{
								// then troops can  transfer
								// we need to check if the selcted planet is his color
								if (selected_planet.Color == player_faction)
								{
									// then everything is otherized

									transfer_troops(selected_planet, planet_at_position, (int)Math.Round(presentage_of_launching_ships * selected_planet.Ships));
								}
							}

						}
						else
						{
							// same planet lets deselct
							planet_is_selected = false;
							selected_planet.deselect();
							selected_planet = null;
						}
					}
				}
			}

			if (mouse.RightButton == ButtonState.Pressed && !(old_mouse.RightButton == ButtonState.Pressed))// right click to desect
			{
				planet_is_selected = false;
				if (selected_planet != null)
					selected_planet.deselect();
				selected_planet = null;
			}
			if (planet_is_selected)
			{
				if (mouse.ScrollWheelValue != old_mouse.ScrollWheelValue)
				{
					presentage_of_launching_ships += ((mouse.ScrollWheelValue - old_mouse.ScrollWheelValue) / 120.0f / 100.0f) * 3f; // should we do poportions increse by 3%

				}
				if (presentage_of_launching_ships < 0)
				{
					presentage_of_launching_ships = 0;
				}
				if (presentage_of_launching_ships > 1)
				{
					presentage_of_launching_ships = 1;
				}
				//launching_ships.updateText((int)Math.Round(presentage_of_launching_ships * selected_planet.Ships) + "--" + (Math.Round(presentage_of_launching_ships * 100)) + "%");
				//launching_ships.updatePosition(selected_planet.position.X + selected_planet.position.Width, selected_planet.position.Y);
				//launching_ships.updateColor(selected_planet.Color);
			}

		}*/

		public Color getWinner()
		{
			//Color color = Color.Black;
			foreach (SoloarOrbit s in soloar_orbits)
			{
				foreach (Planet p in s.Planets)
				{
					if (p.ShipColor != Color.Black)
					{
						//color = p.ShipColor;
						return p.ShipColor;
					}
				}
			}
			return Color.Black;
		}

		private void transfer_troops(Planet first, Planet secound, int amount)
		{
			secound.tranfer_troops(first, amount);
			//planet_is_selected = false;
			//selected_planet.deselect();
			//selected_planet = null;
		}

		public void Load()
		{
			for (int i = 0; i < asteroids.Count; i++)
			{
				//asteroids[i].Load(server, "astriod");
			}
			/*for (int i = 0; i < planets.Count; i++)
            {
                planets[i].Load(server);
            }*/
			for (int i = 0; i < soloar_orbits.Count; i++)
			{
				//soloar_orbits[i].Load(server);
			}

			//launching_ships = new Label("" + presentage_of_launching_ships, new Vector2(), Color.Black, (new ContentManager(server, "Content/").Load<SpriteFont>("SpriteFont1")));
			//sun.Load(server);
		}
		public void Update(GameTime gametime)
		{
			//MouseState m = Mouse.GetState();
			//update everything
			for (int i = 0; i < asteroids.Count; i++)
			{
				//asteroids[i].Update(gametime);
			}
			/*for (int i = 0; i < planets.Count; i++)
            {
                planets[i].Update(gametime,m);
				//Console.Write(planets[i].Angle + " ");
            }*/
			for (int i = 0; i < soloar_orbits.Count; i++)
			{
				soloar_orbits[i].Update(gametime);
			}
			//Console.WriteLine(soloar_orbits[0].Planets[0].position.X);

			// Handel player input
			//handelPlayerInput(m);
			//change locations of planets as they rotate
			//sun.Update(gametime);
			//old_mouse = m;
		}

		public void close()
		{
			Planet.TOTAL_NUMBER_OF_PLANETS = 0;
		}
	}
}
