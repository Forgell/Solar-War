using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
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
                        case 1: temp = Color.Red; break;
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


        public Planet getPlanetById(int id)
        {
            foreach(Planet p in planets)
            {
                if (p.ID == id)
                {
                    return p;
                }
            }
            return null;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Planet planet in planets)
            {
                planet.Update(gameTime);
            }
        }

        public byte[] Encode()
        {
            byte[] map = new byte[planets.Count * 5];
            int index = 0;
            foreach(Planet p in planets)
            {
                byte[] temp = p.Encode();
                for(int i = index; i < temp.Length + index; i++)
                {
                    map[index + i] = temp[i - index];
                }
                index += 5;
            }
            return map;
        }

        
    }
}
