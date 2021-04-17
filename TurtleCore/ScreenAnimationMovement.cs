using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    public enum ScreenAnimationMovementProperty
    {
        /// <summary>
        /// The value of Point2 of a line is changed during the animation
        /// </summary>
        Point2
    }

    public class ScreenAnimationMovement : ScreenAnimation
    {
        public ScreenAnimationMovementProperty AnimatedProperty { get; set; }

        /// <summary>
        /// At the beginning of the animation the animated property has this value
        /// At the end of the animation the animated property has the value, that is specified in the ScreenObject
        /// </summary>
        public Vec2D StartValue { get; set; }

    }
}
