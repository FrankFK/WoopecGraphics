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
            Woopec.Examples.ParallelTurtles.Run();
        }
    }
}
