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
using System.IO;

namespace Server
{
    class Planet
    {

        //-1 will be replace with some resonable value
        //private Random r;
        private Texture2D[] tex;
        private SpriteFont font;
        public Rectangle position
        {

            get { return pos; }
        }
        private Rectangle pos;

        public double Angle
        {
            get { return angle; }
        }
        private double angle;


        public Vector2 Orgin
        {
            get { return origin; }
        }
        private Vector2 origin;


        public double Angular_Speed
        {
            get { return angular_speed; }
        }
        private double angular_speed;



        public int Scaler
        {
            get { return scaler; }
        }
        private int scaler;

        public Color Color
        {
            get { return faction_color; }
        }

        public int Ships
        {
            get
            {
                return ships;
            }
        }

        private IServiceProvider server;

        //private Label ship_label;


        public static int Max_AMOUNT_OF_SHIPS_ON_PLANET = 99;
        public static int TRAVEL_RADIUS = 270 / 2;
        public static int TOTAL_TIME_TO_CAPTURE = 1200; // as in 60 frames
        public static uint TOTAL_NUMBER_OF_PLANETS = 0;
        private uint id;
        public uint ID
        {
            get { return id; }
        }
        private int index;
        private Vector2 offset;
        //radius for the planet to invade
        //private Texture2D Radius_Tex;
        //private Rectangle Radius_rect, mouse_Rect;
        private Boolean Raddis;

        //There is also a ship class, but this is the number of ships at this planet.
        private int ships;
        //private Vector2[] shipPositions;
        //private int color; //team that the planet is for {0 = orange, 1 = blue, 2 = green, 3 = purple, 4 = neutral}
        //Label label;
        private bool isAttacked;
        private int incrementShipTimer;
        private int timer;
        private String fileName;
        private int radius;

        private Color faction_color;
        private bool selected;
        private Rectangle selected_rect;
        private bool is_being_taken_over;
        private double capture_timer;
        //private Label capture_label;
        private Color ships_color;

        public Planet(string fileName, Vector2 origin, int radius, double angular_speed, int scaler, Color faction_color) // input degress
        {
            this.origin = origin;
            this.radius = radius;
            this.fileName = fileName;
            id = ++TOTAL_NUMBER_OF_PLANETS;
			if (id > 18)
			{
				Console.WriteLine("Error: Construction out of bounds -" + TOTAL_NUMBER_OF_PLANETS);
			}
            int width = 60 / scaler;
            int height = 60 / scaler;
            offset = new Vector2(width / 2.0f, height / 2.0f);
            pos = new Rectangle((int)origin.X + radius - (int)offset.X, (int)origin.Y - (int)offset.Y, width, height);
            
            //how to set the size depending on the planet
            angle = Math.PI / 180.0 * 5.0;
            timer = 0;
            incrementShipTimer = 0;
            double distance = angle * Math.Pow(radius, 2);
            this.angular_speed = MathHelper.ToRadians((float)angular_speed);
            this.index = 0;
            this.scaler = scaler;
            ships = 0;

            this.faction_color = faction_color;
            
            selected = false;
            is_being_taken_over = false;
            capture_timer = 0;
            ships_color = faction_color;
        }

        public void setAngle(double angle) // input degeres
        {
            this.angle = MathHelper.ToRadians((float)angle);
        }

        

        private void Orbit()
        {

            double x = (origin.X + Math.Cos(angle) * radius) - offset.X;
            double y = (origin.Y + Math.Sin(angle) * radius) - offset.Y;
            pos.X = (int)x;
            pos.Y = (int)y;

        }

        public void tranfer_troops(Planet source, int amount)
        {
            source.ships -= amount;
            if (source.ships_color == this.ships_color)
            {
                // peacful tranfer
                this.ships += amount;
            }
            else
            {
                // it is attacking
                if (this.ships >= amount)
                {
                    // the faction holds
                    this.ships -= amount;
                }
                else
                {
                    // stop producing
                    // start a timer
                    Console.WriteLine("source is taking over");
                    is_being_taken_over = true;
                    this.ships = amount - this.ships;

                    ships_color = source.faction_color;

                }

            }
        }

        private void updateShips()
        {
            incrementShipTimer++;
            
            

            if (incrementShipTimer == 60)//increase number of ships each second
            {
                incrementShips();
                incrementShipTimer = 0;
            }
        }

        public void updateCapture(GameTime gametime)
        {
            capture_timer++;
            //capture_label.updatePosition(pos.X, pos.Y + pos.Width);
            //capture_label.updateText("" + Math.Round((capture_timer / (TOTAL_TIME_TO_CAPTURE * 1.0)) * 100) + "%");
            if (capture_timer >= TOTAL_TIME_TO_CAPTURE)
            {
                // then lets tranfer the planets contoll
                capture_timer = 0;
                this.faction_color = ships_color;
                //switch()
                /*if (faction_color == Color.Red)
                {
                    fileName = "planet-1";
                }
                if (faction_color == Color.Blue)
                {
                    fileName = "planet-2";
                }
                if (faction_color == Color.Green)
                {
                    fileName = "planet-3";
                }
                if (faction_color == Color.Purple)
                {
                    fileName = "planet-4";
                }*/

                is_being_taken_over = false;
            }
        }

        public void Update(GameTime gameTime)
        {
            // see if the mouse is hovering to show the radius of travel
            //update_radius(m);

            updateShips();

            if (is_being_taken_over)
            {
                updateCapture(gameTime);
            }

            angle += angular_speed;
            Orbit();
            
            int diff = 7;
            selected_rect.X = pos.X - diff;
            selected_rect.Y = pos.Y - diff;//, width + diff, height + diff);
        }

        private void incrementShips()
        {
            if (faction_color == Color.Black)
            {
                //nothing happens because planet is neutral
            }
            else
            {
                if (ships < Max_AMOUNT_OF_SHIPS_ON_PLANET && !is_being_taken_over)
                {
                    ships += 1;
                }
            }
        }

        public byte[] Encode()
        {
            byte[] map = new byte[5];
            map[0] = (byte)(((id & 31) << 3) | (((uint)pos.X & 1792) >> 8)); // first 5 bits are id and the last 3 are the first 11 of pos.X
			if (id > 18)
			{
				//Console.WriteLine(id);
			}
			map[1] = (byte)((uint)pos.X & 255); // last 8 of pos.X
            map[2] = (byte)(((uint)pos.Y & 2040) >> 3); // fist 8 bits are here for pos.Y
            map[3] = (byte)((((uint)pos.Y & 7) << 5 ) | (((uint)ships & 124) >> 2)); // first 3 bits are the last bits of pos.Y and the rest are the first 5 bits of ship
            map[4] = (byte)(((uint)ships & 3) << 6); // first 2 bits are the last 2 bits of ships  2 bits before is the color of the ships on the planet so 1100
            if (faction_color == Color.Red) // last 2 bits are color
            {
                map[4] = (byte)(map[4] | 0);// last 3bits are color
			}
            if (faction_color == Color.Blue)
            {
                map[4] = (byte)(map[4] | 1);
            }
            if (faction_color == Color.Green)
            {
                map[4] = (byte)(map[4] | 2);
            }
            if (faction_color == Color.Purple)
            {
                map[4] = (byte)(map[4] | 3);
            }
            if (faction_color == Color.Black)
            {
                map[4] = (byte)(map[4] | 4);
            }

			if (ships_color == Color.Red) 
			{
				map[4] = (byte)(map[4] | 0); // 000000
			}
			if (ships_color == Color.Blue)
			{
				map[4] = (byte)(map[4] | 8); // 001000
			}
			if (ships_color == Color.Green)
			{
				map[4] = (byte)(map[4] | 16); // 010000
			}
			if (ships_color == Color.Purple)
			{
				map[4] = (byte)(map[4] | 24); // 011000
			}
			if (ships_color == Color.Black)
			{
				map[4] = (byte)(map[4] | 32); // 100000
			}
			return map;
        }

    }
}
