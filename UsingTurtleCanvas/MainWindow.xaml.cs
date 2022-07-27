using System;
using System.Windows;
using Woopec.Core;
using Colors = Woopec.Core.Colors;
using Shape = Woopec.Core.Shape;

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
            Woopec.Core.Examples.ColorDemo.ColorAnimation();
            return;

            /*
            Woopec.Core.Examples.Spirograph.SpiroDemo();
            return;
            Woopec.Core.Examples.Spirograph.SpiroDemo2();
            return;

            Woopec.Core.Examples.Spirograph.WithWheels(5, 3, 0.75, 90, (0, 0), Colors.DarkRed);
            return;
            Woopec.Core.Examples.DrawWoopecName.Run();
            return;
            Woopec.Core.Examples.Stars.StarDemo();
            return;



            */


#pragma warning disable CS0162 // Unreachable code detected
            Woopec.Core.Examples.FeaturesDemo.Run();
#pragma warning restore CS0162 // Unreachable code detected
            return;

            Woopec.Core.Examples.TurtleDemoByteDesign.Run();
            return;

            Woopec.Core.Examples.DrawWoopecName.Run();
            return;

            var turtle1 = Turtle.Seymour();
            var turtle2 = Turtle.Seymour();
            turtle1.Left(90);
            turtle1.Forward(100);
            turtle2.Right(90);
            turtle2.WaitForCompletedMovementOf(turtle1);
            turtle2.Forward(100);
            turtle1.WaitForCompletedMovementOf(turtle2);
            turtle1.Left(30);
            turtle1.Forward(100);
            return;


            // Woopec.Core.Examples.FeaturesDemo.Run();
            // return;
            /*
            var seymour = Turtle.Seymour();
            seymour.PenUp();
            seymour.Position = (-100, 0);
            seymour.Speed = Speeds.Fastest;
            seymour.HideTurtle();
            seymour.PenDown();
            seymour.Forward(200);
            Woopec.Core.Examples.Spirograph.Hypo(5, 3, 0.8, 200);
            return;
            seymour.PenUp();
            seymour.Forward(170);
            seymour.PenDown();
            */
            // Woopec.Core.Examples.Spirograph.DrawStar(seymour, 8, 3, 100);

            // seymour.HideTurtle();

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


