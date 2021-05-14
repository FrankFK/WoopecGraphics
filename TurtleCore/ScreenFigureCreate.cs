using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    /// <summary>
    /// An instance of this class creates a figure (e.g. a polygon) that can be printed and animated on the screen.
    /// </summary>
    public class ScreenFigureCreate : ScreenObject
    {
        public ShapeBase Shape { get; init; }

        public ScreenFigureCreate(int id, ShapeBase shape)
        {
            ID = id;
            Shape = shape;
        }
    }
}
