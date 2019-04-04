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
        //A list for groups of ships that are not at a planet
        List<Ship> ships;


        public Player()
        {
            planets = new List<Planet>();
            ships = new List<Ship>();
        }

        //how do we create a method(s) that allows a player to select a ship/planet 
        //and move a number of ships to another location (at a planet, group of ships, or spot on the map)?
        //answer: have player click and hold mouse over planet/ group of ship then drag object to only a planet 

        public void action()
        {
            //move
            //select planet or ship to move to planet location

            //if mouse position isButtonDown over a planet position
            //keep the value/planet that the mouse start at
            //check if the location that the mouse is released at is a different planet position
            //moveShips(Planet planet, Planet planet2, int numberOfShips, Rectangle position)

            //if mouse position isButtonDown over a ship position
            //keep the value/ship that the mouse start at
            //check if the location that the mouse is released at is a different planet position
            //moveShips(Ship ship1, Planet planet2)


            //if at opponent/neutral planet, attack planet

        }

        public void moveShips(Planet planet, Planet planet2, int numberOfShips, Rectangle position)//planet1 is moving from, planet2 is moving to
        {
            //ships.Add(new Ship(numberOfShips, position)); //fix later
            //add a Ship to the ship list class at the position that the planet is at
            //move Ship toward planet2
        }

        public void moveShips(Ship ship1, Planet planet2)//Ship is moving from, planet2 is moving to
        {
            //move Ship toward planet2
        }

        public void addPlanet(Planet planet1)
        {
            planets.Add(planet1);
        }

    }
}
