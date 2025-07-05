using System;
using System.Numerics;
using System.Windows;
using Woopec.Graphics;
using Colors = Woopec.Graphics.Colors;
using Shape = Woopec.Graphics.Shape;

namespace UsingTurtleCanvas
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            // InitializeComponent();
        }

        public static void WoopecMain()
        {
            Woopec.Graphics.Examples.FeaturesDemo.Run();

            return;
        }

    }
}


