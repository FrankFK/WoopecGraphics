using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{

    /// <summary>
    /// An instance of this class is a pen, which can draw lines on the screen
    /// </summary>
    internal class Pen
    {
        private readonly Screen _screen;

        private readonly int _id;

        public Pen()
        {
            _screen = Screen.GetDefaultScreen();
            // To-do: ID eindeutig setzen
            _id = 0;
            Position = new Vec2D(0, 0);
            Orientation = new Vec2D(1, 0);
            Heading = 0;
        }

        public Vec2D Position { get; set; }

        public Vec2D Orientation { get; private set; }

        public double Heading { get; private set; }

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
            var line = new ScreenLine()
            {
                ID = _screen.CreateLine(),
                Point1 = Position,
                Point2 = newPosition
            };

            // Animation dazu:
            line.Animation = new ScreenAnimation() { GroupID = _id, StartWhenPredecessorHasFinished = true };
            line.Animation.Effects.Add(new ScreenAnimationMovement() { AnimatedProperty = ScreenAnimationMovementProperty.Point2, StartValue = Position, Milliseconds = 2000 });

            _screen.DrawLine(line);

            Position = newPosition;

        }


    }
}
