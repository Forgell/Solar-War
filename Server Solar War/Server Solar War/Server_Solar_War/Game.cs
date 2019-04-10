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

        //used to create planet
        IServiceProvider d;

        public Game()
        {
            planets = new List<Planet>();
            players = new List<Player>();
            asteroids = new List<Asteroid>();
            //later, write number of/each planets and players and asteroids
            asteroids.Add(new Asteroid(new Vector2(200, 200), 50));
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
            return new Planet("", new Rectangle(-2, -2, 1, 1), d);
            //how do we prevent this or how do we get in fo that this happened
        }
        public void Load(IServiceProvider server)
        {
            for (int i = 0; i < asteroids.Count; i++)
            {
                asteroids[i].Load(server);
            }
        }
        public void Update(GameTime gametime)
        {
            //update everything
            //change locations of planets as they rotate
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //draw everything
            //will need to change planet class to be able to draw each planet
            for(int i = 0; i < planets.Count; i++)
            {
                planets[i].Draw(spriteBatch);
            }
            //draw sun/star
            //draw asteroids
            for(int i = 0; i < asteroids.Count; i++)
            {
                asteroids[i].Draw(spriteBatch);
            }
        }
    }
}
