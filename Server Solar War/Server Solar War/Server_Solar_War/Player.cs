using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace Server_Solar_War
{
    class Player
    {
        //All planets controlled by this player
        List<Planet> planets;
        //Should we also create a list for groups of ships that are not at a planet?
        //List<Ship> ships


        public Player()
        {
            planets = new List<Planet>();
            
        }

        public void moveShips(Planet planet, Planet planet2)//planet1 is moving from, planet2 is moving to
        {
            //add a Ship to the ship list class at the position that the planet is at
            //move Ship toward planet2
        }

        //public void moveShips(Ship ship1, Planet planet2)//Ship is moving from, planet2 is moving to
        //{
        //    //move Ship toward planet2
        //}
        
        public void addPlanet(Planet planet1)
        {
            planets.Add(planet1);
        }

    }
}
