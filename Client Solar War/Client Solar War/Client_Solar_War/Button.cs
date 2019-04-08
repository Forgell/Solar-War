using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Client_Solar_War
{
    class Button
    {
        private Rectangle rect;
        public Rectangle Rectangle
        {
            get { return rect; }
        }

        public Label Label
        {
            get { return label; }
        }
        private Label label;

        private Rectangle filler_temp;
        public Button(Label label)
        {
            this.label = label;
            Vector2 size = label.Font.MeasureString(label.Text);
            this.rect = new Rectangle((int)label.Position.X, (int)label.Position.Y, (int)size.X, (int)size.Y);
            filler_temp = new Rectangle( 0 , 0 , 1 , 1);
        }

        public bool hovering(Vector2 position)
        {
            filler_temp.X = (int)position.X;
            filler_temp.Y = (int)position.Y;
            return rect.Intersects(filler_temp);
        }

        public bool hovering(int x, int y)
        {
            filler_temp.X = x;
            filler_temp.Y = y;
            return rect.Intersects(filler_temp);
        }

        public bool hovering(MouseState mouse)
        {
            filler_temp.X = mouse.X;
            filler_temp.Y = mouse.Y;
            return rect.Intersects(filler_temp);
        }
    }
}
