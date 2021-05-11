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

        private bool _firstAnimationIsAdded; // true, if an animation of this pen added

        public Pen()
        {
            _screen = Screen.GetDefaultScreen();
            _id = Interlocked.Increment(ref s_totalCounter);
            _position = new Vec2D(0, 0);
            Orientation = new Vec2D(1, 0);
            Color = Colors.Black;
            Speed = SpeedLevel.Normal;
            Heading = 0;
            IsDown = true;
        }

        public Vec2D Position
        {
            get
            {
                return _position;
            }
            set
            {
                if (IsDown && !value.IsApproximatelyEqualTo(_position, 0.001))
                {
                    DrawMove(_position, value);
                }
                _position = value;
            }
        }
        private Vec2D _position;

        public Color Color { get; set; }

        public Speed Speed { get; set; }

        public Vec2D Orientation { get; private set; }

        public double Heading { get; private set; }

        /// <summary>
        /// True if pen is down, False if it’s up.
        /// </summary>
        public bool IsDown { get; set; }


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

            if (IsDown)
            {
                DrawMove(Position, newPosition);
            }

            _position = newPosition;
        }


        private void DrawMove(Vec2D oldPosition, Vec2D newPosition)
        {
            var line = new ScreenLine()
            {
                ID = _screen.CreateLine(),
                Color = Color,
                Point1 = oldPosition,
                Point2 = newPosition
            };

            if (!Speed.NoAnimation)
            {
                int speedDuration = Speed.MillisecondsForMovement(oldPosition, newPosition);

                // Animation dazu:
                line.Animation = new ScreenAnimation(_id);
                if (_firstAnimationIsAdded)
                {
                    // Wait for previous animations of this pen
                    line.Animation.WaitForAnimationsOfGroupID = _id;
                }
                else
                {
                    if (_screen.LastIssuedAnimatonGroupID != ScreenAnimation.NoGroupId)
                    {
                        // If we do not wait for another animation this pen is drawn immediately. In most cases the programmer expects
                        // that all previously created animation are drawn before this pen is drawn.
                        // Therefor:
                        line.Animation.WaitForAnimationsOfGroupID = _screen.LastIssuedAnimatonGroupID;
                    }
                }
                line.Animation.Effects.Add(new ScreenAnimationMovement() { AnimatedProperty = ScreenAnimationMovementProperty.Point2, StartValue = oldPosition, Milliseconds = speedDuration });
                _firstAnimationIsAdded = true;
            }

            _screen.DrawLine(line);
        }

    }
}
