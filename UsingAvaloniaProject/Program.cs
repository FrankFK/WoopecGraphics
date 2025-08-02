using Woopec.Graphics;
using Woopec.Graphics.Avalonia;

namespace UsingAvaloniaProject
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Frontend.Initialize(args);
        }

        public static void WoopecMain()
        {
            var pen = new Pen();
            pen.Color = Colors.Black;
            pen.Speed = Speeds.Fastest;
            pen.IsDown = true;
            pen.Move(100);
            pen.Color = Colors.Red;
            pen.Rotate(90);
            pen.Move(100);
        }
    }

}
