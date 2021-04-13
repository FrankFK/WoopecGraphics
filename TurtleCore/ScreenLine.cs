using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    /// <summary>
    /// An instance of this class is a line that can be printed and animated on the screen.
    /// It also can be removed from the screen.
    /// </summary>
    public class ScreenLine : ScreenObject
    {
        public Vec2D StartPoint { get; set; }

        public Vec2D EndPoint { get; set; }


        // TO-DO: Color-Handling
        // public Color Color { get; set; }
    }
}
