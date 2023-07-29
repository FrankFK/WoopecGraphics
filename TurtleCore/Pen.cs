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
    /// An instance of this class is a pen, which can draw lines on the screen
    /// </summary>
    public class Pen
    {
        private readonly ILowLevelScreen _lowLevelScreen;

        // If this is part of a turtle: The turtle, the pen of the turtle and the figure of the turtle have the same _id.
        // If this is a standalone object: It has a unique ID that is different to the IDs of all other pens, figures und turtles.
        private readonly int _id;

        private bool _firstAnimationIsAdded; // true, if an animation of this pen (or the turtle it belongs to) is added
        private WaitingForCompletedAnimationInfo _waitingForCompletedAnimationInfo;

        private bool _isShapeDrawing; // true, if pen is used to draw a shape
        private List<Vec2D> _shapeDrawingPolygon;

        private bool _isFilling; // true, if pen is used to draw a polygon

        private bool _isCreatingPoly; // true, if the pen is used to create a polygon

        /// <summary>
        /// Create a pen that you can start learning to code with.
        /// </summary>
        /// <returns>A pen to start with</returns>
        public static Pen CreateExample()
        {
            var pen = new Pen() { Speed = Speeds.Slowest, Color = Colors.DarkGreen };
            return pen;
        }

        /// <summary>
        /// Constructs a Pen that uses the default screen
        /// </summary>
        public Pen()
            : this(IdFactory.CreateNewId())
        {
        }

        /// <summary>
        /// Constructs a Pen that is not used as part of a Turtle class
        /// </summary>
        /// <param name="screen">Pen is printed on this screen</param>
        internal Pen(ILowLevelScreen screen)
            : this(screen, IdFactory.CreateNewId())
        {
        }

        /// <summary>
        /// Constructs a Pen that is used as a part of a Turtle class and uses the default screen
        /// </summary>
        /// <param name="id">The Id of the turtle</param>
        public Pen(int id)
            : this(LowLevelDefaultScreen.Get(), id)
        {
        }


        /// <summary>
        /// Constructs a Pen that is used as a part of a Turtle class 
        /// </summary>
        /// <param name="screen">Pen is printed on this screen</param>
        /// <param name="id">The Id of the turtle</param>
        internal Pen(ILowLevelScreen screen, int id)
        {
            _lowLevelScreen = screen;
            _id = id;
            _position = new Vec2D(0, 0);
            Orientation = new Vec2D(1, 0);
            Color = Colors.Black;
            Speed = Speeds.Normal;
            Heading = 0;
            IsDown = false;
        }

        /// <summary>
        /// The pen's current position (as a Vec2D vector). 
        /// </summary>
        /// <value>The new position of the pen.<br/>
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


        /// <summary>
        /// Move the pen to the given position.
        /// </summary>
        /// <param name="value">The new position of the pen.<br/>
        /// Can be specified as tuple: (50, 30) or as vector: new Vec2D(50, 30)
        /// </param>
        public void SetPosition(Vec2D value)
        {
            if (IsDown)
            {
                DrawMove(_position, value);
            }

            if (_isShapeDrawing)
                _shapeDrawingPolygon.Add(value);

            _position = value;
        }

        /// <summary>
        /// Move the pen to the given position.
        /// </summary>
        /// <param name="x">The new x coordinate of the pen.</param>
        /// <param name="y">The new y coordinate of the pen.</param>
        public void SetPosition(double x, double y) => SetPosition((x, y));

        /// <summary>
        /// Set pen color
        /// Three ways to set the color are allowed.<br></br>
        /// Set a predefined Color:
        /// <example>
        /// <code>
        ///     pen.Color = Colors.Green;
        /// </code>
        /// </example>
        /// <br></br>
        /// Set a predefined Color by its name:
        /// <example>
        /// <code>
        ///     pen.Color = "green";
        /// </code>
        /// </example>
        /// <br></br>
        /// Set RGB color represented by three values for red, green and blue. Each of these values must be in the range 0..255:
        /// <example>
        /// <code>
        ///     pen.Color = new Color(255, 165, 0); // orange
        /// </code>
        /// </example>
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Speed of the pen.
        /// Speed from Speeds.Slowest to Speeds.Fast enforce increasingly faster animation of
        /// line drawing and pen turning.<br></br>
        /// Attention: With Speeds.Fastest *no* animation takes place.<br></br>
        /// </summary>
        public Speed Speed { get; set; }

        private Vec2D Orientation { get; set; }

        /// <summary>
        /// Drawing direction of the pen. The heading is measured in degrees. Some common directions:<br></br>
        ///    0 - east <br></br>
        ///   90 - north<br></br>
        ///  180 - west <br></br>
        ///  270 - south<br></br>
        /// </summary>
        public double Heading
        {
            get
            {
                return _heading;
            }

            set
            {
                double rotation = value - _heading;
                Rotate(rotation);
            }
        }
        private double _heading;

        /// <summary>
        /// True if pen is down, False if it’s up.
        /// </summary>
        public bool IsDown { get; set; }


        /// <summary>
        /// Change the heading (drawing direction) of the pen by <paramref name="angle"/> units. 
        /// </summary>
        /// <param name="angle">Rotation-value specified in degrees (value of 360 is a full rotation). Positive values turn left, negative values turn right.</param>
        public void Rotate(double angle)
        {
            var newHeading = (Heading + angle) % 360;
            if (newHeading < 0) newHeading += 360;

            var newOrientation = Orientation.Rotate(angle);

            Orientation = newOrientation;
            _heading = newHeading;
        }

        /// <summary>
        /// Move the pen by the specified distance, in the direction the pen is headed. 
        /// <example>
        /// <code>
        /// var pen = new Pen() { IsDown = true }; <br/>
        /// // pen position is (0, 0) <br/>
        /// pen.Move(100); <br/>
        /// // Pen position is (100, 0) <br/>
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="distance">Distance to move. Positive values move forward, negative values move backward.</param>
        public void Move(double distance)
        {
            var newPosition = Position + distance * Orientation;

            if (IsDown)
            {
                DrawMove(Position, newPosition);
            }

            if (_isShapeDrawing)
                _shapeDrawingPolygon.Add(newPosition);

            _position = newPosition;
        }

        /// <summary>
        /// Return fillstate (true if filling, false else)
        /// </summary>
        public bool Filling { get { return _isFilling; } }

        /// <summary>
        /// To be called just before drawing a shape to be filled. See EndFill() for an example.
        /// </summary>
        public void BeginFill()
        {
            if (!_isFilling)
            {
                _isFilling = true;
                _isShapeDrawing = true;
                _shapeDrawingPolygon = new();
                _shapeDrawingPolygon.Add(Position);
            }
        }

        /// <summary>
        /// The Shape resulting from the traversed points between BeginFilling and EndFilling is returned.
        /// </summary>
        /// <returns></returns>
        public void EndFill(Color fillColor)
        {
            Shape shape;
            if (_isFilling)
            {
                _isFilling = false;
                _isShapeDrawing = false;
                shape = new Shape(_shapeDrawingPolygon);
                _shapeDrawingPolygon = null;
            }
            else
            {
                shape = new Shape(new List<Vec2D>());
            }

            // The shape simply is created by creating a new figure:

            var figure = new Figure(_lowLevelScreen) { FillColor = fillColor, OutlineColor = Color, Shape = shape };

            // Imagine the created shape is an arrow like this
            //
            //                     /\
            //                    /  \
            // 
            // Now compare it to the predefined shape Shapes.Arrow.
            // Both shapes point to North
            // But if we create a figure, its shape points to West.
            // Therefore we have to rotate the figure from West to North, such that the shape is drawn in the right way.
            figure.Rotate(90);

            figure.IsVisible = true;

        }

        /// <summary>
        /// Start recording the vertices of a polygon. Current pen  position is first vertex of polygon.
        /// </summary>
        public void BeginPoly()
        {
            if (!_isCreatingPoly)
            {
                _isCreatingPoly = true;
                _isShapeDrawing = true;
                _shapeDrawingPolygon = new();
                _shapeDrawingPolygon.Add(Position);
            }
        }

        /// <summary>
        /// Stop recording the vertices of a polygon. Current pen position is last vertex of polygon. 
        /// </summary>
        /// <returns>The recorded polygon.</returns>
        public List<Vec2D> EndPoly()
        {
            List<Vec2D> polygon;
            if (_isCreatingPoly)
            {
                _isCreatingPoly = false;
                _isShapeDrawing = false;
                polygon = _shapeDrawingPolygon;
                _shapeDrawingPolygon = null;
            }
            else
            {
                polygon = new List<Vec2D>();
            }
            return polygon;
        }

        internal void WaitForCompletedMovementOf(Pen pen)
        {
            throw new NotImplementedException("Not implemented yet (and perhaps not needed?)");
#pragma warning disable CS0162 // Unreachable code detected
            var waitingInfo = new WaitingForCompletedAnimationInfo() { WaitForCompletedAnimationOf = pen._id, WaitingFigure = null, WaitingPen = this };
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


        private void DrawMove(Vec2D oldPosition, Vec2D newPosition)
        {
            var line = new ScreenLine()
            {
                ID = _lowLevelScreen.CreateLine(),
                Color = Color,
                Point1 = oldPosition,
                Point2 = newPosition,
                GroupID = _id,
            };

            if (_firstAnimationIsAdded)
            {
                // Wait for previous animations of this pen (or if the pen belongs to a turtle: wait for previous animations of this pen or the figure)
                line.WaitForCompletedAnimationsOfSameGroup = true;
            }
            else
            {
                // _firstAnimationIsAdded == false
                // This is the first movement of this!
                if (_lowLevelScreen.LastIssuedAnimatonGroupID != ScreenObject.NoGroupId)
                {
                    // A little bit of Artificial Intelligence:
                    // If we do not wait for another animation this pen is drawn immediately. In most cases the programmer expects
                    // that all previously created animation are drawn before this pen is drawn.
                    // Therefore:
                    line.WaitForCompletedAnimationsOfAnotherGroup = _lowLevelScreen.LastIssuedAnimatonGroupID;
                }
            }

            if (_waitingForCompletedAnimationInfo != null)
            {
                // The programmer explicitly wanted to wait for another object
                line.WaitForCompletedAnimationsOfAnotherGroup = _waitingForCompletedAnimationInfo.WaitForCompletedAnimationOf;

                // If figure is waiting. It needs not longer to wait
                if (_waitingForCompletedAnimationInfo.WaitingFigure != null)
                    _waitingForCompletedAnimationInfo.WaitingFigure.ResetWaitingInfo();

                // All subsequent drawings of this should not wait for the other object
                _waitingForCompletedAnimationInfo = null;
            }

            if (!Speed.NoAnimation)
            {
                int speedDuration = Speed.MillisecondsForMovement(oldPosition, newPosition);

                // Animation dazu:
                line.Animation = new ScreenAnimation();
                line.Animation.Effects.Add(new ScreenAnimationMovement() { AnimatedProperty = ScreenAnimationMovementProperty.Point2, StartValue = oldPosition, Milliseconds = speedDuration });
                _firstAnimationIsAdded = true;
            }

            _lowLevelScreen.DrawLine(line);
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
