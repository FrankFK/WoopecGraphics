﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Graphics;

namespace Woopec.Graphics.Examples
{
    /// <summary>
    /// This example draws the name "Woopec" with Woopec.Core
    /// </summary>
    public static class DrawWoopecName
    {
        /// <summary>
        /// This Example draws the name "Woopec" with Woopec.Core
        /// </summary>
        public static void RunOldVersion()
        {
            var seymour = Turtle.Seymour();

            int factor = 5;
            var bottomRight = DrawW(seymour, factor);
            seymour.Right(45);
            seymour.PenUp();

            var woopec1 = new Turtle() { Color = Colors.Red, Shape = Shapes.Bird, IsVisible = false, IsDown = false, Speed = Speeds.Normal };
            var woopec2 = new Turtle() { Color = Colors.Red, Shape = Shapes.Bird, IsVisible = false, IsDown = false, Speed = Speeds.Normal };
            var woopec3 = new Turtle() { Color = Colors.SteelBlue, Shape = Shapes.Bird, IsVisible = false, IsDown = false, Speed = Speeds.Normal };
            var woopec4 = new Turtle() { Color = Colors.BlueViolet, Shape = Shapes.Bird, IsVisible = false, IsDown = false, Speed = Speeds.Normal };
            var woopec5 = new Turtle() { Color = Colors.OrangeRed, Shape = Shapes.Bird, IsVisible = false, IsDown = false, Speed = Speeds.Normal };

            int xPos = 0;
            woopec1.Position = bottomRight;
            woopec1.ShowTurtle();
            woopec1.PenDown();
            DrawO(woopec1, factor / 2);

            xPos += 30;
            woopec2.Position = bottomRight + (xPos, 0);
            woopec2.ShowTurtle();
            woopec2.PenDown();
            DrawO(woopec2, factor / 2);

            xPos += 25;
            woopec3.Position = bottomRight + (xPos, 0);
            woopec3.ShowTurtle();
            woopec3.PenDown();
            DrawP(woopec3, factor / 2);

            xPos += 30;
            woopec4.Position = bottomRight + (xPos, 0);
            woopec4.ShowTurtle();
            woopec4.PenDown();
            DrawE(woopec4, factor / 2);

            xPos += 35;
            woopec5.Position = bottomRight + (xPos, 0);
            woopec5.ShowTurtle();
            woopec5.PenDown();
            DrawC(woopec5, factor / 2);

            var seymour2 = Turtle.Seymour();
            seymour2.HideTurtle();
            seymour2.PenUp();
            seymour2.Position = seymour.Position;
            seymour2.Heading = seymour.Heading;
            seymour2.ShowTurtle();
            seymour2.Forward(160);
            seymour.HideTurtle();

            var woopecList = new List<Turtle>() { woopec1, woopec2, woopec3, woopec4, woopec5 };
            foreach (var w in woopecList) { w.PenUp(); w.Speed = Speeds.Slowest; w.Left(360); }
            foreach (var w in woopecList) { w.Speed = Speeds.Normal; w.Forward(400); }

            seymour2.Right(90);
            seymour2.Forward(15);
            seymour2.Left(90);
            seymour2.Speed = Speeds.Normal;
            seymour2.PenDown();
            var startPos = seymour2.Position;
            while (true)
            {
                seymour2.Forward(50);
                seymour2.Left(170);
                if ((startPos - seymour2.Position).AbsoluteValue < 1)
                    break;
            }

            seymour2.PenUp();
            seymour2.Forward(70);
            var startPos2 = seymour2.Position;
            seymour2.Speed = Speeds.Fastest;
            seymour2.PenColor = Colors.DarkOrange;
            seymour2.PenDown();
            while (true)
            {
                seymour2.Forward(40);
                seymour2.Left(170);
                if ((startPos2 - seymour2.Position).AbsoluteValue < 1)
                    break;
            }
            seymour2.PenUp();
            seymour2.PenColor = seymour.PenColor;
            seymour2.Forward(40);

            seymour2.Speed = Speeds.Normal;
            seymour2.Right(90);
            seymour2.Forward(40);
            seymour2.Right(90);
            seymour2.Forward(seymour2.Position.X);
            seymour2.Right(90);
            seymour2.Forward(-seymour2.Position.Y);
            seymour2.Right(90);
        }

        /// <summary>
        /// This Example draws the name "Woopec" with Woopec.Core
        /// </summary>
        public static void Run()
        {
            var seymour = Turtle.Seymour();

            int factor = 5;
            var bottomRigth = DrawW(seymour, factor);
            seymour.Right(45);
            seymour.PenUp();

            var woopec1 = new Turtle() { Color = Colors.Red, Shape = Shapes.Bird, IsVisible = false, IsDown = false, Speed = Speeds.Normal };
            var woopec2 = new Turtle() { Color = Colors.Red, Shape = Shapes.Bird, IsVisible = false, IsDown = false, Speed = Speeds.Normal };
            var woopec3 = new Turtle() { Color = Colors.SteelBlue, Shape = Shapes.Bird, IsVisible = false, IsDown = false, Speed = Speeds.Normal };
            var woopec4 = new Turtle() { Color = Colors.BlueViolet, Shape = Shapes.Bird, IsVisible = false, IsDown = false, Speed = Speeds.Normal };
            var woopec5 = new Turtle() { Color = Colors.OrangeRed, Shape = Shapes.Bird, IsVisible = false, IsDown = false, Speed = Speeds.Normal };

            int xPos = 0;
            woopec1.Position = bottomRigth;
            woopec1.ShowTurtle();
            woopec1.PenDown();
            DrawO(woopec1, factor / 2);

            xPos += 30;
            woopec2.Position = bottomRigth + (xPos, 0);
            woopec2.ShowTurtle();
            woopec2.PenDown();
            DrawO(woopec2, factor / 2);

            xPos += 25;
            woopec3.Position = bottomRigth + (xPos, 0);
            woopec3.ShowTurtle();
            woopec3.PenDown();
            DrawP(woopec3, factor / 2);

            xPos += 30;
            woopec4.Position = bottomRigth + (xPos, 0);
            woopec4.ShowTurtle();
            woopec4.PenDown();
            DrawE(woopec4, factor / 2);

            xPos += 35;
            woopec5.Position = bottomRigth + (xPos, 0);
            woopec5.ShowTurtle();
            woopec5.PenDown();
            DrawC(woopec5, factor / 2);

            seymour.WaitForCompletedMovementOf(woopec4);    // woopec4 needs the most time
            seymour.Forward(160);

            var woopecList = new List<Turtle>() { woopec1, woopec2, woopec3, woopec4, woopec5 };
            foreach (var w in woopecList) { w.PenUp(); w.Speed = Speeds.Slowest; w.Left(360); }

            seymour.Right(90);
            seymour.Forward(15);
            seymour.Left(90);
            seymour.Forward(10);
            seymour.Speed = Speeds.Normal;
            Stars.DrawStar(seymour, 17, 8, 23);

            // Seymours position is at the first (rightmost) corner of the star
            seymour.Heading = 0;
            seymour.HideTurtle();

            var spiroCenter = seymour.Position + (30, 0);

            var spiroPoly = Spirograph.SpiroPoly(17, 8, 0.8, 25).Select(point => point + spiroCenter).ToList();
            var penColorBackup = seymour.PenColor;
            seymour.PenColor = Colors.DarkRed;

            for (var pointIndex = 0; pointIndex < spiroPoly.Count; pointIndex++)
            {
                seymour.Position = spiroPoly[pointIndex];
                if (pointIndex == 0)
                {
                    seymour.PenDown();
                    seymour.Speed = Speeds.Fastest;
                }
            }
            seymour.PenUp();
            seymour.PenColor = penColorBackup;
            seymour.ShowTurtle();
            seymour.Position = spiroPoly[^1];

            foreach (var w in woopecList) { w.Speed = (w != woopec5) ? Speeds.Normal : Speeds.Slowest; w.WaitForCompletedMovementOf(seymour); w.Forward(400); }

            seymour.Speed = Speeds.Normal;
            seymour.Right(90);
            seymour.Forward(20);
            seymour.Right(90);
            seymour.Forward(seymour.Position.X);
            seymour.Right(90);
            seymour.Forward(-seymour.Position.Y);
            seymour.Right(90);
        }

        // Draw a "W" and return bottom-right point
        private static Vec2D DrawW(Turtle t, int factor)
        {
            t.Right(45);
            t.Forward(10 * factor);
            t.Left(90);
            t.Forward(5 * factor);
            t.Right(90);
            t.Forward(5 * factor);
            double bottom = t.Position.Y;
            t.Left(90);
            t.Forward(10 * factor);
            double right = t.Position.X;
            return (right, bottom);
        }

        private static void DrawO(Turtle t, int factor)
        {
            const int edges = 8;
            for (int i = 0; i < edges; i++)
            {
                t.Forward(5 * factor);
                t.Left(360 / edges);
            }
            t.Forward(5 * factor);
        }

        private static void DrawP(Turtle t, int factor)
        {
            const int edges = 8;
            for (int i = 0; i < edges / 2; i++)
            {
                t.Forward(5 * factor);
                t.Left(360 / edges);
            }
            t.Forward(5 * factor);
            t.Left(90);
            t.Forward(5 * factor * 4);
            t.Left(90);
        }
        private static void DrawE(Turtle t, int factor)
        {
            const int edges = 8;
            for (int i = 0; i < edges; i++)
            {
                if (i == 2)
                {
                    var halfstep = 5.0 * factor / 2.0;
                    t.PenUp();
                    t.Forward(halfstep);
                    t.Left(90);
                    t.PenDown();
                    t.Forward(8 * factor);
                    t.PenUp();
                    t.Right(180);
                    t.Forward(8 * factor);
                    t.PenDown();
                    t.Left(90);
                    t.Forward(halfstep);
                    t.Left(360 / edges);
                }
                else
                {
                    t.Forward(5 * factor);
                    t.Left(360 / edges);
                }
            }
            t.Forward(5 * factor);
        }

        private static void DrawC(Turtle t, int factor)
        {
            const int edges = 8;
            for (int i = 0; i < edges; i++)
            {
                if (i == 2)
                {
                    t.PenUp();
                }
                t.Forward(5 * factor);
                t.Left(360 / edges);
                if (i == 2)
                {
                    t.PenDown();
                }
            }
            t.Forward(5 * factor);
        }
    }

}
