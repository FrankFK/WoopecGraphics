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
    /// These examples draw colors
    /// </summary>
    public static class ColorDemo
    {
        /// <summary>
        /// Displays the colors for a hue value one after the other. 
        /// An array with different combinations of value (y-axis) 
        /// and saturation (x-axis) is displayed for each hue value.
        /// </summary>
        public static void ColorAnimation()
        {
            var turtle = new Turtle() { IsVisible = false, IsDown = false, Shape = Shapes.Triangle, Speed = Speeds.Fastest };
            var radius = 200.0;
            var ringSize = 20.0;
            var colorRingSegments = 90;

            var center = new Vec2D(0, 0);

            turtle.Position = center;

            // Draw a color ring with saturation and vue = 1.0
            ColorRing(turtle, colorRingSegments, radius, ringSize, 1.0, 1.0);

            var turtleShapeSize = 20.0;
            var rectRadius = (radius - ringSize) / Math.Sqrt(2) - turtleShapeSize;
            var colorRectangleSegments = 10;

            // Create a rectangleArray for the table with different saturations and vues
            var turtleArry = ColorRectangleArray(center, rectRadius, colorRectangleSegments);

            var huePointerRadius = radius - ringSize - turtleShapeSize;
            for (var hueIndex = 0; hueIndex < colorRingSegments; hueIndex++)
            {
                var hue = 360.0 / colorRingSegments * hueIndex;
                var huePointerAngle = hue + (hueIndex + 0.5) * 360.0 / colorRingSegments;
                var huePointerAngleRadiant = Math.PI * huePointerAngle / 360.0;
                Vec2D huePointerPosition = (Math.Cos(huePointerAngleRadiant) * huePointerRadius, Math.Sin(huePointerAngleRadiant) * huePointerRadius);
                turtle.IsVisible = false;
                turtle.Heading = center.HeadingTo(huePointerPosition);
                turtle.Color = Color.FromHSV(hue, 1.0, 1.0);
                turtle.IsVisible = true;
                turtle.Position = huePointerPosition;
                for (var row = colorRectangleSegments - 1; row >= 0; row--)
                {
                    for (var col = colorRectangleSegments - 1; col >= 0; col--)
                    {
                        turtleArry[row, col].WaitForCompletedMovementOf(turtle);
                    }
                }
                FillColorRectangles(turtleArry, colorRectangleSegments, hue);

                // If the code fills the buffer to fast, we come into problems:
                Thread.Sleep(200);
            }
        }

        /// <summary>
        /// An array with different combinations of value (y-axis) 
        /// and saturation (x-axis) is displayed for the given hue value.
        /// </summary>
        /// <param name="center">center of the array</param>
        /// <param name="arrayRadius">radius of the array</param>
        /// <param name="segments">number of segments per axis</param>
        /// <param name="hue"></param>
        public static void ColorArrayForHue(Vec2D center, double arrayRadius, int segments, double hue)
        {
            var turtleArry = ColorRectangleArray(center, arrayRadius, segments);
            FillColorRectangles(turtleArry, segments, hue);
        }

        /// <summary>
        /// Draw the colors for a given value in the Hue-Saturation-Value color space.
        /// The hue of the colors determines the angle at the circle.
        /// The saturation determines the distance to the center of the disc.
        /// </summary>
        /// <param name="center">center of the disc</param>
        /// <param name="radius">radius of the disc</param>
        /// <param name="colors">the number of the colors</param>
        /// <param name="segments">number of segments from outer to inner</param>
        /// <param name="value">all colors should be printed with this value</param>
        public static void ColorDisc(Vec2D center, double radius, int colors, int segments,
            double value)
        {
            var turtle = new Turtle() { IsVisible = false, IsDown = false, Shape = Shapes.Bird, Speed = Speeds.Fastest };
            ColorDisk(center, radius, colors, segments, value, turtle);

        }


        /// <summary>
        /// Draw a ring of colors for a given value in the Hue-Saturation-Value color space.
        /// The hue of the colors determines the angle at the circle.
        /// </summary>
        /// <param name="turtle">Draw it with this turtle. Actual position of turtle determines the center of the disc</param>
        /// <param name="outerRadius">Outer radius of the disc</param>
        /// <param name="thickness">Thickness of the ring</param>
        /// <param name="colors">the number of the colors</param>
        /// <param name="saturation">All colors should be printed with this saturation</param>
        /// <param name="value">All colors should be printed with this value</param>
        public static void ColorRing(Turtle turtle, int colors, double outerRadius, double thickness, double saturation, double value)
        {
            var outerPoly = Stars.StarPoly(colors, 1, outerRadius).
                Select(point => point + turtle.Position).ToList();
            var innerPoly = Stars.StarPoly(colors, 1, outerRadius - thickness).
                Select(point => point + turtle.Position).ToList();

            for (var i = 0; i < outerPoly.Count - 1; i++)
            {
                var hue = i * 360.0 / colors;
                turtle.FillColor = Color.FromHSV(hue, saturation, value);
                turtle.PenColor = turtle.FillColor;

                turtle.Position = outerPoly[i];
                turtle.BeginFill();
                turtle.Position = outerPoly[i + 1];
                turtle.Position = innerPoly[i + 1];
                turtle.Position = innerPoly[i];
                turtle.EndFill();
            }
        }

        private static void ColorDisk(Vec2D center, double radius, int corners, int segments, double value, Turtle turtle)
        {
            for (var s = 0; s < segments; s++)
            {
                turtle.Position = center;
                ColorRing(turtle,
                    corners, radius - s * (radius / segments), radius / segments,
                    1.0 - s * (1.0) / segments,
                    value
                );
                Thread.Sleep(500);
            }
        }

        private static Turtle[,] ColorRectangleArray(Vec2D center, double radius, int segments)
        {
            var turtleArray = new Turtle[segments, segments];
            var rectSize = 2 * radius / segments;
            var xy = rectSize / 2.0;
            var rectShape = new Shape(new List<Vec2D>() { (-xy, -xy), (-xy, xy), (xy, xy), (xy, -xy) });
            for (var row = segments - 1; row >= 0; row--)
            {
                for (var col = segments - 1; col >= 0; col--)
                {
                    var t = new Turtle() { IsVisible = false, IsDown = false, Shape = rectShape };
                    t.Position = center;
                    t.Position += (-radius + rectSize / 2, -radius + rectSize / 2);
                    t.Position += (col * rectSize, row * rectSize);
                    turtleArray[col, row] = t;
                }

            }
            return turtleArray;
        }

        private static void FillColorRectangles(Turtle[,] turtleArray, int segments, double hue)
        {
            for (var row = segments - 1; row >= 0; row--)
            {
                for (var col = segments - 1; col >= 0; col--)
                {
                    var t = turtleArray[col, row];
                    t.Color = Color.FromHSV(hue, 1.0 * col / (segments - 1), 1.0 * row / (segments - 1));
                    t.IsVisible = true;
                }

            }
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
