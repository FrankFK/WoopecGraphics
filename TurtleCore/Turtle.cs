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
        private readonly Pen _pen;
        private readonly Form _form;

        public Vec2D Position { get { return _pen.Position; } set { _pen.Position = value; } }

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
                _pen.Rotate(value - Heading);
                _form.Rotate(value - Heading);
            }
        }


        public Turtle()
        {
            _pen = new Pen();
            _form = new Form();
        }

        public void Forward(double distance)
        {
            _pen.Move(distance);
            _form.Move(distance);
        }

        public void Backward(double distance)
        {
            _pen.Move(-distance);
            _form.Move(-distance);
        }

        public void Left(double angle)
        {
            _pen.Rotate(angle);
            _form.Rotate(angle);
        }

        public void Right(double angle)
        {
            _pen.Rotate(-angle);
            _form.Rotate(-angle);
        }


    }
}
