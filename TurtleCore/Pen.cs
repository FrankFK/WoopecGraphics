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
            // See https://en.wikipedia.org/wiki/Turtle_graphics
            //     Seymour Papert added support for turtle graphics to Logo in the late 1960s to support his version of the turtle robot
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

            if (_isShapeDrawing)
                _shapeDrawingPolygon.Add(value);

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

            if (_isShapeDrawing)
                _shapeDrawingPolygon.Add(newPosition);

            _position = newPosition;
        }

        public bool Filling { get { return _isFilling; } }

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
        /// Start recording the vertices of a polygon. Current turtle position is first vertex of polygon.
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
        /// Stop recording the vertices of a polygon. Current turtle position is last vertex of polygon. 
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

        public void WaitForCompletedMovementOf(Pen pen)
        {
            throw new NotImplementedException("Not tested yet");
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
