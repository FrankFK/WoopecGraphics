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
            // Woopec.Examples.FeaturesDemo.Run();

            var turtle = Turtle.Seymour();
            turtle.Forward(50);
            var answer = turtle.Screen.TextInput("Color selection", "Please input the name of a color (e.g. red):");

            if (answer != null)
                turtle.Color = answer;

            turtle.Right(45);
            turtle.Forward(50);
            turtle.Left(90);
            turtle.Forward(100);
            turtle.Right(45);
            turtle.Forward(20);

            /*
            var turtle = Turtle.Seymour();

            turtle.Right(45);
            turtle.Forward(50);
            turtle.Left(90);
            turtle.Forward(100);
            turtle.Right(45);
            turtle.Forward(20);
            */
        }

    }
}
