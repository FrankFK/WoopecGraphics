using System.Windows;
using Woopec.Core;

namespace UsingWoopec
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static void TurtleMain()
        {
            Woopec.Examples.FeaturesDemo.Run();


            var whoopec = new Turtle() { Shape = Shapes.Bird, FillColor = Colors.DarkBlue, PenColor = Colors.LightBlue, Speed = Speeds.Fastest, IsVisible = false };

            whoopec.BeginFill();
            do
            {
                whoopec.Forward(200);
                whoopec.Right(170);

            } while (whoopec.Position.AbsoluteValue > 1);
            whoopec.EndFill();
            whoopec.PenUp();

            whoopec.Speed = Speeds.Slowest;
            whoopec.IsVisible = true;
            whoopec.Heading = 30;
            whoopec.Forward(200);
        }
    }
}
