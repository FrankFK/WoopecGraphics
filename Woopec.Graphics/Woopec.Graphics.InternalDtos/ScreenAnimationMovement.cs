using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Graphics.InternalDtos
{
    internal enum ScreenAnimationMovementProperty
    {
        /// <summary>
        /// The value of Point1 of a line is changed during the animation
        /// </summary>
        Point1,

        /// <summary>
        /// The value of Point2 of a line is changed during the animation
        /// </summary>
        Point2,

        /// <summary>
        /// The value of Position of a figure is changed during the animation
        /// </summary>
        Position,

    }

    internal class ScreenAnimationMovement : ScreenAnimationEffect
    {
        public ScreenAnimationMovementProperty AnimatedProperty { get; set; }

        /// <summary>
        /// At the beginning of the animation the animated property has this value
        /// At the end of the animation the animated property has the value, that is specified in the ScreenObject
        /// </summary>
        public Vec2DValue StartValue { get; set; }

    }
}
