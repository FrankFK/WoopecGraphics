using Woopec.Core;

namespace UsingTurtleCanvas
{
    class SampleProgram
    {
        public static void TurtleMain()
        {
            var turtle = new Turtle();
            turtle.Color = Colors.DarkGreen;
            turtle.Shape = Shapes.Turtle;
            turtle.Speed = SpeedLevel.Slowest;
            turtle.Left(45);
            turtle.Forward(100);
            turtle.Right(45);
        }
    }
}
