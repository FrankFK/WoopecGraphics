using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    /// <summary>
    /// An instance of this class is a form (for instance the image of a turtle), which 
    /// can be moved on the screen
    /// </summary>
    internal class Form
    {
        private readonly Screen _screen;

        public Vec2D Position { get; set; }

        public Vec2D Orientation { get; private set; }

        public double Heading { get; private set; }

        public Form()
        {
            _screen = Screen.GetDefaultScreen();
            Position = new Vec2D(0, 0);
            Orientation = new Vec2D(1, 0);
            Heading = 0;
        }

        public void Rotate(double angle)
        {
            var newHeading = (Heading + angle) % 360;
            if (newHeading < 0) newHeading += 360;

            var newOrientation = Orientation.Rotate(angle);

            // TODO: Turtle neu anzeigen
            Orientation = newOrientation;
            Heading = newHeading;
        }

        public void Move(double distance)
        {
            var newPosition = Position + distance * Orientation;

            // TODO: Turtle bewegen

            Position = newPosition;

        }

    }
}
