using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server_Solar_War
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

        public SoloarOrbit(int radius, double angular_speed , int number_of_planets)
        {
            planets = new List<Planet>();
            double angle_between = 360 / number_of_planets;
            for(int i = 0; i < number_of_planets; i++)
            {

            }
        }

        public void Update(GameTime gameTime)
        {
            foreach(Planet planet in planets)
            {
                planet.Update(gameTime);
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
