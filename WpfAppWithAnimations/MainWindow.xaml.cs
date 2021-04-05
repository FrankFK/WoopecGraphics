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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppWithAnimations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Storyboard myStoryboard;

        public MainWindow()
        {
            InitializeComponent();
            PrintLines();
            // Animation1Initialize();
        }

        private void PrintLines()
        {
            var canvas = new Canvas() { Width = 300, Height = 1000 };
            this.Content = canvas;

            var image = new Image();
            image.Source = new BitmapImage(new Uri("C:/Users/Silver/Source/repos/simple-graphics-for-csharp-beginners/WpfAppWithAnimations/Bitmap1.bmp"));
            image.Name = "TurtleImage";
            image.RenderTransform = new RotateTransform(45);
            canvas.Children.Add(image);
            Canvas.SetLeft(image, 70);
            Canvas.SetTop(image, 10);

            double newY = 100;
            double newX = 200;
            var turtleAnimation = new DoubleAnimation();
            var top = Canvas.GetTop(image);
            var left = Canvas.GetLeft(image);
            var trans = new TranslateTransform();
            image.RenderTransform = trans;
            DoubleAnimation anim1 = new DoubleAnimation(top, newY - top, TimeSpan.FromSeconds(5));
            anim1.AutoReverse = true;
            anim1.RepeatBehavior = RepeatBehavior.Forever;
            var anim2 = new DoubleAnimation(left, newX - left, TimeSpan.FromSeconds(5));
            anim2.AutoReverse = true;
            anim2.RepeatBehavior = RepeatBehavior.Forever;
            trans.BeginAnimation(TranslateTransform.XProperty, anim1);
            trans.BeginAnimation(TranslateTransform.YProperty, anim2);

            for (int idx = 0; idx < 50; idx++)
            {
                var line = new Line
                {
                    Stroke = System.Windows.Media.Brushes.LightSteelBlue,
                    X1 = 10,
                    Y1 = 10 + 8 * idx,
                    X2 = 250,
                    Y2 = 10 + 8 * idx,
                    // HorizontalAlignment = HorizontalAlignment.Left,
                    // VerticalAlignment = VerticalAlignment.Center,
                    StrokeThickness = 1
                };

                // Animate the Line length (vergleiche https://docs.microsoft.com/de-de/dotnet/desktop/wpf/graphics-multimedia/how-to-animate-a-property-without-using-a-storyboard?view=netframeworkdesktop-4.8)
                var myDoubleAnimation = new DoubleAnimation();
                myDoubleAnimation.From = 10;
                myDoubleAnimation.To = 250;
                myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(5));
                myDoubleAnimation.AutoReverse = true;
                myDoubleAnimation.RepeatBehavior = RepeatBehavior.Forever;

                line.BeginAnimation(Line.X2Property, myDoubleAnimation);


                canvas.Children.Add(line);

            }

            // Achtung: Clocks verbrauchen Performance, auch wenn sie zu Ende sind. Daher muss man sie am besten wieder entfernen
            // Siehe Using the Compose HandoffBehavior Consumes System Resources
            // in https://docs.microsoft.com/de-de/dotnet/desktop/wpf/graphics-multimedia/animation-tips-and-tricks?view=netframeworkdesktop-4.8
        }

        /// <summary>
        /// Siehe https://docs.microsoft.com/de-de/dotnet/desktop/wpf/graphics-multimedia/animation-overview?view=netframeworkdesktop-4.8
        /// </summary>
        private void Animation1Initialize()
        {
            StackPanel myPanel = new StackPanel();
            myPanel.Margin = new Thickness(10);

            Rectangle myRectangle = new Rectangle();
            myRectangle.Name = "myRectangle";
            this.RegisterName(myRectangle.Name, myRectangle);
            myRectangle.Width = 100;
            myRectangle.Height = 100;
            myRectangle.Fill = Brushes.Blue;

            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 1.0;
            myDoubleAnimation.To = 0.0;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(5));
            myDoubleAnimation.AutoReverse = true;
            myDoubleAnimation.RepeatBehavior = RepeatBehavior.Forever;

            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(myDoubleAnimation);
            Storyboard.SetTargetName(myDoubleAnimation, myRectangle.Name);
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Rectangle.OpacityProperty));

            // Use the Loaded event to start the Storyboard.
            myRectangle.Loaded += new RoutedEventHandler(myAnimation1EventHandler);
            myPanel.Children.Add(myRectangle);
            this.Content = myPanel;
        }

        private void myAnimation1EventHandler(object sender, RoutedEventArgs e)
        {
            myStoryboard.Begin(this);
        }
    }
}
