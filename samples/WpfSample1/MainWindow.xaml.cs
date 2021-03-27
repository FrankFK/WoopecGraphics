using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace WpfSample1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string CommandText { get; set; }

        public ConsoleContent Output { get; set; }

        public string OutputText { get; set; }

        private const string FirstCommandTextContent = "// Type your command here. Try with typing 'ellipses'\n";

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            CommandText = FirstCommandTextContent;
            Output = new ConsoleContent() { Text = "Output of your program: \n" };
        }

        /// <summary>
        /// On each change of CommandText: Check if command is ended with return and call the command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CommandTextChanged(object sender, TextChangedEventArgs args)
        {
            // Simple Command Interpreter
            if (CommandText.EndsWith("\n") && CommandText != FirstCommandTextContent)
            {
                DrawingExample();
            }
        }

        private void DrawingExample()
        {
            // More or less copied from https://docs.microsoft.com/de-de/dotnet/desktop/wpf/graphics-multimedia/how-to-create-a-geometrydrawing?view=netframeworkdesktop-4.8
            //
            //
            // Create the Geometry to draw.
            //
            var ellipses = new GeometryGroup
            {
                Children = { new EllipseGeometry(new Point(50, 50), 45, 20), new EllipseGeometry(new Point(50, 50), 20, 45) }
            };

            //
            // Create a GeometryDrawing.
            //
            var aGeometryDrawing = new GeometryDrawing
            {
                Geometry = ellipses,

                // Paint the drawing with a gradient.
                Brush =
                new LinearGradientBrush(
                    Colors.Blue,
                    Color.FromRgb(204, 204, 255),
                    new Point(0, 0),
                    new Point(1, 1)),

                // Outline the drawing with a solid color.
                Pen = new Pen(Brushes.Black, 10)
            };

            //
            // Use a DrawingImage and an Image control
            // to display the drawing.
            //
            var geometryImage = new DrawingImage(aGeometryDrawing);

            // Freeze the DrawingImage for performance benefits.
            geometryImage.Freeze();


            var anImage = Image1;
            anImage.Source = geometryImage;

            Output.Text += " Ellipse printed.\n";

        }
    }
}
