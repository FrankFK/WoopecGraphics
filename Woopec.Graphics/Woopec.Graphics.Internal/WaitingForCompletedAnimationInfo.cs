using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Graphics
{
    /// <summary>
    /// Handling data, when Turtle.WaitForCompletedMovementOf is used
    /// </summary>
    internal class WaitingForCompletedAnimationInfo
    {
        public int WaitForCompletedAnimationOf { get; set; }

        public Figure WaitingFigure { get; set; }

        public Pen WaitingPen { get; set; }


    }
}
