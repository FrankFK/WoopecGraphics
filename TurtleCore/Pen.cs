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
        private readonly IScreen _screen;

        private static int s_totalCounter;

        private readonly int _id;

        private bool _firstAnimationIsAdded; // true, if an animation of this pen (or the turtle it belongs to) is added

        private bool _isFilling; // true, if pen is used to draw a shape
        private List<Vec2D> _fillingPolygon;

        /// <summary>
        /// Constructs a Pen that is not used as part of a Turtle class and uses the default screen
        /// </summary>
        public Pen()
            : this(Interlocked.Increment(ref s_totalCounter))
        {
        }

        /// <summary>
        /// Constructs a Pen that is not used as part of a Turtle class
        /// </summary>
        /// <param name="screen">Pen is printed on this screen</param>
        public Pen(IScreen screen)
            : this(screen, Interlocked.Increment(ref s_totalCounter))
        {
        }

        /// <summary>
        /// Constructs a Pen that is used as a part of a Turtle class and uses the default screen
        /// </summary>
        /// <param name="id">The Id of the turtle</param>
        public Pen(int id)
            : this(Screen.GetDefaultScreen(), id)
        {
        }


        /// <summary>
        /// Constructs a Pen
        /// </summary>
        /// <param name="screen">Pen is printed on this screen</param>
        /// <param name="id">The Id of the turtle</param>
        internal Pen(IScreen screen, int id)
        {
            _screen = screen;
            _id = id;
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
        }
        private Vec2D _position;


        public void SetPosition(Vec2D value)
        {
            if (IsDown)
            {
                DrawMove(_position, value);
            }

            if (_isFilling)
                _fillingPolygon.Add(value);

            _position = value;
        }

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

            if (_isFilling)
                _fillingPolygon.Add(newPosition);

            _position = newPosition;
        }

        public bool Filling { get { return _isFilling; } }

        public void BeginFilling()
        {
            if (!_isFilling)
            {
                _isFilling = true;
                _fillingPolygon = new();
                _fillingPolygon.Add(Position);
            }
        }

        /// <summary>
        /// The Shape resulting from the traversed points between BeginFilling and EndFilling is returned.
        /// </summary>
        /// <returns></returns>
        public Shape EndFilling()
        {
            Shape shape;
            if (_isFilling)
            {
                _isFilling = false;
                shape = new Shape(_fillingPolygon);
                _fillingPolygon = null;
            }
            else
            {
                shape = new Shape(new List<Vec2D>());
            }
            return shape;
        }

        private void DrawMove(Vec2D oldPosition, Vec2D newPosition)
        {
            var line = new ScreenLine()
            {
                ID = _screen.CreateLine(),
                Color = Color,
                Point1 = oldPosition,
                Point2 = newPosition,
                GroupID = _id,
            };

            if (_firstAnimationIsAdded)
            {
                // Wait for previous animations of this pen
                line.WaitForAnimationsOfGroupID = _id;
            }
            else
            {
                if (_screen.LastIssuedAnimatonGroupID != ScreenObject.NoGroupId)
                {
                    // If we do not wait for another animation this pen is drawn immediately. In most cases the programmer expects
                    // that all previously created animation are drawn before this pen is drawn.
                    // Therefore:
                    line.WaitForAnimationsOfGroupID = _screen.LastIssuedAnimatonGroupID;
                }
            }

            if (!Speed.NoAnimation)
            {
                int speedDuration = Speed.MillisecondsForMovement(oldPosition, newPosition);

                // Animation dazu:
                line.Animation = new ScreenAnimation();
                line.Animation.Effects.Add(new ScreenAnimationMovement() { AnimatedProperty = ScreenAnimationMovementProperty.Point2, StartValue = oldPosition, Milliseconds = speedDuration });
                _firstAnimationIsAdded = true;
            }

            _screen.DrawLine(line);
        }

        /// <summary>
        /// This method is called if the first turtle object is sent to screen
        /// </summary>
        internal void TurtleObjectSentToScreen()
        {
            _firstAnimationIsAdded = true;
        }
    }
}
