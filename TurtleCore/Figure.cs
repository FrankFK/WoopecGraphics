using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Woopec.Core.Internal;

namespace Woopec.Core
{
    /// <summary>
    /// An instance of this class is a form (for instance the image of a turtle), which 
    /// can be moved on the screen
    /// </summary>
    internal class Figure
    {
        // If this is part of a turtle: The turtle, the pen of the turtle and the figure of the turtle have the same _id.
        // If this is a standalone object: It has a unique ID that is different to the IDs of all other pens, figures und turtles.
        private readonly int _id;

        private readonly ILowLevelScreen _lowLevelScreen;
        private bool _firstAnimationIsAdded;
        private WaitingForCompletedAnimationInfo _waitingForCompletedAnimationInfo;
        private ShapeBase _shape;
        private bool _shapeIsChanged;
        private int _idOnScreen;
        private bool _figureIsCreated;
        private bool _isVisible;
        private Color _fillColor;
        private Color _outlineColor;

        /// <summary>
        /// Constructs a Figure that uses the default screen
        /// </summary>
        public Figure()
            : this(IdFactory.CreateNewId())
        {
        }

        /// <summary>
        /// Constructs a Figure that is printed on the given screen
        /// </summary>
        /// <param name="lowLevelScreen">Figure is printed on this screen</param>
        public Figure(ILowLevelScreen lowLevelScreen)
            : this(lowLevelScreen, IdFactory.CreateNewId())
        {
        }

        /// <summary>
        /// Constructs a Figure that is used as a part of a Turtle class and uses the default screen
        /// </summary>
        /// <param name="id">The Id of the turtle</param>
        public Figure(int id)
            : this(LowLevelScreen.GetDefaultScreen(), id)
        {
        }

        /// <summary>
        /// Constructs a Figure
        /// </summary>
        /// <param name="lowLevelScreen">Figure is printed on this screen</param>
        /// <param name="id">The Id of the turtle</param>
        public Figure(ILowLevelScreen lowLevelScreen, int id)
        {
            _id = id;
            _lowLevelScreen = lowLevelScreen;
            _position = new Vec2D(0, 0);
            Orientation = new Vec2D(1, 0);
            Heading = 0;
            _isVisible = false;
            Speed = Speeds.Normal;
            _fillColor = Colors.Black;
            _outlineColor = Colors.Black;
            _shape = Shapes.Classic;
            _shapeIsChanged = true;
            _figureIsCreated = false; // the figure is only created when it is necessary
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

        public Color FillColor
        {
            get
            {
                return _fillColor;
            }
            set
            {
                _fillColor = value;
                if (_isVisible)
                    UpdateScreen();
            }
        }

        public Color OutlineColor
        {
            get
            {
                return _outlineColor;
            }
            set
            {
                _outlineColor = value;
                if (_isVisible)
                    UpdateScreen();
            }
        }

        public ShapeBase Shape
        {
            get
            {
                return _shape;
            }
            set
            {
                _shape = value;
                _shapeIsChanged = true;
                if (_isVisible)
                    UpdateScreen();
            }
        }


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

        public void WaitForCompletedMovementOf(Figure figure)
        {
            throw new NotImplementedException("Not tested yet");
#pragma warning disable CS0162 // Unreachable code detected
            var waitingInfo = new WaitingForCompletedAnimationInfo() { WaitForCompletedAnimationOf = figure._id, WaitingFigure = this, WaitingPen = null };
            WaitForCompletedMovementOf(waitingInfo);
#pragma warning restore CS0162 // Unreachable code detected
        }

        internal void WaitForCompletedMovementOf(WaitingForCompletedAnimationInfo waitingInfo)
        {
            _waitingForCompletedAnimationInfo = waitingInfo;
        }

        internal void ResetWaitingInfo()
        {
            _waitingForCompletedAnimationInfo = null;
        }

        private void UpdateScreen()
        {
            var figure = CreateScreenFigureUpdate(false);

            if (!Speed.NoAnimation)
            {
                _firstAnimationIsAdded = true;
            }

            _lowLevelScreen.UpdateFigure(figure);

        }

        private void MoveOnScreen(Vec2D oldPosition, bool togetherWithPreviousAnimation)
        {
            var figure = CreateScreenFigureUpdate(togetherWithPreviousAnimation);

            if (!Speed.NoAnimation)
            {
                int speedDuration = Speed.MillisecondsForMovement(oldPosition, Position);

                // Animation dazu:
                figure.Animation = new ScreenAnimation();
                figure.Animation.Effects.Add(new ScreenAnimationMovement() { AnimatedProperty = ScreenAnimationMovementProperty.Position, StartValue = oldPosition, Milliseconds = speedDuration });
                _firstAnimationIsAdded = true;
            }

            _lowLevelScreen.UpdateFigure(figure);
        }

        private void RotateOnScreen(double oldHeading)
        {
            var figure = CreateScreenFigureUpdate(togetherWithPreviousAnimation: false);

            if (!Speed.NoAnimation)
            {
                int speedDuration = Speed.MillisecondsForRotation(oldHeading, Heading);

                // Animation dazu:
                figure.Animation = new ScreenAnimation();
                figure.Animation.Effects.Add(new ScreenAnimationRotation() { AnimatedProperty = ScreenAnimationRotationProperty.Heading, StartValue = oldHeading, Milliseconds = speedDuration });
                _firstAnimationIsAdded = true;
            }

            _lowLevelScreen.UpdateFigure(figure);
        }

        /// <summary>
        /// Create a ScreenFigure object according to the actual values of this.
        /// </summary>
        /// <returns></returns>
        private ScreenFigure CreateScreenFigureUpdate(bool togetherWithPreviousAnimation)
        {
            if (!_figureIsCreated)
            {
                _idOnScreen = _lowLevelScreen.CreateFigure();
                _figureIsCreated = true;
            }

            var figure = new ScreenFigure(_idOnScreen)
            {
                IsVisible = IsVisible,
                Position = Position,
                FillColor = FillColor,
                OutlineColor = OutlineColor,
                Heading = Heading,
                GroupID = _id,
            };
            if (_shapeIsChanged)
            {
                // To reduce overhead we only set the shape if it was changed:
                figure.Shape = Shape;
                _shapeIsChanged = false;
            }
            if (!togetherWithPreviousAnimation)
            {
                if (_firstAnimationIsAdded)
                {
                    // Wait for previous animations of this figure (or if the figure belongs to a turtle: wait for previous animations of this figure or the pen)
                    figure.WaitForCompletedAnimationsOfSameGroup = true;
                }
                else
                {
                    // _firstAnimationIsAdded == false
                    // This is the first movement of this!
                    if (_lowLevelScreen.LastIssuedAnimatonGroupID != ScreenObject.NoGroupId)
                    {
                        // A little bit of Artificial Intelligence:
                        // If we do not wait for another animation, this object is drawn immediately. In most cases the programmer expects
                        // that all previously created animations are drawn before this pen is drawn.
                        // Therefore:
                        figure.WaitForCompletedAnimationsOfAnotherGroup = _lowLevelScreen.LastIssuedAnimatonGroupID;
                    }
                }
                if (_waitingForCompletedAnimationInfo != null)
                {
                    // The programmer explicitly wanted to wait for another object
                    figure.WaitForCompletedAnimationsOfAnotherGroup = _waitingForCompletedAnimationInfo.WaitForCompletedAnimationOf;

                    // If pen is waiting. It needs not longer to wait
                    if (_waitingForCompletedAnimationInfo.WaitingPen != null)
                        _waitingForCompletedAnimationInfo.WaitingPen.ResetWaitingInfo();

                    // All subsequent drawings of this should not wait for the other object
                    _waitingForCompletedAnimationInfo = null;
                }
            }
            return figure;
        }

    }
}
