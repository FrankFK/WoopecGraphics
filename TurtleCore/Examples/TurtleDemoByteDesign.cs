using System;
using System.Diagnostics;
using System.Linq;
using Woopec.Core;

namespace Woopec.Examples
{
    /// <summary>
    /// This is a port of https://github.com/python/cpython/blob/main/Lib/turtledemo/bytedesign.py
    /// Description from there:<br></br>
    ///         An example adapted from the example-suite
    ///         of PythonCard's turtle graphics.
    ///         It's based on an article in BYTE magazine
    ///         Problem Solving with Logo: Using Turtle
    ///         Graphics to Redraw a Design
    ///         November 1982, p. 118 - 134
    /// <br></br>
    /// <br></br>
    /// This example shows the performance with Speed.Fastest (no animations)
    /// </summary>
    public class TurtleDemoByteDesign : Turtle
    {
        /// <summary>
        /// This is a port of https://github.com/python/cpython/blob/main/Lib/turtledemo/bytedesign.py
        /// Description from there:<br></br>
        ///         An example adapted from the example-suite
        ///         of PythonCard's turtle graphics.
        ///         It's based on an article in BYTE magazine
        ///         Problem Solving with Logo: Using Turtle
        ///         Graphics to Redraw a Design
        ///         November 1982, p. 118 - 134
        /// <br></br>
        /// <br></br>
        /// This example shows the performance with Speed.Fastest (no animations)
        /// </summary>
        public static void Run()
        {
            var design = new TurtleDemoByteDesign();
            design.HideTurtle();
            design.Speed = Speeds.Fastest;
            design.Design(design.Position, 2);
        }

        private void Design(Vec2D homePos, double scale)
        {
            PenUp();
            foreach (var i in Enumerable.Range(0, 5))
            {
                Forward(64.65 * scale);
                PenDown();
                Wheel(Position, scale);
                PenUp();
                Backward(64.65 * scale);
                Right(72);
            }
            PenUp();
            GoTo(homePos);
            Right(36);
            Forward(24.5 * scale);
            Right(198);
            PenDown();
            CenterPiece(46 * scale, 143.4, scale);
        }

        private void Wheel(Vec2D initpos, double scale)
        {
            Right(54);
            foreach (var i in Enumerable.Range(0, 4))
            {
                PentPiece(initpos, scale);
            }
            PenDown();
            Left(36);
            foreach (var i in Enumerable.Range(0, 5))
            {
                TriPiece(initpos, scale);
            }
            Left(36);
            foreach (var i in Enumerable.Range(0, 5))
            {
                PenDown();
                Right(72);
                Forward(28 * scale);
                PenUp();
                Backward(28 * scale);
            }
            Left(54);
        }

        private void TriPiece(Vec2D initpos, double scale)
        {
            var oldh = Heading;
            PenDown();
            Backward(2.5 * scale);
            TriPolyRight(31.5 * scale, scale);
            PenUp();
            GoTo(initpos);
            SetHeading(oldh);
            PenDown();
            Backward(2.5 * scale);
            TriPolyLeft(31.5 * scale, scale);
            PenUp();
            GoTo(initpos);
            SetHeading(oldh);
            Left(72);
        }

        private void PentPiece(Vec2D initpos, double scale)
        {
            var oldh = Heading;
            PenUp();
            Forward(29 * scale);
            PenDown();
            foreach (var i in Enumerable.Range(0, 5))
            {
                Forward(18 * scale);
                Right(72);
            }
            PentRight(18 * scale, 75, scale);
            PenUp();
            GoTo(initpos);
            SetHeading(oldh);
            Forward(29 * scale);
            PenDown();
            foreach (var j in Enumerable.Range(0, 5))
            {
                Forward(18 * scale);
                Right(72);
            }
            PentLeft(18 * scale, 75, scale);
            PenUp();
            GoTo(initpos);
            SetHeading(oldh);
            Left(72);
        }

        private void PentLeft(double side, double ang, double scale)
        {
            if (side < (2 * scale))
                return;
            Forward(side);
            Left(ang);
            PentLeft(side - (.38 * scale), ang, scale);
        }

        private void PentRight(double side, double ang, double scale)
        {
            if (side < (2 * scale))
                return;
            Forward(side);
            Right(ang);
            PentRight(side - (.38 * scale), ang, scale);
        }

        private void TriPolyRight(double side, double scale)
        {
            if (side < (4 * scale))
                return;
            Forward(side);
            Right(111);
            Forward(side / 1.78);
            Right(111);
            Forward(side / 1.3);
            Right(146);
            TriPolyRight(side * .75, scale);
        }

        private void TriPolyLeft(double side, double scale)
        {
            if (side < (4 * scale))
                return;
            Forward(side);
            Left(111);
            Forward(side / 1.78);
            Left(111);
            Forward(side / 1.3);
            Left(146);
            TriPolyLeft(side * .75, scale);

        }

        private void CenterPiece(double s, double a, double scale)
        {
            Forward(s);
            Left(a);
            if (s < (7.5 * scale))
                return;
            CenterPiece(s - (1.2 * scale), a, scale);
        }

    }
}
