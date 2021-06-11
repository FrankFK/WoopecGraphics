using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Woopec.Core;

namespace Woopec.Examples
{
    /// <summary>
    /// This example shows how turtles can be animated in parallel
    /// </summary>
    public static class ParallelTurtles
    {
        /// <summary>
        /// This example shows how turtles can be animated in parallel
        /// </summary>
        public static void Run()
        {
            const int count = 100;
            const int radius = 350;

            var turtles = new List<Turtle>();
            for (var index = 0; index < count; index++)
            {
                var turtle = Turtle.Seymour();
                turtle.Color = new Color(0, 2 * index, 0);
                turtle.PenUp();
                turtles.Add(turtle);
            }

            for (var index = 0; index < count; index++)
            {
                var turtle = turtles[index];
                turtle.Left(index * 360.0 / count);
                turtle.Forward(radius);
                turtle.Speed = Speeds.Fast;
                turtle.Left(180);
            }

            Thread.Sleep(10000);

            for (var index = 0; index < count; index++)
            {
                var turtle = turtles[index];
                turtle.Speed = Speeds.Normal;
                turtle.PenDown();
                turtle.Forward(radius);
                turtle.HideTurtle();
            }
        }
    }
}
