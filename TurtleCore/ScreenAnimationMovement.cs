using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    public enum ScreenAnimationMovementProperty
    {
        EndPoint
    }

    public class ScreenAnimationMovement : ScreenAnimation
    {
        public ScreenAnimationMovementProperty AnimatedProperty { get; set; }

        public Vec2D StartValue { get; set; }

        public Vec2D EndValue { get; set; }
    }
}
