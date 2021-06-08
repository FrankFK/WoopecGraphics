using Woopec.Core;

namespace UsingTurtleCanvas
{
    class SampleProgram
    {
        public static void TurtleMain()
        {
            // TurtleSamples.TurtleFunctionsDemo.Run();
            
            var turtle = Turtle.Seymour();
            turtle.Left(45);
            turtle.Forward(100);
            turtle.Right(45);

            var turtle2 = new Turtle() { Shape = Shapes.Bird, Speed = SpeedLevel.Slowest, Color = Colors.DarkGreen };
            turtle2.Forward(100);
            turtle2.Left(45);
            turtle2.Forward(100);
            turtle2.Right(45);

        }
    }
}
