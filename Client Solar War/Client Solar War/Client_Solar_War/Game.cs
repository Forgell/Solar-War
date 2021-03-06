﻿using System;

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
		List<Line> lines;

		bool planet_is_selected;
		Planet selected_planet;
		MouseState old_mouse;
		private Color player_faction;

		Label launching_ships;
		float presentage_of_launching_ships;
		//Background background;
		Texture2D line_text;
		Texture2D numbers_text;

        //for checking for a win, player color that has won (or black)
        List<Color> winColor;
        //Color that has won: orangeRed, blue, green, or purple, else black(neutral)
        Color colorWon;
        //to draw text for a player winning the game
        //spriteBatch.DrawString(spriteFont, "", Vector2 position, Color.OrangeRed);
        string winText;
        Vector2 winPosition;
        SpriteFont spriteFont;

        public Game(int screenWidth, int screenHeight, ContentManager Content, Color player_faction)
		{
			//planets = new List<Planet>();
			//players = new List<Player>();
			asteroids = new List<Asteroid>();
			this.player_faction = player_faction;

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
			selected_planet = null;
			planet_is_selected = false;
			old_mouse = Mouse.GetState();
			//sun
			//sun = new Sun((screenWidth/2)-100, (screenHeight/2)-100);
			presentage_of_launching_ships = 1;
			//background = new Background();
			lines = new List<Line>();
		}
		public void reset()
		{
			foreach (SoloarOrbit s in soloar_orbits)
			{
				foreach (Planet p in s.Planets)
				{
					p.reset();
				}
			}
			if (selected_planet != null)
			{
				selected_planet.deselect();
			}
			selected_planet = null;
			planet_is_selected = false;
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

		public void setPlayer(Color faction)
		{
			player_faction = faction;
		}


		public string handlePlayerInput(MouseState mouse)
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

									//transfer_troops(selected_planet, planet_at_position, (int)Math.Round(presentage_of_launching_ships * selected_planet.Ships));
									int faction = 0;
									if (player_faction == Color.OrangeRed)
									{
										faction = 0;
									}
									if (player_faction == Color.Blue)
									{
										faction = 1;
									}
									if (player_faction == Color.Green)
									{
										faction = 2;
									}
									if (player_faction == Color.Purple)
									{
										faction = 3;
									}
									if (player_faction == Color.Black)
									{
										faction = 4;
									}
                                    int ship_amount = (int)Math.Round(presentage_of_launching_ships * selected_planet.Ships);
                                    if (ship_amount > 99)
                                    {
                                        Console.WriteLine("Error more than accitibel amunt\n--" + presentage_of_launching_ships + "\n--" + selected_planet.Ships);
                                    }

                                    Console.WriteLine(selected_planet.ID + " " + planet_at_position.ID + " " + ((int)Math.Round(presentage_of_launching_ships * selected_planet.Ships)) + " " + faction);
									return selected_planet.ID + " " + planet_at_position.ID + " " + ((int)(presentage_of_launching_ships *100)) + " "  + faction;
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
				launching_ships.updateText((int)Math.Round(presentage_of_launching_ships * selected_planet.Ships) + "--" + (Math.Round(presentage_of_launching_ships * 100)) + "%");
				launching_ships.updatePosition(selected_planet.position.X + selected_planet.position.Width, selected_planet.position.Y);
				launching_ships.updateColor(selected_planet.Color);
			}

			return "";
		}

		private void transfer_troops(Planet first, Planet secound, int amount)
		{
			secound.tranfer_troops(first, amount);
			planet_is_selected = false;
			selected_planet.deselect();
			selected_planet = null;
		}

		public void Load(IServiceProvider server)
		{
			ContentManager Content = new ContentManager(server , "Content/" );
            for (int i = 0; i < asteroids.Count; i++)
			{
				asteroids[i].Load(server, "astriod");
			}
			/*for (int i = 0; i < planets.Count; i++)
            {
                planets[i].Load(server);
            }*/
			for (int i = 0; i < soloar_orbits.Count; i++)
			{
				soloar_orbits[i].Load(server);
			}

			launching_ships = new Label("" + presentage_of_launching_ships, new Vector2(), Color.Black, Content.Load<SpriteFont>("SpriteFont1"));
			//sun.Load(server);
			//background.Load(Content);
			line_text = Content.Load<Texture2D>("Star");
			numbers_text = Content.Load<Texture2D>("Sprites/text/numbers");
		}

        //check who wins, return player color of winner, else return black
        private Color WhoWon()
        {
            winColor = new List<Color>();
            //win condition
            //4 players, check 4 colors
            //soloar_orbits
            for (int i = 0; i < soloar_orbits.Count; i++)
            {
                winColor.Add(soloar_orbits[i].isWin());
            }
            //check each player color
            //orangeRed, blue, green, or purple, else black
            if (checkColorWonRows(winColor, Color.OrangeRed))
                return Color.OrangeRed;
            if (checkColorWonRows(winColor, Color.Blue))
                return Color.Blue;
            if (checkColorWonRows(winColor, Color.Green))
                return Color.Green;
            if (checkColorWonRows(winColor, Color.Purple))
                return Color.Purple;
            return Color.Black;
        }

        private bool checkColorWonRows(List<Color> colorRows, Color faction)
        {
            //chack each row for a specific color
            for (int i = 0; i < colorRows.Count; i++)
            {
                if (faction != colorRows[i])
                    return false;
            }
            return true;
        }

		public string Update_Input()
		{
			MouseState m = Mouse.GetState();

			foreach (SoloarOrbit orbit in soloar_orbits)
			{
				orbit.UpdateInput(m);
			}
			//string s = handlePlayerInput(m);
			//handlePlayerInput(m);
			//change locations of planets as they rotate
			//sun.Update(gametime);
			old_mouse = m;
			return handlePlayerInput(m); 
		}

		public void Update_as_Bytes(byte[] map)
		{
			int index = 0;
			for (int i = 0; i < soloar_orbits.Count; i++)
			{
				byte[] temp = new byte[soloar_orbits[i].Planets.Count * 5];
				for (int j = 0; j < temp.Length; j++)
				{
					temp[j] = map[index + j];
				}
				index += temp.Length;
				soloar_orbits[i].Update_As_Bytes(temp);
			}
		}

		public string Update(GameTime gametime)
		{
            //Win condition
            colorWon = WhoWon();
            MouseState m = Mouse.GetState();
			//update everything
			for (int i = 0; i < asteroids.Count; i++)
			{
				asteroids[i].Update(gametime);
			}
			
			// Handel player input
			string ss = handlePlayerInput(m);
			//background.Update();
			foreach(SoloarOrbit s in soloar_orbits)
			{
				s.Update(gametime);
			}
			//change locations of planets as they rotate
			//sun.Update(gametime);
			old_mouse = m;
			return ss;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			//draw everything

			//draw sun/star
			//sun.Draw(spriteBatch);
			//draw asteroids
			// draw lines first
			for(int i = 0; i < soloar_orbits.Count; i++)
			{
				for(int j = 0; j < soloar_orbits.Count; j++)
				{
					if (i == j)
					{
						continue;
					}
					for(int h = 0; h < soloar_orbits[i].Planets.Count; h++)
					{
						for(int g = 0; g < soloar_orbits[j].Planets.Count; g++)
						{

							Planet one = soloar_orbits[i].Planets[h];
							Planet two = soloar_orbits[j].Planets[g];
							if (one.Color == player_faction || two.Color == player_faction)
							{
								// see if planets are in range
								int offset = (one.position.Width / 2); // to calcuale form the center of the planet
								Vector2 pos_1 = new Vector2(one.position.X + offset, one.position.Y + offset);
								Vector2 pos_2 = new Vector2(two.position.X + offset, two.position.Y + offset);
								double dist = Math.Sqrt(Math.Pow(pos_1.X - pos_2.X, 2) + Math.Pow(pos_1.Y - pos_2.Y, 2));
								if (dist <= Planet.TRAVEL_RADIUS)
								{
									DrawLine(spriteBatch, pos_1, pos_2);
								}
							}
							
						}
					}
				}
			}
			//background.Draw(spriteBatch);
			for (int i = 0; i < asteroids.Count; i++)
			{
				asteroids[i].Draw(spriteBatch);
			}

			for (int i = 0; i < soloar_orbits.Count; i++)
			{
				soloar_orbits[i].Draw(spriteBatch);
			}
			if (planet_is_selected)
			{
				//launching_ships.Draw(spriteBatch);
				// TODO draw the launching ships as the pixel font
				try
				{
					int offset = 20;
					Vector2 launching_ships_label_pos = new Vector2(selected_planet.position.X + selected_planet.position.Width + offset, selected_planet.position.Y + (selected_planet.position.Height / 4));
					draw_numbers_modified((int)(presentage_of_launching_ships * selected_planet.Ships), launching_ships_label_pos , spriteBatch);
					launching_ships_label_pos.X += 45;
					draw_numbers_modified( (int)(presentage_of_launching_ships * 100), launching_ships_label_pos , spriteBatch);
				}catch(Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
		}

		public void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end)
		{
			Vector2 edge = end - start;
			// calculate angle to rotate line
			float angle =
				(float)Math.Atan2(edge.Y, edge.X);


			sb.Draw(line_text,
				new Rectangle(// rectangle defines shape of line and position of start of line
					(int)start.X,
					(int)start.Y,
					(int)edge.Length(), //sb will strech the texture to fill this rectangle
					1), //width of line, change this to make thicker line
				null,
				player_faction, //colour of line
				angle,     //angle of line (calulated above)
				new Vector2(0, 0), // point in line about which to rotate
				SpriteEffects.None,
				0);

		}

		private void draw_numbers_modified(int number, Vector2 pos, SpriteBatch spritebatch)
		{
			int counter = 0;
			Rectangle numbers_dest_rect = new Rectangle((int)pos.X , (int)pos.Y , 15 , 15);
			Rectangle numbers_souce_rect = new Rectangle();
			foreach (Char c in ("" + number))
			{
				int num = c - '0';
				numbers_souce_rect = new Rectangle(num * 60, 0, 60, 60);
				numbers_dest_rect.X = (15 * counter++) + (int)pos.X;
				spritebatch.Draw(numbers_text, numbers_dest_rect, numbers_souce_rect, player_faction);
			}
		}
	}
}

