using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Core;

namespace Woopec.Core.Examples
{
    /// <summary>
    /// This Example shows many features of the library (in the order they were implemented)
    /// </summary>
    public static class FeaturesDemo
    {
        /// <summary>
        /// This Example shows many features of the library (in the order they were implemented)
        /// </summary>
        public static void Run()
        {
            /////////////////////////////////////////////////////////////
            // 08.03.2021: First turtle with WPF
            var firstTurtle = new Turtle() { Speed = Speeds.Slow, IsVisible = false, IsDown = true };

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
                turtles.Add(new Turtle() { IsDown = false, Speed = Speeds.Fast, IsVisible = false, });
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
            turtles[0].Speed = Speeds.Slowest; turtles[0].PenColor = Colors.DarkGreen;
            turtles[1].Speed = Speeds.Slow; turtles[1].PenColor = Colors.DarkRed;
            turtles[2].Speed = Speeds.Normal; turtles[2].PenColor = Colors.DarkOrange;
            turtles[3].Speed = Speeds.Fast; turtles[3].PenColor = Colors.DarkBlue;
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
            activeTurtle.SetPosition((0, activeTurtle.Position.Y));

            /////////////////////////////////////////////////////////////
            // 15.05.2021 TurtleMovement
            activeTurtle.Speed = Speeds.Slow;
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
            activeTurtle.Speed = Speeds.Normal;
            activeTurtle.Left(720);
            activeTurtle.Right(720 + 90);
            activeTurtle.Forward(40);
            activeTurtle.Right(90);
            activeTurtle.Forward(30);
            activeTurtle.Right(90);
            activeTurtle.Forward(20);

            activeTurtle.FillColor = Colors.DarkRed;
            activeTurtle.Speed = Speeds.Slowest;
            activeTurtle.Forward(50);
            activeTurtle.PenColor = Colors.Blue;

            /////////////////////////////////////////////////////////////
            // 22.05.2021 Speed.Fastest => No animation
            activeTurtle.IsVisible = false;
            activeTurtle.Speed = Speeds.Fastest;
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
            activeTurtle.Left(180);
            activeTurtle.Color = Colors.DarkGreen;
            activeTurtle.Shape = Shapes.Turtle;
            activeTurtle.Speed = Speeds.Slowest;
            activeTurtle.IsVisible = true;
            activeTurtle.Forward(150);

            /////////////////////////////////////////////////////////////
            // 30.05.2021 Filling (with the classic example from the python turtle documentation)
            var startPos = activeTurtle.Position;
            activeTurtle.Speed = Speeds.Fast;
            activeTurtle.FillColor = Colors.LightGreen;
            activeTurtle.PenColor = Colors.DarkGreen;
            activeTurtle.IsVisible = false;
            activeTurtle.PenDown();
            activeTurtle.BeginFill();
            while (true)
            {
                activeTurtle.Forward(200);
                activeTurtle.Left(170);
                if ((startPos - activeTurtle.Position).AbsoluteValue < 1)
                    break;
            }
            activeTurtle.EndFill();

            /////////////////////////////////////////////////////////////
            // 08.06.2021 Shape for bird (in the meantime: nuget-package, documentation)
            activeTurtle.Speed = Speeds.Slow;
            activeTurtle.Shape = Shapes.Bird;
            activeTurtle.Color = Colors.DarkGreen;
            activeTurtle.PenUp();
            activeTurtle.Heading = 45;
            activeTurtle.IsVisible = true;
            activeTurtle.Forward(300);

            /////////////////////////////////////////////////////////////
            // 19.04.2022 When debugger is attached, the WPF-renderer runs in a second process. This makes debugging of turtle code much easier

            /////////////////////////////////////////////////////////////
            // 06.06.2022 TextInput()

            var colorAnswer = activeTurtle.Screen.TextInput("Color selection", "Please input the name of a color (e.g. red):");
            if (colorAnswer != null)
                activeTurtle.Color = colorAnswer;
            activeTurtle.Forward(50);

            /////////////////////////////////////////////////////////////
            // 12.06.2022 NumInput() and DoubleInput()

            int numberOfStarships = 0;
            var countAnswer = activeTurtle.Screen.NumInput("User defined shape", "Please input the number of new shapes (between 1 and 10)", 5, 1, 10, null);
            numberOfStarships = countAnswer.GetValueOrDefault();

            /////////////////////////////////////////////////////////////
            // 18.06.2022 BeginPoly EndPoly, Shapes.Add, Shapes.Get
            var shapeName = "starship";
            AddStarshipShape(shapeName);

            if (numberOfStarships > 0)
            {
                var starships = new List<Turtle>();
                for (int starshipId = 1; starshipId <= numberOfStarships; starshipId++)
                {
                    var color = new Color(255 - starshipId * 20, 0, 0);
                    var starship = new Turtle() { IsDown = false, Shape = Shapes.Get(shapeName), Color = color, Heading = 5 * starshipId, Speed = Speeds.Slow };
                    starships.Add(starship);
                }
                foreach (var starship in starships)
                {
                    starship.Forward(1000);
                }
            }

            var bullet = new Turtle() { Shape = Shapes.Circle, IsDown = false, Color = Colors.DarkGoldenrod, Speed = Speeds.Slow };

            /////////////////////////////////////////////////////////////
            // 10.07.2022 transparent colors
            var transparentStarship = new Turtle() { IsDown = false, Shape = Shapes.Get(shapeName), Speed = Speeds.Slowest };
            transparentStarship.PenColor = colorAnswer;
            Color answerColor = colorAnswer;
            transparentStarship.FillColor = answerColor.Transparent(0.8);
            transparentStarship.Left(135);
            transparentStarship.Forward(1000);

            /////////////////////////////////////////////////////////////
            // 17.07.2022 An object can wait for the completed movement of another object
            bullet.Position = activeTurtle.Position;
            activeTurtle.WaitForCompletedMovementOf(bullet);
            activeTurtle.Speed = Speeds.Normal;
            activeTurtle.Heading = 0;
            activeTurtle.Forward(100);
            bullet.WaitForCompletedMovementOf(activeTurtle);
            bullet.Position = activeTurtle.Position;
            activeTurtle.WaitForCompletedMovementOf(bullet);
            activeTurtle.Speed = Speeds.Normal;
            activeTurtle.Forward(100);
            bullet.HideTurtle();

            /////////////////////////////////////////////////////////////
            // 23.11.2022 A Pen can be used standalone (Pen class made visible and changed code of Pen.EndFilling)
            var pen = new Pen();
            pen.Move(1);

            /////////////////////////////////////////////////////////////
            // 08.12.2022 A Figure can be used standalone (Figure class made)
            var figure = new Figure() { Shape = Shapes.Bird };
            figure.Position = (100, 100);
            figure.IsVisible = true;
            figure.Move(500);


        }

        private static void AddStarshipShape(string name)
        {
            var length = 10;

            var turtle = new Turtle() { IsDown = false, Color = Colors.DarkRed, Shape = Shapes.Classic };

            // Hexagon:
            turtle.Position = (0, length);
            turtle.Heading = -30;
            turtle.BeginPoly();
            for (int hexaEdge = 1; hexaEdge < 6; hexaEdge++)
            {
                turtle.Forward(length);
                turtle.Right(60);
            }
            var hexagon = turtle.EndPoly();

            // Triangles
            turtle.Position = (0, -1.5 * length);
            turtle.Heading = 30;
            turtle.BeginPoly();
            turtle.Forward(length); turtle.Right(120); turtle.Forward(length);
            var triangle1 = turtle.EndPoly();

            turtle.Position = (0, -1.5 * length);
            turtle.Heading = 150;
            turtle.BeginPoly();
            turtle.Forward(length); turtle.Left(120); turtle.Forward(length);
            var triangle2 = turtle.EndPoly();

            var shape = new Shape();
            shape.AddComponent(hexagon);
            shape.AddComponent(triangle1);
            shape.AddComponent(triangle2);
            Shapes.Add(name, shape);

            turtle.IsVisible = false;

        }
    }
}
