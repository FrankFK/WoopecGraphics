using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Woopec.Core;

namespace Woopec.Core.Examples
{
    /// <summary>
    /// This example draws hypotrochoids (also known from the Spirograph game).
    /// See https://en.wikipedia.org/wiki/Hypotrochoid for details.
    /// </summary>
    public static class Spirograph
    {
        /// <summary>
        /// Create a spirograph curve and return the points of the curve
        /// </summary>
        /// <param name="givenCorners">The wanted number of corners.</param>
        /// <param name="givenDelta">The wanted delta, corner 1 is connected with corner 1 + givenDelta</param>
        /// <param name="relativeDistance">Distance between the center of the inner disc and the drawing hole. Value 0.0 specifies the disc center, value 1.0 specifies the disc border</param>
        /// <param name="rLarge">Radius of the outer sprirograph wheel.</param>
        /// <returns>The coordinates of the points of the spriograph curve (point (0,0) is the center).</returns>
        /// <remarks>If givenCorners and givenDelta have a common divisor, these values make no sense.
        /// In such a case the GCD of givenCorners and givenDelta is calculated and a star with
        /// givenCorners/GCD number of corners and a delta of givenDelta/GCD is calculated.
        /// </remarks>
        public static List<Vec2D> SpiroPoly(int givenCorners, int givenDelta, double relativeDistance, double rLarge)
        {
            var center = new Vec2D(0, 0);

            var gcd = Fractions.GCD(givenCorners, givenDelta);
            var corners = givenCorners / gcd;
            var delta = givenDelta / gcd;

            var rSmall = (rLarge * delta) / corners;
            var distance = rSmall * relativeDistance;
            var maxT = 2 * Math.PI * corners;


            var pen = new Turtle() { IsVisible = false, IsDown = false, Speed = Speeds.Fastest };

            var firstPos = true;
            var deltaT = 2.0 / distance;

            for (var t = 0.0; t < maxT; t += deltaT)
            {
                var pos = CalcPenPos(t, rLarge, rSmall, distance);
                pen.Position = center + pos;
                if (firstPos)
                {
                    pen.BeginPoly();
                    firstPos = false;
                }
            }

            return pen.EndPoly();
        }

        /// <summary>
        /// Create a spirograph animation which draws the curve for the given parameters
        /// </summary>
        /// <param name="givenCorners">The wanted number of corners.</param>
        /// <param name="givenDelta">The wanted delta, corner 1 is connected with corner 1 + givenDelta</param>
        /// <param name="relativeDistance">Distance between the center of the inner disc and the drawing hole. Value 0.0 specifies the disc center, value 1.0 specifies the disc border</param>
        /// <param name="rLarge">Radius of the outer sprirograph wheel.</param>
        /// <param name="center">The center-position of the spirograph curve</param>
        /// <param name="penColor">The color of the spirograph curve</param>
        /// <remarks>If givenCorners and givenDelta have a common divisor, these values make no sense.
        /// In such a case the GCD of givenCorners and givenDelta is calculated and a star with
        /// givenCorners/GCD number of corners and a delta of givenDelta/GCD is calculated.
        /// </remarks>
        public static void WithWheels(int givenCorners, int givenDelta, double relativeDistance, double rLarge, Vec2D center, Color penColor)
        {
            var gcd = Fractions.GCD(givenCorners, givenDelta);
            var corners = givenCorners / gcd;
            var delta = givenDelta / gcd;

            var largeCircleTurtle = new Turtle() { IsVisible = false, IsDown = false, Shape = new Shape(Stars.StarPoly(100, 1, rLarge)), Speed = Speeds.Fastest, PenColor = Colors.LightGray, FillColor = Colors.White.Transparent(1.0), Position = center };
            largeCircleTurtle.ShowTurtle();

            var rSmall = (rLarge * delta) / corners;

            var smallCirclePoly = Stars.StarPoly(100, 1, rSmall);
            var centerCirclePoly = Stars.StarPoly(10, 1, 2);
            var dotCircleRadius = 5;
            var dotCirclePoly = Stars.StarPoly(10, 1, dotCircleRadius).Select(p => p + (0, rSmall * relativeDistance)).ToList();
            var lineToDotPoly = new List<Vec2D>() { (0, 0), (0, Math.Max(0, (rSmall * relativeDistance) - dotCircleRadius)), (0, 0) };

            var shape = new Shape(smallCirclePoly);
            shape.AddComponent(centerCirclePoly);
            shape.AddComponent(lineToDotPoly);
            shape.AddComponent(dotCirclePoly);


            var innerWheel = new Turtle() { IsVisible = false, IsDown = false, Shape = shape, Speed = Speeds.Fastest, PenColor = Colors.LightSlateGray, FillColor = Colors.White.Transparent(0.5) };
            innerWheel.Position = (center.X, center.Y + (rLarge + rSmall) * 1.25);
            innerWheel.IsVisible = true;
            innerWheel.Speed = Speeds.Slowest;
            innerWheel.Position = center + (rLarge - rSmall, 0);

            var distance = rSmall * relativeDistance;

            var maxT = 2 * Math.PI * corners;

            var speed = Speeds.Fast;

            var pen = new Turtle() { IsVisible = false, IsDown = false, Speed = speed, Color = penColor, Shape = new Shape(Stars.StarPoly(10, 1, dotCircleRadius / 2.0)) };
            innerWheel.Speed = speed;

            var firstPos = true;
            var deltaT = 2.0 / distance;
            var loopCount = maxT / deltaT;
            var innerWheelRotAngle = (corners - delta) * 360.0 / loopCount;

            innerWheel.Left(innerWheelRotAngle); // weil wir mit t = 0.0 anfangen.

            for (var t = 0.0; t < maxT; t += deltaT)
            {
                pen.WaitForCompletedMovementOf(innerWheel);
                var pos = CalcPenPos(t, rLarge, rSmall, distance);
                pen.Position = center + pos;
                innerWheel.Position = center + CalcInnerWheelCenterPos(t, rLarge, rSmall); // (Math.Cos(t * delta / corners) * (rLarge - rSmall), Math.Sin(t * delta / corners) * (rLarge - rSmall));
                innerWheel.Right(innerWheelRotAngle);
                if (firstPos)
                {
                    pen.PenDown();
                    pen.ShowTurtle();
                }
            }

            pen.PenUp();
            pen.HideTurtle();

            largeCircleTurtle.WaitForCompletedMovementOf(pen);

            largeCircleTurtle.HideTurtle();
            innerWheel.Speed = Speeds.Slow;
            innerWheel.Position = (center.X, center.Y - (rLarge + rSmall) * 1.25);
        }

        /// <summary>
        /// A demo which draws a few spirograph curves
        /// </summary>
        public static void SpiroDemo()
        {
            var radius = 70;
            var distance = 10;
            var xPos = -300;

            Vec2D firstCenterPos = (xPos, 0);
            var secondStartPos = (-xPos, 0);

            var speed = Speeds.Slow;
            var penColor = Colors.DarkRed;
            var fillColor = Colors.DarkRed.Transparent(0.1);

            var shapeFor7_5 = new Shape(SpiroPoly(7, 5, 0.6, radius));
            var spiro7_5 = new Turtle() { IsVisible = false, IsDown = false, Shape = shapeFor7_5, PenColor = penColor, FillColor = fillColor, Position = secondStartPos, Speed = speed };
            spiro7_5.Left(90);

            fillColor = Colors.DarkRed.Transparent(0.9);

            var shapeFor7_3 = new Shape(SpiroPoly(7, 3, 0.8, radius));
            var spiro7_3 = new Turtle() { IsVisible = false, IsDown = false, Shape = shapeFor7_3, PenColor = penColor, FillColor = fillColor, Position = secondStartPos, Speed = speed };
            spiro7_3.Left(90);

            speed = Speeds.Slowest;
            var colorA = Colors.MediumBlue;
            var colorB = Colors.Navy;
            fillColor = colorA.Transparent(0.5);
            penColor = fillColor;

            var shapeA = new Shape(SpiroPoly(13, 9, 0.8, radius));
            var spiroA = new Turtle() { IsVisible = false, IsDown = false, Shape = shapeA, PenColor = penColor, FillColor = fillColor, Position = secondStartPos, Speed = speed };
            spiroA.Left(90);

            fillColor = colorB.Transparent(0.5);
            penColor = fillColor;

            var shapeB = new Shape(SpiroPoly(13, 9, 0.8, radius));
            var spiroB = new Turtle() { IsVisible = false, IsDown = false, Shape = shapeB, PenColor = penColor, FillColor = fillColor, Position = secondStartPos, Speed = speed };
            spiroB.Left(90);

            var withWheelsCorners = 5;
            var withWheelsDelta = 3;
            var withWheelsDistance = 0.8;
            var withWheelsColor = Colors.DarkBlue;
            WithWheels(withWheelsCorners, withWheelsDelta, withWheelsDistance, radius, firstCenterPos, withWheelsColor);

            var centerPos = firstCenterPos;
            centerPos += (2 * radius + distance, 0);

            spiro7_5.IsVisible = true;
            spiro7_5.Position = centerPos;

            spiro7_3.WaitForCompletedMovementOf(spiro7_5);
            spiro7_3.IsVisible = true;
            spiro7_3.Position = centerPos;

            centerPos += (2 * radius + distance, 0);

            spiroA.WaitForCompletedMovementOf(spiro7_3);
            spiroA.IsVisible = true;
            spiroA.Position = centerPos;

            spiroB.WaitForCompletedMovementOf(spiroA);
            spiroB.IsVisible = true;
            spiroB.Position = centerPos;

            spiroA.WaitForCompletedMovementOf(spiroB);
            spiroA.Speed = 0.6;
            spiroB.Speed = 0.6;
            spiroA.Left(360.0 / 13 * 10);
            spiroB.Right(360.0 / 13 * 10);

        }

        /// <summary>
        /// A demo which uses a spirograph curve to draw a nice animated picture
        /// </summary>
        public static void SpiroDemo2()
        {
            var corners = 5;
            var delta = 3;
            var distance = 0.8;
            var radius = 100;
            var color = Colors.Maroon;

            // Woopec.Core.Examples.Spirograph.WithWheels(corners, delta, distance, radius, (0, 0), color);

            var spiroShape = new Woopec.Core.Shape(Woopec.Core.Examples.Spirograph.SpiroPoly(corners, delta, distance, radius));

            var list = new List<Turtle>();
            var multiples = 15;
            var angle = 360.0 / corners / multiples;
            var forward = 0.0;
            for (var i = 0; i <= 2 * multiples * corners; i++)
            {
                var color2 = Color.FromHSV(angle * i / 2.0, 1, 0.8);
                var s = new Turtle()
                {
                    IsVisible = false,
                    IsDown = false,
                    FillColor = color2.Transparent(0.5),
                    PenColor = Colors.White,
                    Shape = spiroShape,
                };
                s.Speed = Speeds.Normal;
                s.Left(90.0 + i * angle);
                if (i < multiples * corners)
                    forward++;
                else
                    forward--;
                s.Forward(forward - 1);
                s.IsVisible = true;
                s.Forward(1);
                list.Add(s);
            }

            var lastMovedIndex = list.Count - 1;
            var visible = false;
            for (var totalRuns = 8; totalRuns >= 1; totalRuns--)
            {
                for (var j = list.Count - 1; j >= 0; j--)
                {
                    if (j == list.Count - 1)
                    {
                        if (lastMovedIndex != j)
                            list[j].WaitForCompletedMovementOf(list[lastMovedIndex]);
                    }
                    else
                        list[j].WaitForCompletedMovementOf(list[j + 1]);

                    // We need one visible move (first try)
                    if (list[j].IsVisible)
                        list[j].Forward(1);

                    list[j].IsVisible = visible;

                    // We need one visible move (second try)
                    if (list[j].IsVisible)
                        list[j].Forward(10);
                }
                visible = !visible;
                lastMovedIndex = 0;
                Thread.Sleep(1000);
            }

        }

        private static Vec2D CalcPenPos(double t, double rLarge, double rSmall, double distance)
        {
            var sizeTerm = rLarge - rSmall;
            var ratio = rSmall / rLarge;
            var angle1 = ratio * t;
            var angle2 = (1 - ratio) * t;
            var x = sizeTerm * Math.Cos(angle1) + distance * Math.Cos(angle2);
            var y = sizeTerm * Math.Sin(angle1) - distance * Math.Sin(angle2);
            return new Vec2D(x, y);
        }

        private static Vec2D CalcInnerWheelCenterPos(double t, double rLarge, double rSmall)
        {
            var distanceToLargeWheelCenter = rLarge - rSmall;
            return new Vec2D(
                Math.Cos(t * rSmall / rLarge) * distanceToLargeWheelCenter,
                Math.Sin(t * rSmall / rLarge) * distanceToLargeWheelCenter
            );
        }

    }

}
