using Woopec.Core;

namespace UsingTurtleCanvas
{
    class SampleProgram
    {
        public static void TurtleMain()
        {
            var turtle = Turtle.Seymour();

            turtle.Right(45);
            turtle.Forward(50);
            turtle.Left(90);
            turtle.Forward(100);
            turtle.Right(45);
            turtle.Forward(20);
        }

            // Woopec.Examples.TurtleDemoByteDesign.Run();
            // Woopec.Examples.FeaturesDemo.Run();
            // Woopec.Examples.ParallelTurtles.Run();

            /*
            var cynthia = new Turtle();
            cynthia.Speed = Speeds.Slowest;

            var wally = new Turtle();
            wally.Speed = Speeds.Slowest;

            cynthia.Left(90);
            cynthia.Forward(200);

            wally.Right(90);
            wally.Forward(200);
            */
        }
    }
}
