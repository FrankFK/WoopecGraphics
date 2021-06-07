using Woopec.Core;

namespace UsingTurtleCanvas
{
    class SampleProgram
    {
        public static void TurtleMain()
        {
            var turtle = Turtle.Seymour();
            turtle.Left(45);
            turtle.Forward(100);
            turtle.Right(45);
        }
    }
}
