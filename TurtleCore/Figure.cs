using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TurtleCore
{
    /// <summary>
    /// An instance of this class is a form (for instance the image of a turtle), which 
    /// can be moved on the screen
    /// </summary>
    internal class Figure
    {
        private static int s_totalCounter;
        private readonly int _id;

        private readonly IScreen _screen;
        private bool _firstAnimationIsAdded;
        private string _shapeName;
        private int _idOnScreen;
        private bool _isVisible;

        /// <summary>
        /// Constructs a Figure that is not used as part of a Turtle class and uses the default screen
        /// </summary>
        public Figure()
            : this(Interlocked.Increment(ref s_totalCounter))
        {
        }

        /// <summary>
        /// Constructs a Figure that is not used as part of a Turtle class
        /// </summary>
        /// <param name="screen">Figure is printed on this screen</param>
        public Figure(IScreen screen)
            : this(screen, Interlocked.Increment(ref s_totalCounter))
        {
        }

        /// <summary>
        /// Constructs a Figure that is used as a part of a Turtle class and uses the default screen
        /// </summary>
        /// <param name="id">The Id of the turtle</param>
        public Figure(int id)
            : this(Screen.GetDefaultScreen(), id)
        {
        }

        /// <summary>
        /// Constructs a Figure
        /// </summary>
        /// <param name="screen">Figure is printed on this screen</param>
        /// <param name="id">The Id of the turtle</param>
        public Figure(IScreen screen, int id)
        {
            _id = id;
            _screen = screen;
            _position = new Vec2D(0, 0);
            Orientation = new Vec2D(1, 0);
            Heading = 0;
            _isVisible = false;
            Speed = SpeedLevel.Normal;
            FillColor = Colors.Black;
            OutlineColor = Colors.Black;
            _shapeName = ShapeNames.Classic;

            _idOnScreen = _screen.CreateFigure(_shapeName);
        }

        public Vec2D Position
        {
            get
            {
                return _position;
            }
        }
        private Vec2D _position;


        public Vec2D Orientation { get; private set; }

        public double Heading { get; private set; }

        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                bool updateScreen = (_isVisible != value);
                _isVisible = value;
                if (updateScreen)
                    UpdateScreen();
            }
        }

        public Color FillColor { get; set; }

        public Color OutlineColor { get; set; }

        public Speed Speed { get; set; }


        public void Rotate(double angle)
        {
            var oldHeading = Heading;
            Heading = (Heading + angle);

            var newOrientation = Orientation.Rotate(angle);

            Orientation = newOrientation;

            if (IsVisible)
                RotateOnScreen(oldHeading);

            // Reset heading to range 0..360
            Heading = Heading % 360;
            if (Heading < 0) Heading += 360;
        }

        public void Move(double distance)
        {
            Move(distance, false);
        }

        internal void Move(double distance, bool togetherWithPreviousAnimation)
        {
            var oldPosition = _position;
            _position = _position + distance * Orientation;

            if (IsVisible)
                MoveOnScreen(oldPosition, togetherWithPreviousAnimation);
        }

        public void SetPosition(Vec2D value)
        {
            SetPosition(value, false);
        }

        internal void SetPosition(Vec2D value, bool togetherWithPreviousAnimation)
        {
            var oldPosition = _position;
            _position = value;
            if (IsVisible)
                MoveOnScreen(oldPosition, togetherWithPreviousAnimation);
        }

        private void UpdateScreen()
        {
            var figure = CreateScreenFigure(false);

            if (!Speed.NoAnimation)
            {
                // To do: animation dazu
                _firstAnimationIsAdded = true;
            }

            _screen.UpdateFigure(figure);

        }

        private void MoveOnScreen(Vec2D oldPosition, bool togetherWithPreviousAnimation)
        {
            var figure = CreateScreenFigure(togetherWithPreviousAnimation);

            if (!Speed.NoAnimation)
            {
                int speedDuration = Speed.MillisecondsForMovement(oldPosition, Position);

                // Animation dazu:
                figure.Animation = new ScreenAnimation();
                figure.Animation.Effects.Add(new ScreenAnimationMovement() { AnimatedProperty = ScreenAnimationMovementProperty.Position, StartValue = oldPosition, Milliseconds = speedDuration });
                _firstAnimationIsAdded = true;
            }

            _screen.UpdateFigure(figure);
        }

        private void RotateOnScreen(double oldHeading)
        {
            var figure = CreateScreenFigure(togetherWithPreviousAnimation: false);

            if (!Speed.NoAnimation)
            {
                int speedDuration = Speed.MillisecondsForRotation(oldHeading, Heading);

                // Animation dazu:
                figure.Animation = new ScreenAnimation();
                figure.Animation.Effects.Add(new ScreenAnimationRotation() { AnimatedProperty = ScreenAnimationRotationProperty.Heading, StartValue = oldHeading, Milliseconds = speedDuration });
                _firstAnimationIsAdded = true;
            }

            _screen.UpdateFigure(figure);
        }

        /// <summary>
        /// Create a ScreenFigure object according to the actual values of this.
        /// </summary>
        /// <returns></returns>
        private ScreenFigure CreateScreenFigure(bool togetherWithPreviousAnimation)
        {
            var figure = new ScreenFigure(_idOnScreen)
            {
                IsVisible = IsVisible,
                Position = Position,
                FillColor = FillColor,
                OutlineColor = OutlineColor,
                Heading = Heading,
                GroupID = _id,
            };
            if (!togetherWithPreviousAnimation)
            {
                if (_firstAnimationIsAdded)
                {
                    // Wait for previous animations of this pen
                    figure.WaitForAnimationsOfGroupID = _id;
                }
                else
                {
                    if (!Speed.NoAnimation && _screen.LastIssuedAnimatonGroupID != ScreenObject.NoGroupId)
                    {
                        // If we do not wait for another animation this turtle is drawn immediately. In most cases the programmer expects
                        // that all previously created animation are drawn before this pen is drawn.
                        // Therefore:
                        figure.WaitForAnimationsOfGroupID = _screen.LastIssuedAnimatonGroupID;
                    }
                }
            }
            return figure;
        }
    }
}
