using Woopec.Core;

namespace UsingTurtleCanvas
{
    class SampleProgram
    {
        public static void TurtleMain()
        {
            // Woopec.Examples.TurtleDemoByteDesign.Run();
            // Woopec.Examples.FeaturesDemo.Run();
            // Woopec.Examples.ParallelTurtles.Run();

            var turtle = Turtle.Seymour();

            turtle.Right(45);
            turtle.Forward(50);
            turtle.Left(90);
            turtle.Forward(100);
            turtle.Right(45);
            turtle.Forward(20);
        }
    }
}
