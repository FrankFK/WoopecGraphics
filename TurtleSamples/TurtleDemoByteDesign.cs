using System;
using System.Diagnostics;
using System.Linq;
using TurtleCore;

namespace TurtleSamples
{
    /// <summary>
    /// This is a port of https://github.com/python/cpython/blob/main/Lib/turtledemo/bytedesign.py
    /// Description from there:
    ///         An example adapted from the example-suite
    ///         of PythonCard's turtle graphics.
    ///         It's based on an article in BYTE magazine
    ///         Problem Solving with Logo: Using Turtle
    ///         Graphics to Redraw a Design
    ///         November 1982, p. 118 - 134
    /// </summary>
    public class TurtleDemoByteDesign : Turtle
    {
        public static void Run()
        {
            var design = new TurtleDemoByteDesign();
            design.HideTurtle();
            design.Speed = SpeedLevel.Fast;
            design.design(design.Position, 2);
        }

        private void design(Vec2D homePos, double scale)
        {
            this.PenUp();
            foreach (var i in Enumerable.Range(0, 5))
            {
                this.Forward(64.65 * scale);
                this.PenDown();
                this.wheel(this.Position, scale);
                this.PenUp();
                this.Backward(64.65 * scale);
                this.Right(72);
            }
            this.PenUp();
            this.SetPosition(homePos);
            this.Right(36);
            this.Forward(24.5 * scale);
            this.Right(198);
            this.PenDown();
            this.centerpiece(46 * scale, 143.4, scale);
            // this.Screen.tracer(true);
        }

        private void wheel(Vec2D initpos, double scale)
        {
            this.Right(54);
            foreach (var i in Enumerable.Range(0, 4))
            {
                this.pentpiece(initpos, scale);
            }
            this.PenDown();
            this.Left(36);
            foreach (var i in Enumerable.Range(0, 5))
            {
                this.tripiece(initpos, scale);
            }
            this.Left(36);
            foreach (var i in Enumerable.Range(0, 5))
            {
                this.PenDown();
                this.Right(72);
                this.Forward(28 * scale);
                this.PenUp();
                this.Backward(28 * scale);
            }
            this.Left(54);
            // this.Screen().update();
        }

        private void tripiece(Vec2D initpos, double scale)
        {
            var oldh = this.Heading;
            this.PenDown();
            this.Backward(2.5 * scale);
            this.tripolyr(31.5 * scale, scale);
            this.PenUp();
            this.SetPosition(initpos);
            this.Heading = oldh;
            this.PenDown();
            this.Backward(2.5 * scale);
            this.tripolyl(31.5 * scale, scale);
            this.PenUp();
            this.SetPosition(initpos);
            this.Heading = oldh;
            this.Left(72);
            // this.Screen.update();
        }

        private void pentpiece(Vec2D initpos, double scale)
        {
            var oldh = this.Heading;
            this.PenUp();
            this.Forward(29 * scale);
            this.PenDown();
            foreach (var i in Enumerable.Range(0, 5))
            {
                this.Forward(18 * scale);
                this.Right(72);
            }
            this.pentr(18 * scale, 75, scale);
            this.PenUp();
            this.SetPosition(initpos);
            this.Heading = oldh;
            this.Forward(29 * scale);
            this.PenDown();
            foreach (var j in Enumerable.Range(0, 5))
            {
                this.Forward(18 * scale);
                this.Right(72);
            }
            this.pentl(18 * scale, 75, scale);
            this.PenUp();
            this.SetPosition(initpos);
            this.Heading = oldh;
            this.Left(72);
            // this.Screen.update();
        }

        private void pentl(double side, double ang, double scale)
        {
            if (side < (2 * scale))
                return;
            this.Forward(side);
            this.Left(ang);
            this.pentl(side - (.38 * scale), ang, scale);
        }

        private void pentr(double side, double ang, double scale)
        {
            if (side < (2 * scale))
                return;
            this.Forward(side);
            this.Right(ang);
            this.pentr(side - (.38 * scale), ang, scale);
        }

        private void tripolyr(double side, double scale)
        {
            if (side < (4 * scale))
                return;
            Forward(side);
            Right(111);
            Forward(side / 1.78);
            Right(111);
            Forward(side / 1.3);
            Right(146);
            tripolyr(side * .75, scale);
        }

        private void tripolyl(double side, double scale)
        {
            if (side < (4 * scale))
                return;
            Forward(side);
            Left(111);
            Forward(side / 1.78);
            Left(111);
            Forward(side / 1.3);
            Left(146);
            tripolyl(side * .75, scale);

        }

        private void centerpiece(double s, double a, double scale)
        {
            Forward(s);
            Left(a);
            if (s < (7.5 * scale))
                return;
            centerpiece(s - (1.2 * scale), a, scale);
        }

    }
}
