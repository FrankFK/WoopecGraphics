using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Graphics.Interface.Dtos
{
    /// <summary>
    /// An instance of this class describes a figure (position, heading, colors, visibility, shape) on the screen.
    /// If the id of the figure was already used, the figure already exists on the screen. In this case the 
    /// screen figure is updated according to the actual values of the class-instance.
    /// </summary>
    internal class ScreenFigure : ScreenObject
    {
        public DtoVec2D Position { get; set; }

        public double Heading { get; set; }

        public DtoColor FillColor { get; set; }

        public DtoColor OutlineColor { get; set; }

        public bool IsVisible { get; set; }

        /// <summary>
        /// If this value is null, the current shape of the figure is retained.
        /// </summary>
        public DtoShapeBase Shape { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">If this is the id of an already existing ScreenFigure, this figure is upated according to the values of this. 
        /// Otherwise the figure is created with the given values.</param>
        public ScreenFigure(int id)
        {
            ID = id;
        }
    }
}
