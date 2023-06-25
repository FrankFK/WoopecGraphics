using System;
using System.Numerics;
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
            // InitializeComponent();
        }

        public static void WoopecMain()
        {
            Woopec.Core.Examples.FeaturesDemo.Run();

            return;
        }

    }
}


