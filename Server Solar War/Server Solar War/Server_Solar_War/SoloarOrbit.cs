﻿using Microsoft.Xna.Framework;
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
            double angle_between = 360.0 / number_of_planets;
            double currnet_angle = 0.0;
            int translateX = 0, translateY = 0;
            /*for(int i = 0; i < number_of_planets; i++)
            {
                
                planets.Add(new Planet( "temp_image",new Vector2((float)(radius * Math.Cos(MathHelper.ToRadians((float)currnet_angle))) + translateX , (float)(radius * Math.Sin(MathHelper.ToRadians((float)currnet_angle))) + translateY) , radius , angular_speed ));
                currnet_angle += angle_between;
            }*/ //fix this later
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