using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurtleCore;

namespace TurtleSamples
{
    /// <summary>
    /// This class uses many features of Turtle (in the order I have implemented them)
    /// </summary>
    public static class TurtleFunctionsDemo
    {
        public static void Run()
        {
            /////////////////////////////////////////////////////////////
            // 08.03.2021: First turtle with WPF
            var firstTurtle = new Turtle() { Speed = SpeedLevel.Slow, IsVisible = false, };

            firstTurtle.Right(45);
            firstTurtle.Forward(50);
            firstTurtle.Left(90);
            firstTurtle.Forward(100);
            firstTurtle.Right(45);
            firstTurtle.Forward(20);

            /////////////////////////////////////////////////////////////
            // 1.5.2021: First version of animation handling
            // - Animations of the same turtle follows one after the other
            // - Animations of different turtles are printed in parallel
            var turtles = new List<Turtle>();
            for (var counter = 0; counter < 10; counter++)
            {
                turtles.Add(new Turtle() { IsDown = false, Speed = SpeedLevel.Fast, IsVisible = false, });
            }

            // Move all turtles to the same position as firstTurtle
            foreach (var turtle in turtles)
            {
                turtle.Right(45);
                turtle.Forward(50);
                turtle.Left(90);
                turtle.Forward(100);
                turtle.Right(45);
                turtle.Forward(20);
            }

            foreach (var t in turtles)
                t.PenDown();
            // The turtles are moving in parallel
            for (var j = 0; j < 40; j++)
            {
                for (var index = 0; index < turtles.Count; index++)
                {
                    var turtle2 = turtles[index];
                    turtle2.Left(1 + index * 0.2);
                    turtle2.Forward(5);

                    if (j == 5) turtle2.PenColor = Colors.Green;    // 04.05.2021: Predefined Colors
                }
            }

            //////////////////////////////////////////////////////////////
            // 5.5.2021: Different Speeds
            turtles[0].Speed = SpeedLevel.Slowest; turtles[0].PenColor = Colors.DarkGreen;
            turtles[1].Speed = SpeedLevel.Slow; turtles[1].PenColor = Colors.DarkRed;
            turtles[2].Speed = SpeedLevel.Normal; turtles[2].PenColor = Colors.DarkOrange;
            turtles[3].Speed = SpeedLevel.Fast; turtles[3].PenColor = Colors.DarkBlue;
            for (var speedIndex = 0; speedIndex <= 3; speedIndex++)
            {
                var speedTurtle = turtles[speedIndex];
                speedTurtle.Right(135 + speedIndex * 5);
                speedTurtle.Forward(200);
                speedTurtle.Right(90);
                speedTurtle.Forward(200);
            }

            /////////////////////////////////////////////////////////////
            // 10.05.2021 Penup and Pendown
            var activeTurtle = turtles[0];
            activeTurtle.PenUp();
            activeTurtle.Forward(5);
            activeTurtle.PenDown();
            activeTurtle.Forward(10);

            /////////////////////////////////////////////////////////////
            // 10.05.2021 Position
            activeTurtle.SetPosition((0, activeTurtle.Position.YCor));

            /////////////////////////////////////////////////////////////
            // 15.05.2021 TurtleMovement
            activeTurtle.Speed = SpeedLevel.Slow;
            activeTurtle.Heading = 180;
            activeTurtle.ShowTurtle();
            activeTurtle.Forward(50);
            activeTurtle.SetPosition(activeTurtle.Position + new Vec2D(0, 50));
            activeTurtle.Forward(50);
            activeTurtle.HideTurtle();
            activeTurtle.Forward(50);
            activeTurtle.ShowTurtle();
            activeTurtle.Forward(50);

            /////////////////////////////////////////////////////////////
            // 15.05.2021 (late in the evening) TurtleRotation
            activeTurtle.Speed = SpeedLevel.Normal;
            activeTurtle.Left(720);
            activeTurtle.Right(720 + 90);
            activeTurtle.Forward(40);
            activeTurtle.Right(90);
            activeTurtle.Forward(30);
            activeTurtle.Right(90);
            activeTurtle.Forward(20);

            activeTurtle.FillColor = Colors.DarkRed;
            activeTurtle.Speed = SpeedLevel.Slowest;
            activeTurtle.Forward(50);
            activeTurtle.PenColor = Colors.Blue;

            /////////////////////////////////////////////////////////////
            // 22.05.2021 Speed.Fastest => No animation
            activeTurtle.IsVisible = false;
            activeTurtle.Speed = SpeedLevel.Fastest;
            foreach (var i in Enumerable.Range(0, 10))
            {
                activeTurtle.PenUp();
                activeTurtle.Left(72);
                activeTurtle.Forward(5);
                activeTurtle.Right(72);
                activeTurtle.PenDown();
                foreach (var _ in Enumerable.Range(0, 5))
                {
                    activeTurtle.Forward(60 - 5 * i);
                    activeTurtle.Left(72);
                }
            }

            /////////////////////////////////////////////////////////////
            // 29.05.2021 shape can be changed
            activeTurtle.PenUp();
            activeTurtle.Color = Colors.DarkGreen;
            activeTurtle.Shape = ShapeNames.Turtle;
            activeTurtle.Speed = SpeedLevel.Slowest;
            activeTurtle.IsVisible = true;
            activeTurtle.Forward(100);
        }
    }
}
