using System;
using System.Threading;

namespace Woopec.Core
{
    /// <summary>
    /// An instance of this class represents a turtle.
    /// </summary>
    public class Turtle
    {
        private static int s_totalCounter;

        private readonly int _id;
        private readonly IScreen _screen;
        private readonly Pen _pen;
        private readonly Figure _figure;

        /// <summary>
        /// Constructs a turtle that uses the default screen
        /// </summary>
        public Turtle()
            : this(Screen.GetDefaultScreen())
        {

        }

        /// <summary>
        /// Constructs a turtle
        /// </summary>
        /// <param name="screen">Figure is printed on this screen</param>
        /// <remarks>At the moment internal, because multi-screen support is not tested.</remarks>
        internal Turtle(IScreen screen)
        {
            _id = Interlocked.Increment(ref s_totalCounter);
            _screen = screen;
            _figure = new Figure(screen, _id);
            _pen = new Pen(screen, _id);

            // The Turtle should be visible immediately:
            _figure.IsVisible = true;
            // The pen must know that it is not the first screen-operation of the turtle:
            _pen.TurtleObjectSentToScreen();
        }

        /// <summary>
        /// Create a turtle that you can start learning to code with.
        /// </summary>
        /// <returns>A turtle to start with</returns>
        public static Turtle Seymour()
        {
            // See https://en.wikipedia.org/wiki/Turtle_graphics
            //     Seymour Papert added support for turtle graphics to Logo in the late 1960s to support his version of the turtle robot
            var example = new Turtle() { Speed = SpeedLevel.Slowest, Shape = Shapes.Turtle, Color = Colors.DarkGreen };
            return example;
        }

        /// <summary>
        /// Move the turtle forward by the specified distance, in the the direction the turtle is headed. 
        /// <example>
        /// <code>
        /// var turtle = Turtle.Seymour(); <br/>
        /// // Turtle position is (0, 0) <br/>
        /// turtle.Forward(100); <br/>
        /// // Turtle position is (100, 0) <br/>
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="distance"></param>
        public void Forward(double distance)
        {
            _pen.Move(distance);
            bool togetherWithPreviousAnimation = _pen.IsDown; // if pen was down, the figure should not wait untel the pen-line is finished
            _figure.Move(distance, togetherWithPreviousAnimation);
        }

        /// <summary>
        /// Move the turtle backward by the specified distance, opposite to the direction the turtle is headed. 
        /// Do not change the turtle's heading.
        /// <example>
        /// <code>
        /// var turtle = new Turtle(); <br/>
        /// // Turtle position is (0, 0) <br/>
        /// turtle.Backward(100); <br/>
        /// // Turtle position is (-100, 0) <br/>
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="distance"></param>
        public void Backward(double distance)
        {
            _pen.Move(-distance);
            bool togetherWithPreviousAnimation = _pen.IsDown; // if pen was down, the figure should not wait untel the pen-line is finished
            _figure.Move(-distance, togetherWithPreviousAnimation);
        }

        /// <summary>
        /// Move the turtle to the given position. Do not change the turtle's heading.
        /// <example>
        /// <code>
        /// var turtle = new Turtle(); <br/>
        /// // Turtle position is (0, 0) <br/>
        /// turtle.SetPosition((100, 100)); <br/>
        /// // Turtle position is (100, 100) <br/>
        /// turtle.SetPosition(new Vec2D(150, 120)); <br/>
        /// // Turtle position is (150, 120) <br/>
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="position">The new position of the turtle.<br/>
        /// Can be specified as tuple: (50, 30) or as vector: new Vec2D(50, 30)
        /// </param>
        public void SetPosition(Vec2D position)
        {
            _pen.SetPosition(position);
            bool togetherWithPreviousAnimation = _pen.IsDown; // if pen was down, the figure should not wait untel the pen-line is finished
            _figure.SetPosition(position, togetherWithPreviousAnimation);
        }

        /// <summary>
        /// Move the turtle to the given position. Do not change the turtle's heading.
        /// <example>
        /// <code>
        /// var turtle = new Turtle(); <br/>
        /// // Turtle position is (0, 0) <br/>
        /// turtle.GoTo((100, 100)); <br/>
        /// // Turtle position is (100, 100) <br/>
        /// turtle.GoTo(new Vec2D(150, 120)); <br/>
        /// // Turtle position is (150, 120) <br/>
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="position">The new position of the turtle.<br/>
        /// Can be specified as tuple: (50, 30) or as vector: new Vec2D(50, 30)
        /// </param>
        public void GoTo(Vec2D position) => SetPosition(position);

        /// <summary>
        /// Gets or sets the turtle's current position (as a Vec2D vector). Does not change the turtle's heading.
        /// <example>
        /// <code>
        /// var turtle = new Turtle(); <br/>
        /// turtle.Position = (100, 100); <br/>
        /// var actualPosition = turtle.Position;  <br/>
        /// // actualPosition is (100, 100) <br/>
        /// var newPosition = actualPosition + (50, 0);  <br/>
        /// turtle.Position = newPosition;  <br/>
        /// </code>
        /// </example>
        /// </summary>
        /// <value>The new position of the turtle.<br/>
        /// Can be specified as tuple: (50, 30) or as vector: new Vec2D(50, 30)
        /// </value>
        public Vec2D Position { get { return _pen.Position; } set { SetPosition(value); } }

        /// <summary>
        /// Turn turtle left by <paramref name="angle"/> units. 
        /// <example>
        /// <code>
        /// var turtle = new Turtle(); <br/>
        /// // turtle.Heading is 0 and turtle looks to the east <br/>
        /// turtle.Left(90); <br/>
        /// // turtle.Heading is 90 and turtle looks north <br/>
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="angle">Rotation-value specified in degrees (value of 360 is a full rotation).</param>
        public void Left(double angle)
        {
            _pen.Rotate(angle);
            _figure.Rotate(angle);
        }

        /// <summary>
        /// Turn turtle right by <paramref name="angle"/> units. 
        /// <example>
        /// <code>
        /// var turtle = new Turtle(); <br/>
        /// // turtle.Heading is 0 and turtle looks to the east <br/>
        /// turtle.Right(90); <br/>
        /// // turtle.Heading is 270 and turtle looks south<br/>
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="angle">Rotation-value specified in degrees (value of 360 is a full rotation).</param>
        public void Right(double angle)
        {
            _pen.Rotate(-angle);
            _figure.Rotate(-angle);
        }

        /// <summary>
        /// The orientation of the turtle. The heading is measured in degrees. Some common directions:<br></br>
        ///    0 - east <br></br>
        ///   90 - north<br></br>
        ///  180 - west <br></br>
        ///  270 - south<br></br>
        /// <br></br>
        /// Examples:
        /// <example>
        /// <code>
        /// var turtle = new Turtle(); <br/>
        /// // Get the heading (in this example the heading is 0 and turtle looks to the east):<br/>
        /// var heading = turtle.Heading; <br></br>
        /// // Set the heading (turtle will rotate left and finally look north):<br/>
        /// turtle.Heading = 90; <br/>
        /// </code>
        /// </example>
        /// </summary>
        public double Heading
        {
            get { return _pen.Heading; }
            set
            {
                double rotation = value - _pen.Heading;
                _pen.Rotate(rotation);
                _figure.Rotate(rotation);
            }
        }

        /// <summary>
        /// Sets the orientation of the turtle. The heading is measured in degrees. Some common directions:<br></br>
        ///    0 - east <br></br>
        ///   90 - north<br></br>
        ///  180 - west <br></br>
        ///  270 - south<br></br>
        /// Example:
        /// <example>
        /// <code>
        /// var turtle = new Turtle(); <br/>
        /// // heading is 0 and turtle looks to the east <br/>
        /// turtle.SetHeading(90); <br/>
        /// // turtle rotates left and finally looks north<br/>
        /// </code>
        /// </example>
        /// </summary>
        public void SetHeading(double value)
        {
            Heading = value;
        }

        /// <summary>
        /// Speed of the turtle.
        /// Speed from SpeedLevel.Slowest to SpeedLevel.Fast enforce increasingly faster animation of
        /// line drawing and turtle turning.<br></br>
        /// Attention: With SpeedLevel.Fastest *no* animation takes place.<br></br>
        /// <br></br>
        /// <para>
        /// Examples for setting the speed:
        /// <example>
        /// <code>
        /// turtle.Speed = SpeedLevel.Slowest; // turtle moves very slow <br/>
        /// turtle.Speed = SpeedLevel.Fast;    // turtle moves very fast<br/>
        /// turtle.Speed = SpeedLevel.Fastest; // No animation takes place<br/>
        /// </code>
        /// </example>
        /// </para>
        /// <br></br>
        /// <para>
        /// Examples for getting and checking the speed:
        /// <example>
        /// <code>
        /// var currentSpeed = turtle.Speed; <br/>
        /// if (currentSpeed == SpeedLevel.Slow) ...<br/>
        /// if (currentSpeed.NoAnimation) ...<br></br>
        /// </code>
        /// </example>
        /// </para>
        /// </summary>
        public Speed Speed { get { return _pen.Speed; } set { _pen.Speed = value; _figure.Speed = value; } }




        public Color PenColor { get { return _pen.Color; } set { _pen.Color = value; _figure.OutlineColor = value; } }

        public Color FillColor { get { return _figure.FillColor; } set { _figure.FillColor = value; } }

        /// <summary>
        /// Set pen color and fill color.
        /// </summary>
        /// <remarks>
        /// This property has no getter. Use PenColor and FillColor.
        /// </remarks>
        public Color Color { set { PenColor = value; FillColor = value; } }

        /// <summary>
        /// True if pen is down, False if it’s up.
        /// </summary>
        public bool IsDown { get { return _pen.IsDown; } set { _pen.IsDown = value; } }

        /// <summary>
        /// Pull the pen down – drawing when moving.
        /// </summary>
        public void PenDown() { IsDown = true; }

        /// <summary>
        /// True if figure is visible, false if is not visible
        /// </summary>
        public bool IsVisible { get { return _figure.IsVisible; } set { _figure.IsVisible = value; } }

        /// <summary>
        /// Set the turtles shape.
        /// </summary>
        public ShapeBase Shape { get { return _figure.Shape; } set { _figure.Shape = value; } }

        public void HideTurtle() { IsVisible = false; }
        public void ShowTurtle() { IsVisible = true; }


        /// <summary>
        /// Pull the pen up – no drawing when moving.
        /// </summary>
        public void PenUp() { IsDown = false; }

        public bool Filling { get { return _pen.Filling; } }

        public void BeginFill()
        {
            _pen.BeginFill();
        }

        public void EndFill()
        {
            var shape = _pen.EndFill();

            var figure = new Figure(_screen, _id) { FillColor = FillColor, OutlineColor = PenColor, Shape = shape };

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
        /// Only needed for unit tests
        /// </summary>
        internal Pen Pen { get { return _pen; } }

        /// <summary>
        /// Only needed for unit tests
        /// </summary>
        internal Figure Figure {  get { return _figure; } }
    }
}
