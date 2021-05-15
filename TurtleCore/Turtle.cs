using System.Threading;

namespace TurtleCore
{
    /// <summary>
    /// An instance of this class represents a turtle.
    /// Documentation: see https://docs.python.org/3/library/turtle.html#module-turtle
    /// python Implementation: see https://github.com/python/cpython/blob/master/Lib/turtle.py
    /// Sample turtle programs: see https://github.com/python/cpython/tree/master/Lib/turtledemo
    /// </summary>
    public class Turtle
    {
        private static int s_totalCounter;

        private readonly int _id;
        private readonly Pen _pen;
        private readonly Figure _figure;

        public Turtle()
        {
            _id = Interlocked.Increment(ref s_totalCounter);
            _figure = new Figure(_id);
            _pen = new Pen(_id);

            // The Turtle should be visible immediately:
            _figure.IsVisible = true;
            // The pen must know that it is not the first screen-operation of the turtle:
            _pen.TurtleObjectSentToScreen();
        }

        public Vec2D Position { get { return _pen.Position; } }


        public Color PenColor { get { return _pen.Color; } set { _pen.Color = value; } }

        public Speed Speed { get { return _pen.Speed; } set { _pen.Speed = value; _figure.Speed = value; } }

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

        public void HideTurtle() { IsVisible = false; }
        public void ShowTurtle() { IsVisible = true; }


        /// <summary>
        /// Pull the pen up – no drawing when moving.
        /// </summary>
        public void PenUp() { IsDown = false; }

        /// <summary>
        /// Orientation of the turtle.
        /// Here are some common directions in degrees:
        ///  standard - mode:          logo-mode:
        /// -------------------|--------------------
        ///    0 - east                0 - north
        ///   90 - north              90 - east
        ///  180 - west              180 - south
        ///  270 - south             270 - west
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


        public void Forward(double distance)
        {
            _pen.Move(distance);
            bool togetherWithPreviousAnimation = _pen.IsDown; // if pen was down, the figure should not wait untel the pen-line is finished
            _figure.Move(distance, togetherWithPreviousAnimation);
        }

        public void Backward(double distance)
        {
            _pen.Move(-distance);
            bool togetherWithPreviousAnimation = _pen.IsDown; // if pen was down, the figure should not wait untel the pen-line is finished
            _figure.Move(distance, togetherWithPreviousAnimation);
        }


        public void SetPosition(Vec2D position)
        {
            _pen.SetPosition(position);
            bool togetherWithPreviousAnimation = _pen.IsDown; // if pen was down, the figure should not wait untel the pen-line is finished
            _figure.SetPosition(position, togetherWithPreviousAnimation);
        }

        public void Left(double angle)
        {
            _pen.Rotate(angle);
            _figure.Rotate(angle);
        }

        public void Right(double angle)
        {
            _pen.Rotate(-angle);
            bool togetherWithPreviousAnimation = _pen.IsDown; // if pen was down, the figure should not wait untel the pen-line is finished
            _figure.Rotate(-angle);
        }


    }
}
