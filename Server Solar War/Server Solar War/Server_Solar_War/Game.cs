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

namespace Server_Solar_War
{
    class Game
    {
        //information for the specific game
        List<Planet> planets;
        List<Player> players;
        //contains all planets and players
        //contains all asteroids
        List<Asteroid> asteroids;
		//list of SolarOrbits 
		List<SoloarOrbit> soloar_orbits;
        //used to create planet
        IServiceProvider d;
        //sun  object
        Sun sun;


        public Game(int screenWidth , int screenHeight , ContentManager Content)
        {
            planets = new List<Planet>();
            players = new List<Player>();
            asteroids = new List<Asteroid>();
			
			


			soloar_orbits = new List<SoloarOrbit>();
			int radius = 200;
			double angular_speed = 1/25.0;
			int number_of_planets = 4;
			for(int i = 0; i < 3; i++)
			{
				soloar_orbits.Add(new SoloarOrbit(radius, angular_speed, number_of_planets, screenWidth, screenHeight , i == 2));
				number_of_planets += 2;
				angular_speed = -angular_speed;
				radius += 100;
			}
		
            
            //sun
            sun = new Sun((screenWidth/2)-100, (screenHeight/2)-100);
		}

        public Planet getPlanet(Rectangle pos) 
        {
			// this methoud is out of date all planets are now held within the solar obrits list
			for (int i = 0; i < planets.Count; i++)
            {
                if(pos.Intersects((planets.ElementAt<Planet>(i)).position))
                {
                    return planets.ElementAt<Planet>(i);
                }
            }
			//this shouldn't happen:
			//return new Planet("Earth",new Rectangle(new Vector2(200,10) , 1,1),20);
			//how do we prevent this or how do we get in fo that this happened
			//insert file name and read the file
			//.in order to orbit
			//oorigin = sun location
			//radius = distance from sun
			return null;

        }
        public void Load(IServiceProvider server)
        {
            for (int i = 0; i < asteroids.Count; i++)
            {
                asteroids[i].Load(server,"astriod");
            }
            for (int i = 0; i < planets.Count; i++)
            {
                planets[i].Load(server);
            }
			for (int i = 0; i < soloar_orbits.Count; i++)
			{
				soloar_orbits[i].Load(server);
			}
            sun.Load(server);
        }
        public void Update(GameTime gametime)
        {
            MouseState m = Mouse.GetState();
            //update everything
            for (int i = 0; i < asteroids.Count; i++)
            {
                asteroids[i].Update(gametime);
            }
            for (int i = 0; i < planets.Count; i++)
            {
                planets[i].Update(gametime,m);
				//Console.Write(planets[i].Angle + " ");
            }
			for(int i = 0; i < soloar_orbits.Count; i++)
			{
				soloar_orbits[i].Update(gametime , m);
			}
            //change locations of planets as they rotate
            sun.Update(gametime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //draw everything

            //draw sun/star
            sun.Draw(spriteBatch);
            //draw asteroids
            for(int i = 0; i < asteroids.Count; i++)
            {
                asteroids[i].Draw(spriteBatch);
            }
            //will need to change planet class to be able to draw each planet
            for (int i = 0; i < planets.Count; i++)
            {
                planets[i].Draw(spriteBatch);
            }

			for(int i = 0; i < soloar_orbits.Count; i++)
			{
				soloar_orbits[i].Draw(spriteBatch);
			}
        }
    }
}
