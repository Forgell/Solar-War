using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        public SoloarOrbit(int radius, double angular_speed , int number_of_planets , int screenWidth , int screenHeight)
        {
            planets = new List<Planet>();
            double angle_between = 360.0 / number_of_planets;
            double currnet_angle = 0.0;
			Vector2 orgin = new Vector2(screenWidth / 2f , screenHeight/2f);
            for(int i = 0; i < number_of_planets; i++)
            {

				planets.Add(new Planet("planet-5" , orgin , radius , angular_speed , 2 , Color.Black));
				planets[i].setAngle(currnet_angle);
				currnet_angle += angle_between;
            }
        }

		public void Load(IServiceProvider server)
		{
			foreach (Planet planet in planets)
			{
				planet.Load(server);
			}
		}

        public void Update(GameTime gameTime , MouseState m)
        {
            foreach(Planet planet in planets)
            {
                planet.Update(gameTime , m);
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
