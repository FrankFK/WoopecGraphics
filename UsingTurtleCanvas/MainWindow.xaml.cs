using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Woopec.Core;
using Woopec.Wpf;

namespace UsingTurtleCanvas
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

        public static void WoopecMain()
        {
            //
            var seymour = Turtle.Seymour();
            seymour.PenUp();
            seymour.Position = (-100, 0);
            seymour.Speed = Speeds.Normal;
            Woopec.Core.Examples.Spirograph.DrawStar(seymour, 8, 1, 100);
            seymour.PenUp();
            seymour.Forward(170);
            seymour.PenDown();
            //
            Woopec.Core.Examples.Spirograph.DrawStar(seymour, 8, 3, 100);

            seymour.HideTurtle();

            // Woopec.Core.Examples.Spirograph.StarDemo();
            // Woopec.Core.Examples.Spirograph.Article(5, 2, 200);
            // Woopec.Core.Examples.Spirograph.Hypo(19, 13, 0.5, 200);
            // Woopec.Examples.FeaturesDemo.Run();

            /*


            turtle.Right(45);
            turtle.Left(90);
            turtle.Forward(100);
            turtle.Right(45);
            turtle.Forward(20);
            */
        }

    }
}
