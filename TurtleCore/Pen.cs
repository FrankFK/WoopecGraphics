using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TurtleCore
{

    /// <summary>
    /// An instance of this class is a pen, which can draw lines on the screen
    /// </summary>
    internal class Pen
    {
        private readonly Screen _screen;

        private static int s_totalCounter;

        private readonly int _id;

        public Pen()
        {
            _screen = Screen.GetDefaultScreen();
            _id = Interlocked.Increment(ref s_totalCounter);
            Position = new Vec2D(0, 0);
            Orientation = new Vec2D(1, 0);
            Color = Colors.Black;
            Speed = SpeedLevel.Normal;
            Heading = 0;
        }

        public Vec2D Position { get; set; }

        public Color Color { get; set; }

        public Speed Speed { get; set; }

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

            var line = new ScreenLine()
            {
                ID = _screen.CreateLine(),
                Color = Color,
                Point1 = Position,
                Point2 = newPosition
            };

            if (!Speed.NoAnimation)
            {
                int speedDuration = Speed.MillisecondsForMovement(Position, newPosition);

                // Animation dazu:
                line.Animation = new ScreenAnimation() { GroupID = _id, StartWhenPredecessorHasFinished = true };
                line.Animation.Effects.Add(new ScreenAnimationMovement() { AnimatedProperty = ScreenAnimationMovementProperty.Point2, StartValue = Position, Milliseconds = speedDuration });

            }

            _screen.DrawLine(line);

            Position = newPosition;

        }

    }
}
