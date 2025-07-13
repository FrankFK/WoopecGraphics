using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Woopec.Graphics.InternalDtos;
using Woopec.Graphics.Helpers;

namespace Woopec.Graphics
{
    /// <summary>
    /// An instance of this class is a figure (for instance the image of a turtle), which 
    /// can be moved on the screen
    /// </summary>
    public class Figure
    {
        // If this is part of a turtle: The turtle, the pen of the turtle and the figure of the turtle have the same _id.
        // If this is a standalone object: It has a unique ID that is different to the IDs of all other pens, figures und turtles.
        private readonly int _id;

        private readonly ILowLevelScreen _lowLevelScreen;
        private readonly Screen _screen;
        private bool _firstAnimationIsAdded;
        private WaitingForCompletedAnimationInfo _waitingForCompletedAnimationInfo;
        private ShapeBase _shape;
        private bool _shapeIsChanged;
        private int _idOnScreen;
        private bool _figureIsCreated;
        private bool _isVisible;
        private Color _fillColor;
        private Color _outlineColor;
        private double _heading;

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
        /// <remarks>At the moment internal, because multi-screen support is not tested.</remarks>
        internal Figure(ILowLevelScreen lowLevelScreen)
            : this(lowLevelScreen, IdFactory.CreateNewId())
        {
        }

        /// <summary>
        /// Constructs a Figure that is used as a part of a Turtle class and uses the default screen
        /// </summary>
        /// <param name="id">The Id of the turtle</param>
        internal Figure(int id)
            : this(LowLevelDefaultScreen.Get(), id)
        {
        }

        /// <summary>
        /// Constructs a Figure that is used as a part of a Turtle class and uses the a given screen
        /// </summary>
        /// <param name="lowLevelScreen">Figure is printed on this screen</param>
        /// <param name="id">The Id of the turtle</param>
        internal Figure(ILowLevelScreen lowLevelScreen, int id)
        {
            _id = id;
            _lowLevelScreen = lowLevelScreen;
            _screen = new Screen(_lowLevelScreen);
            _position = new Vec2D(0, 0);
            Orientation = new Vec2D(1, 0);
            _heading = 0;
            _isVisible = false;
            Speed = Speeds.Normal;
            _fillColor = Colors.Black;
            _outlineColor = Colors.Black;
            _shape = Shapes.Classic;
            _shapeIsChanged = true;
            _figureIsCreated = false; // the figure is only created when it is necessary
        }

        /// <summary>
        /// The figures's current position (as a Vec2D vector). Does not change the figure's heading.
        /// </summary>
        /// <value>The new position of the figure.<br/>
        /// Can be specified as tuple: (50, 30) or as vector: new Vec2D(50, 30)
        /// </value>
        public Vec2D Position
        {
            get
            {
                return _position;
            }
            set
            {
                SetPosition(value);
            }
        }
        private Vec2D _position;


        private Vec2D Orientation { get; set; }

        /// <summary>
        /// Orientation of the figure. The heading is measured in degrees. Some common directions:<br></br>
        ///    0 - east <br></br>
        ///   90 - north<br></br>
        ///  180 - west <br></br>
        ///  270 - south<br></br>
        /// </summary>
        public double Heading
        {
            get { return _heading; }
            set
            {
                double rotation = value - _heading;
                Rotate(rotation);
            }
        }

        /// <summary>
        /// True if figure is shown, false if it is hidden.
        /// </summary>
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

        /// <summary>
        /// Set fill color and outline color
        /// Three ways to set the color are allowed.<br></br>
        /// Set a predefined Color:
        /// <example>
        /// <code>
        ///     figure.Color = Colors.Green;
        /// </code>
        /// </example>
        /// <br></br>
        /// Set a predefined Color by its name:
        /// <example>
        /// <code>
        ///     figure.Color = "green";
        /// </code>
        /// </example>
        /// <br></br>
        /// Set RGB color represented by three values for red, green and blue. Each of these values must be in the range 0..255:
        /// <example>
        /// <code>
        ///     figure.Color = new Color(255, 165, 0); // orange
        /// </code>
        /// </example>
        /// </summary>
        /// <remarks>
        /// This property has no getter. Use OutlineColor and FillColor.
        /// </remarks>
        public Color Color { set { FillColor = value; OutlineColor = value; } }

        /// <summary>
        /// Fill color
        /// </summary>
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

        /// <summary>
        /// Outline color
        /// </summary>
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

        /// <summary>
        /// The figures shape.<br></br>
        /// Two types of shapes are possible:<br></br>
        /// <br></br>
        /// Set a predefined shape:
        /// <example>
        /// <code>
        ///     figure.Shape = Shapes.Turtle;   <br/>
        ///     figure.Shape = Shapes.Arrow;<br/>
        ///     figure.Shape = Shapes.Circle;<br/>
        ///     figure.Shape = Shapes.Classic;<br/>
        ///     figure.Shape = Shapes.Triangle;<br/>
        ///     figure.Shape = Shapes.Bird;<br/>
        /// </code>
        /// </example>
        /// <br></br>
        /// Set a user defined shape:
        /// <example>
        /// <code>
        ///     var shape = new Shape(new() { (0, 0), (-5, -9), (0, -7), (5, -9) }); // same shape as Shapes.Classic <br></br>
        ///     figure.Shape = shape;   <br/>
        /// </code>
        /// </example>
        /// <br></br>
        /// Register a shape and use it (later)
        /// <example>
        /// <code>
        ///     Shapes.Add("square", new() { (-3, -3), (6, -3), (6, 6), (-3, 6) });  <br></br>
        ///     // ...
        ///     figure.Shape = Shapes.Get("square");   <br/>
        /// </code>
        /// </example>
        /// </summary>
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


        /// <summary>
        /// Speed of the figure.
        /// Speed from Speeds.Slowest to Speeds.Fast enforce increasingly faster animation of
        /// line drawing and figure turning.<br></br>
        /// Attention: With Speeds.Fastest *no* animation takes place.<br></br>
        /// <br></br>
        /// <para>
        /// Examples for setting the speed:
        /// <example>
        /// <code>
        /// figure.Speed = Speeds.Slowest; // figure moves very slow <br/>
        /// figure.Speed = Speeds.Fast;    // figure moves very fast<br/>
        /// figure.Speed = Speeds.Fastest; // No animation takes place<br/>
        /// </code>
        /// </example>
        /// </para>
        /// <br></br>
        /// <para>
        /// Examples for getting and checking the speed:
        /// <example>
        /// <code>
        /// var currentSpeed = figure.Speed; <br/>
        /// if (currentSpeed == Speeds.Slow) ...<br/>
        /// if (currentSpeed.NoAnimation) ...<br></br>
        /// </code>
        /// </example>
        /// </para>
        /// </summary>
        public Speed Speed { get; set; }


        /// <summary>
        /// Rotate the figure by <paramref name="angle"/> units. 
        /// <example>
        /// <code>
        /// var figure = new Figure(); <br/>
        /// // figure.Heading is 0 and figure looks to the east <br/>
        /// figure.Rotate(90); <br/>
        /// // figure.Heading is 90 and figure looks north <br/>
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="angle">Rotation-value specified in degrees (value of 360 is a full rotation). Positive values turn left, negative values turn right.</param>
        public void Rotate(double angle)
        {
            var oldHeading = _heading;
            _heading = (_heading + angle);

            var newOrientation = Orientation.Rotate(angle);

            Orientation = newOrientation;

            if (IsVisible)
                RotateOnScreen(oldHeading);

            // Reset heading to range 0..360
            _heading = _heading % 360;
            if (_heading < 0) _heading += 360;
        }

        /// <summary>
        /// Move the figure by the specified distance, in the direction the figure is headed. 
        /// <example>
        /// <code>
        /// var figure = new Figure(); <br/>
        /// // figure position is (0, 0) <br/>
        /// figure.Move(100); <br/>
        /// // figure position is (100, 0) <br/>
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="distance">Distance to move. Positive values move forward, negative values move backward.</param>
        public void Move(double distance)
        {
            Move(distance, false);
        }

        /// <summary>
        /// Only needed internally for a figure that is part of a turtle and does not have to wait for the animation of the turtle's pen.
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="togetherWithPreviousAnimation"></param>
        internal void Move(double distance, bool togetherWithPreviousAnimation)
        {
            var oldPosition = _position;
            _position = _position + distance * Orientation;

            if (IsVisible)
                MoveOnScreen(oldPosition, togetherWithPreviousAnimation);
        }

        /// <summary>
        /// Move the figure to the given position. Do not change the figure's heading.
        /// <example>
        /// <code>
        /// var figure = new Figure(); <br/>
        /// // figure position is (0, 0) <br/>
        /// figure.SetPosition((100, 100)); <br/>
        /// // figure position is (100, 100) <br/>
        /// figure.SetPosition(new Vec2D(150, 120)); <br/>
        /// // figure position is (150, 120) <br/>
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="value">The new position of the figure.<br/>
        /// Can be specified as tuple: (50, 30) or as vector: new Vec2D(50, 30)
        /// </param>
        public void SetPosition(Vec2D value)
        {
            SetPosition(value, false);
        }

        /// <summary>
        /// Move the figure to the given position.
        /// </summary>
        /// <param name="x">The new x coordinate of the pen.</param>
        /// <param name="y">The new y coordinate of the pen.</param>
        public void SetPosition(double x, double y) => SetPosition((x, y));


        /// <summary>
        /// Only needed internally for a figure that is part of a turtle and does not have to wait for the animation of the turtle's pen.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="togetherWithPreviousAnimation"></param>
        internal void SetPosition(Vec2D value, bool togetherWithPreviousAnimation)
        {
            var oldPosition = _position;
            _position = value;
            if (IsVisible)
                MoveOnScreen(oldPosition, togetherWithPreviousAnimation);
        }

        /// <summary>
        /// Return the screen on which this figure is drawn
        /// </summary>
        public Screen Screen { get { return _screen; } }

        /// <summary>
        /// Calling WaitForCompletedMovementOf(otherFigure)` ensures that the subsequent movement of this figure is not executed until the previous movement of the otherFigure has finished.
        /// <example>
        /// <code>
        /// var figure1 = new Figure() { IsVisible = true} ; <br/>
        /// var figure2 = new Figure() { IsVisible = true} ; <br/>
        /// figure1.Rotate(90); <br/>
        /// figure1.Move(100); <br/>
        /// figure2.Rotate(-90); <br/>
        /// figure2.WaitForCompletedMovementOf(figure1); <br/>
        /// figure2.Move(100); <br/>
        /// </code>
        /// </example>
        /// </summary>
        /// <remarks>
        /// If you call WaitForCompletedMovementOf(f) with different figures `f` only the last one counts.
        /// </remarks>
        /// <param name="otherFigure">Figure to wait for</param>
        public void WaitForCompletedMovementOf(Figure otherFigure)
        {
            var waitingInfo = new WaitingForCompletedAnimationInfo() { WaitForCompletedAnimationOf = otherFigure._id, WaitingFigure = this, WaitingPen = null };
            WaitForCompletedMovementOf(waitingInfo);
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
                figure.Animation.Effects.Add(new ScreenAnimationMovement() { AnimatedProperty = ScreenAnimationMovementProperty.Position, StartValue = DtoMapper.Map(oldPosition), Milliseconds = speedDuration });
                _firstAnimationIsAdded = true;
            }

            _lowLevelScreen.UpdateFigure(figure);
        }

        private void RotateOnScreen(double oldHeading)
        {
            var figure = CreateScreenFigureUpdate(togetherWithPreviousAnimation: false);

            if (!Speed.NoAnimation)
            {
                int speedDuration = Speed.MillisecondsForRotation(oldHeading, _heading);

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
                Position = DtoMapper.Map(Position),
                FillColor = DtoMapper.Map(FillColor),
                OutlineColor = DtoMapper.Map(OutlineColor),
                Heading = _heading,
                GroupID = _id,
            };
            if (_shapeIsChanged)
            {
                // To reduce overhead we only set the shape if it was changed:
                figure.Shape = DtoMapper.Map(Shape);
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
