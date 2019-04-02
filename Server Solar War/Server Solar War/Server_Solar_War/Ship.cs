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

namespace Server_Solar_War
{
    class Ship
    {
        //This class is for each group of ships not at a planet.
        int ships;
        private Texture2D tex;
        private Rectangle pos;

        public Ship(int numberOfShips, Rectangle position)
        {
            ships = numberOfShips;
            pos = position;
        }
    }
}
