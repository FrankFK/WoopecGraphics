using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Woopec.Graphics;

namespace Woopec.Graphics.Examples
{
    /// <summary>
    /// This example draws hypotrochoids (also known from the Spirograph game).
    /// See https://en.wikipedia.org/wiki/Hypotrochoid for details.
    /// </summary>
    public static class Stars
    {

        /// <summary>
        /// Draw a star
        /// </summary>
        /// <param name="turtle">Use this turtle to draw the star</param>
        /// <param name="givenCorners">The wanted number of corners.</param>
        /// <param name="givenDelta">The wanted delta, corner 1 is connected with corner 1 + givenDelta</param>
        /// <param name="radius">Radius of the star. The center is the actual turtle position</param>
        /// <remarks>If givenCorners and givenDelta have a common divisor, these values make no sense.
        /// In such a case the GCD of givenCorners and givenDelta is calculated and a star with
        /// givenCorners/GCD number of corners and a delta of givenDelta/GCD is calculated.
        /// </remarks>
        public static void DrawStar(Turtle turtle, int givenCorners, int givenDelta, double radius)
        {
            var polygonAround0 = StarPoly(givenCorners, givenDelta, radius);

            var polygon = polygonAround0.Select(p => p + turtle.Position).ToList();

            var initiallyWasDown = turtle.IsDown;
            var initialHeading = turtle.Heading;
            turtle.IsDown = false;

            for (var i = 0; i < polygon.Count; i++)
            {
                var nextPosition = polygon[i];
                turtle.Heading = turtle.Position.HeadingTo(nextPosition);
                turtle.Position = polygon[i];
                if (i == 0)
                    turtle.IsDown = true;
            }

            turtle.IsDown = initiallyWasDown;
            turtle.Heading = initialHeading;
        }


        /// <summary>
        /// Return the points of a star with (0, 0) as center
        /// </summary>
        /// <param name="givenCorners">The wanted number of corners.</param>
        /// <param name="givenDelta">The wanted delta, corner 1 is connected with corner 1 + givenDelta</param>
        /// <param name="radius">Radius of the star. The center is the actual turtle position</param>
        /// <returns>The coordinates of the cornes of the star</returns>
        /// <remarks>If givenCorners and givenDelta have a common divisor, these values make no sense.
        /// In such a case the GCD of givenCorners and givenDelta is calculated and a star with
        /// givenCorners/GCD number of corners and a delta of givenDelta/GCD is calculated.
        /// </remarks>
        public static List<Vec2D> StarPoly(int givenCorners, int givenDelta, double radius)
        {
            // If the given values for corners and delta have a common divison, the resulting star has less corners
            var gcd = Fractions.GCD(givenCorners, givenDelta);

            // Real corners and real delta of the resulting star:
            var corners = givenCorners / gcd;
            var delta = givenDelta / gcd;

            if (delta > corners / 2.0)
                delta = corners - delta;

            CalculateStarParameters(radius, corners, delta, out var turnAngle, out var innerAngle, out var length);

            var turtle = new Turtle() { IsVisible = false, IsDown = false, Speed = Speeds.Fastest };

            turtle.Forward(radius);
            turtle.Left(innerAngle);
            turtle.BeginPoly();

            for (var i = 0; i < corners; i++)
            {
                turtle.Left(turnAngle);
                turtle.Forward(length);
            }

            return turtle.EndPoly();
        }

        /// <summary>
        /// Demo with some polygons and stars
        /// </summary>
        public static void StarDemo()
        {
            var radius = 40;
            var distance = 10;
            var xPos = -300;

            var turtles = new List<Turtle>();
            for (var i = 0; i < 20; i++)
            {
                turtles.Add(new Turtle() { IsVisible = false, IsDown = false, Shape = Shapes.Bird, Speed = Speeds.Normal });
            }

            Turtle t;

            var turtleIndex = 0;
            var color = Colors.Black;
            var speed = Speeds.Slowest;

            // 5 Corners
            color = Colors.DarkBlue;
            speed = Speeds.Slowest;
            t = turtles[turtleIndex++];
            t.Color = color;
            t.Speed = speed;
            t.Position = (xPos, 0);
            t.IsVisible = true;
            DrawStar(t, 5, 2, radius);
            t.Forward(800);

            xPos = xPos + 2 * radius + distance;

            // 7 Corners
            color = Colors.DarkCyan;
            speed = Speeds.Slow;
            t = turtles[turtleIndex++];
            t.Color = color;
            t.Speed = speed;
            t.Position = (xPos, 0);
            t.IsVisible = true;
            DrawStar(t, 7, 3, radius);
            t.Forward(800);

            xPos = xPos + 2 * radius + distance;

            // 9 Corners
            color = Colors.DarkGoldenrod;
            speed = Speeds.Normal;
            t = turtles[turtleIndex++];
            t.Color = color;
            t.Speed = speed;
            t.Position = (xPos, 0);
            t.IsVisible = true;
            DrawStar(t, 9, 4, radius);
            t.Forward(800);

            xPos = xPos + 2 * radius + distance;

            // 10 Corners
            color = Colors.DarkGreen;
            speed = Speeds.Fast;
            t = turtles[turtleIndex++];
            t.Color = color;
            t.Speed = speed;
            t.Position = (xPos, 0);
            t.IsVisible = true;
            DrawStar(t, 10, 3, radius);
            t.Forward(800);

            xPos = xPos + 2 * radius + distance;

            // 11 Corners
            color = Colors.DarkMagenta;
            speed = Speeds.Fast;
            t = turtles[turtleIndex++];
            t.Color = color;
            t.Speed = speed;
            t.Position = (xPos, 0);
            t.IsVisible = true;
            DrawStar(t, 11, 5, radius);
            t.Forward(800);

            xPos = xPos + 2 * radius + distance;

            // 13 Corners
            color = Colors.DarkOrange;
            speed = Speeds.Fast;
            t = turtles[turtleIndex++];
            t.Color = color;
            t.Speed = speed;
            t.Position = (xPos, 0);
            t.IsVisible = true;
            DrawStar(t, 13, 6, radius);
            t.Forward(800);

            xPos = xPos + 2 * radius + distance;

            // 14 Corners
            color = Colors.DarkOrchid;
            speed = Speeds.Fast;
            t = turtles[turtleIndex++];
            t.Color = color;
            t.Speed = speed;
            t.Position = (xPos, 0);
            t.IsVisible = true;
            DrawStar(t, 14, 5, radius);
            t.Forward(800);

            xPos = xPos + 2 * radius + distance;

            // 15 Corners
            color = Colors.DarkRed;
            speed = Speeds.Fast;
            t = turtles[turtleIndex++];
            t.Color = color;
            t.Speed = speed;
            t.Position = (xPos, 0);
            t.IsVisible = true;
            DrawStar(t, 15, 7, radius);
            t.Forward(800);

            xPos = xPos + 2 * radius + distance;
        }

        private static void CalculateStarParameters(double radius, int corners, int delta, out double turnAngle, out double innerAngle, out double length)
        {
            turnAngle = (double)delta * 360 / (double)corners;
            innerAngle = Math.Abs(180 - turnAngle) / 2;
            var innerAngleRadiant = innerAngle / 180 * Math.PI;
            length = 2 * radius * Math.Cos(innerAngleRadiant);
        }
    }

}
