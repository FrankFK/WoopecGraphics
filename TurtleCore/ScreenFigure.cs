using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    /// <summary>
    /// An instance of this class draws a figure on the screen
    /// </summary>
    public class ScreenFigure : ScreenObject
    {
        public Vec2D Position { get; set; }

        public double Heading { get; set; }

        public Color FillColor { get; set; }

        public Color OutlineColor { get; set; }

        public bool IsVisible { get; set; }

        public ScreenFigure(int id)
        {
            ID = id;
        }
    }
}
