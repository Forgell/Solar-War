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
    class Asteroid
    {
        private Texture2D image;
        private Rectangle rect;
        private Vector2 speeed;
        
        public Asteroid(int GUI_width, int GUI_height, Texture2D i)
        {
            image = i; 
        }
    }
}
