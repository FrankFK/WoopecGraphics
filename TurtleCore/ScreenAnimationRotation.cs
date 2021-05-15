using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{

    public enum ScreenAnimationRotationProperty
    {
        /// <summary>
        /// The value of Heading of a line is changed during the animation
        /// </summary>
        Heading,
    }

    public class ScreenAnimationRotation : ScreenAnimationEffect
    {
        public ScreenAnimationRotationProperty AnimatedProperty { get; set; }

        /// <summary>
        /// At the beginning of the animation the animated property has this value
        /// At the end of the animation the animated property has the value, that is specified in the ScreenObject
        /// </summary>
        public double StartValue { get; set; }

    }
}
