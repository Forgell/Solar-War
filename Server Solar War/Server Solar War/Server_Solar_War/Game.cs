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
			//orbits = new List<SolarOrbit>();
			//later, write number of/each planets and players and asteroids
			//asteroids.Add(new Asteroid(new Vector2(200, 200), 50));
			//fill orbits with planets
			Console.WriteLine(Content.RootDirectory);
			//int radius = 200;
			//double speed = 1.0 /5.0;
			//planets.Add(new Planet("planet-2", new Vector2(screenWidth / 2 , screenHeight / 2) , radius , speed, 2, Content, 3));
			//planets[0].setAngle(0);
			//planets.Add(new Planet("planet-1", new Vector2(screenWidth / 2, screenHeight / 2), radius, speed, 2, Content, 0));
			//planets[1].setAngle(90);
			//planets.Add(new Planet("planet-3", new Vector2(screenWidth / 2, screenHeight / 2), radius, speed, 2, Content, 1));
			//planets[2].setAngle(180);
			//planets.Add(new Planet("planet-4", new Vector2(screenWidth / 2, screenHeight / 2), radius, speed, 2, Content, 2));
			//planets[3].setAngle(270);


			// this is the one that works
			/*double speed = 1.0 / 25;
            int numberOfRows = 3;
            int planet;
            int count = 0;
            int numberOfPlanets;//per row
            int anglePerPlanet;
            //colorful planets
            for (int j = 0; j < numberOfRows; j++) //number of rows
            {
                radius = (j * 100) + 200; //radius increases by 100 each time
                planet = 1;
                numberOfPlanets = (radius - 100) / 25;
                anglePerPlanet = (360 / numberOfPlanets);
                speed = -speed;
                for (int i = 0; i < numberOfPlanets; i++) //number of planets per row
                {

                    planets.Add(new Planet("planet-" + planet, new Vector2(screenWidth / 2, screenHeight / 2), radius, speed, 2, planet - 1));
                    //planets[count].setAngle(((planet - 1) * 90));
                    planets[count].setAngle(anglePerPlanet * i);
                    planet++;
                    count++;
                    if (planet == 5)
                    {
                        planet = 1;
                    }
                }
            }*/
			soloar_orbits = new List<SoloarOrbit>();
			int radius = 200;
			double angular_speed = 1/25.0;
			int number_of_planets = 4;
			for(int i = 0; i < 3; i++)
			{
				soloar_orbits.Add(new SoloarOrbit(radius, angular_speed, number_of_planets, screenWidth, screenHeight));
				number_of_planets += 2;
				angular_speed = -angular_speed;
				radius += 100;
			}
		
            //only neutral planets
            //for (int j = 0; j < numberOfRows; j++) //number of rows
            //{
            //    radius = (j * 100) + 200; //radius increases by 100 each time
            //    numberOfPlanets = (radius - 100) / 25;
            //    anglePerPlanet = (360 / numberOfPlanets);
            //    speed = -speed;
            //    for (int i = 0; i < numberOfPlanets; i++) //number of planets per row
            //    {

            //        planets.Add(new Planet("planet-" + 5, new Vector2(screenWidth / 2, screenHeight / 2), radius, speed, 2, Content, 4));
            //        //planets[count].setAngle(((planet - 1) * 90));
            //        planets[count].setAngle(anglePerPlanet * i);
            //        count++;
            //    }
            //}

            //first orbit
            //planets.Add(new Planet("planet-1", new Vector2(screenWidth / 2, screenHeight / 2), radius, speed, 2, Content, 0));
            //planets[0].setAngle(0);
            //planets.Add(new Planet("planet-2", new Vector2(screenWidth / 2, screenHeight / 2), radius, speed, 2, Content, 1));
            //planets[1].setAngle(90);
            //planets.Add(new Planet("planet-3", new Vector2(screenWidth / 2, screenHeight / 2), radius, speed, 2, Content, 2));
            //planets[2].setAngle(180);
            //planets.Add(new Planet("planet-4", new Vector2(screenWidth / 2, screenHeight / 2), radius, speed, 2, Content, 3));
            //planets[3].setAngle(270);
            //Console.WriteLine(planets[0].Angle + " " + planets[1].Angle);


            //asteroid 
            //asteroids.Add(new Asteroid(new Vector2(100, 600), 50));
            //asteroids.Add(new Asteroid(new Vector2(300, 150), 50));
            //sun
            sun = new Sun((screenWidth/2)-100, (screenHeight/2)-100);
		}

        public Planet getPlanet(Rectangle pos) 
        {
            for(int i = 0; i < planets.Count; i++)
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
            //  Console.WriteLine("mouse: " + m.X);
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
            //Console.WriteLine();
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
