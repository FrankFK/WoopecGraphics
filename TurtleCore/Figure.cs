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

        private readonly Screen _screen;
        private string _shapeName;
        private int _idOnScreen;
        private bool _isVisible;

        public Vec2D Position { get; set; }

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
                if (!_isVisible && value)
                    ShowOnScreen();
                else
                    HideOnScreen();
                _isVisible = value;
            }
        }

        public Color FillColor { get; set; }

        public Color OutlineColor { get; set; }

        public Speed Speed { get; set; }

        /// <summary>
        /// Constructor for a Figure that is not used as part of a Turtle class
        /// </summary>
        public Figure()
            : this(Interlocked.Increment(ref s_totalCounter))
        {
        }

        /// <summary>
        /// Constructor for a Pen that is used as a part of a Turtle class
        /// </summary>
        /// <param name="id">The Id of the turtle</param>
        public Figure(int id)
        {
            _id = id;
            _screen = Screen.GetDefaultScreen();
            Position = new Vec2D(0, 0);
            Orientation = new Vec2D(1, 0);
            Heading = 0;
            _isVisible = false;
            Speed = SpeedLevel.Normal;
            FillColor = Colors.Black;
            OutlineColor = Colors.Black;
            _shapeName = ShapeNames.Classic;

            _idOnScreen = _screen.CreateFigure(_shapeName);
        }

        public void Rotate(double angle)
        {
            var newHeading = (Heading + angle) % 360;
            if (newHeading < 0) newHeading += 360;

            var newOrientation = Orientation.Rotate(angle);

            // TODO: Turtle neu anzeigen
            Orientation = newOrientation;
            Heading = newHeading;

            if (IsVisible)
                ShowOnScreen();
        }

        public void Move(double distance)
        {
            var newPosition = Position + distance * Orientation;

            // TODO: Turtle bewegen

            Position = newPosition;

            if (IsVisible)
                ShowOnScreen();

        }


        private void ShowOnScreen()
        {
            var figure = new ScreenFigure(_idOnScreen)
            {
                IsVisible = true,
                Position = Position,
                FillColor = FillColor,
                OutlineColor = OutlineColor,
                Heading = Heading,
                GroupID = _id,
            };
            if (!Speed.NoAnimation && _screen.LastIssuedAnimatonGroupID != ScreenObject.NoGroupId)
            {
                // If we do not wait for another animation this turtle is drawn immediately. In most cases the programmer expects
                // that all previously created animation are drawn before this pen is drawn.
                // Therefore:
                figure.WaitForAnimationsOfGroupID = _screen.LastIssuedAnimatonGroupID;
            }
            _screen.UpdateFigure(figure);

        }

        private void HideOnScreen()
        {
            throw new NotImplementedException();
        }
    }
}
