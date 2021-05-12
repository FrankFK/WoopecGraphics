// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        // private readonly Storyboard _myStoryboard;

        private int _counter;
        private Canvas _canvas;
        private Image _image;
        private Path _sampleShape;
        private bool _turtleAnimationIsFinished;
        private bool _lineAnimationIsFinished;

        private RotateTransform _rotateTransform;
        private TransformGroup _turtleTransforms;
        private TranslateTransform _turtleTranslate;
        private double _turtleX2;

        private Line _currentLine;

        public MainWindow()
        {
            InitializeComponent();
            StartPrintingLines();
            // Animation1Initialize();
        }

        private void StartPrintingLines()
        {
            _counter = 0;
            _canvas = new Canvas() { Width = 500, Height = 1000 };
            this.Content = _canvas;

            _image = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/Silver/Source/repos/simple-graphics-for-csharp-beginners/WpfAppWithAnimations/Bitmap1.bmp")),
                Name = "TurtleImage"
            };
            _canvas.Children.Add(_image);

            // Compare: https://social.msdn.microsoft.com/Forums/vstudio/en-US/9af33f36-b34e-44f8-874a-73984fba0405/how-to-draw-this-path-in-c-code?forum=wpf
            _sampleShape = new Path();
            _sampleShape.Fill = new SolidColorBrush(Colors.Yellow);
            _sampleShape.Stroke = new SolidColorBrush(Colors.Red);
            _sampleShape.Name = "Classic";

            int vergroeesern = 5;

            // Turtle-Koordinaten: ((0,0),(-5,-9),(0,-7),(5,-9)), Spitze zeigt nach oben
            // WPF-Koordinaten (y-Achse geht in die entgegengesetzte Richtung): ((0,0),(-5,9),(0,7),(5,9)), Spitze zeigt nach oben
            var polygonOne = new PathFigure();
            polygonOne.StartPoint = new Point(0, 0);   // ((0,0),(-5,-9),(0,-7),(5,-9))
            polygonOne.IsClosed = true;
            polygonOne.Segments.Add(new LineSegment() { Point = new Point(-5 * vergroeesern, 9 * vergroeesern) });
            polygonOne.Segments.Add(new LineSegment() { Point = new Point(0, 7 * vergroeesern) });
            polygonOne.Segments.Add(new LineSegment() { Point = new Point(5 * vergroeesern, 9 * vergroeesern) });

            var polygonTwo = new PathFigure();
            var point2_1 = new Point(0, 7 * vergroeesern);
            var point2_2 = new Point(-5 * vergroeesern, 16 * vergroeesern);
            var point2_3 = new Point(0, 14 * vergroeesern);
            var point2_4 = new Point(5 * vergroeesern, 16 * vergroeesern);
            polygonTwo.StartPoint = point2_1;
            polygonTwo.IsClosed = true;
            polygonTwo.Segments.Add(new LineSegment() { Point = point2_2 });
            polygonTwo.Segments.Add(new LineSegment() { Point = point2_3 });
            polygonTwo.Segments.Add(new LineSegment() { Point = point2_4 });
            // Wenn ich abgerundete Ecken machen möchte, muss ich das relative kompliziert mit quadratischen Bezier Kurven machen
            // Siehe zum Beispiel hier: https://www.codeproject.com/Articles/128705/WPF-rounded-corners-polygon

            var pathGeometry = new PathGeometry();
            pathGeometry.FillRule = FillRule.Nonzero;
            pathGeometry.Figures.Add(polygonOne);
            pathGeometry.Figures.Add(polygonTwo);
            _sampleShape.Data = pathGeometry;
            _canvas.Children.Add(_sampleShape);

            // Microsoft Documentation:
            //      The greater the value of a given element, the more likely the element is to appear in the foreground
            //      Members of a Children collection that have equal ZIndex values are rendered in the order in which they appear in the visual tree.
            //      You can determine the index position of a child by iterating the members of the Children collection.
            //
            // With SetZIndex we can set the ZIndex value explicitely. All other elements have ZIndex == 0
            // Alle lines have ZIndex == 0. If we set the ZIndex of the shape to 1 it appears in the foreground.
            Canvas.SetZIndex(_sampleShape, 1);

            PrintNextLineWithShape();
        }


        private void PrintNextLineWithShape()
        {
            var shapeWidth = _sampleShape.Data.Bounds.Width;
            var shapeHeight = _sampleShape.Data.Bounds.Height;

            const double AnimationDuration = 1.5; // 0.9;

            double y = 50 + _counter * 10;
            var leftToRight = (_counter % 2 == 0);
            double x1 = leftToRight ? 100 : 400;
            double x2 = leftToRight ? 400 : 100;
            var angle = leftToRight ? 90 : 270;

            // Turtle-Spitze (Koordinaten 0,0) muss auf x1 starten und nach x2 wandern:
            var turtleX1 = x1; // - shapeWidth / 2;
            _turtleX2 = x2; // - shapeWidth / 2;

            // (0,0) dahin setzen, wo auch die Linie begingt
            Canvas.SetTop(_sampleShape, y - 0); // shapeHeight / 2);
            Canvas.SetLeft(_sampleShape, turtleX1);

            // Drehung und Bewegung sind zwei Transformationen. Daher mache ich eine Gruppe:
            if (_turtleTransforms == null)
            {
                _turtleTransforms = new TransformGroup();
                _sampleShape.RenderTransform = _turtleTransforms;
            }

            // Drehung der Turtle (ohne Animation):
            if (_rotateTransform == null)
            {
                // Drehen um den (0, 0) des Polygons (dieser Punkt liegt auf der Pen-Linie)
                _rotateTransform = new RotateTransform(angle, 0, 0); // shapeWidth / 2, shapeHeight/ 2);
                _turtleTransforms.Children.Add(_rotateTransform);
            }
            else
            {
                _rotateTransform.Angle = angle;
            }


            // Testweise rotieren lassen, hier aber parallel zum Movement
            var turtleRotateAnimation = new DoubleAnimation(angle, TimeSpan.FromSeconds(AnimationDuration));
            turtleRotateAnimation.AutoReverse = false;
            _rotateTransform.BeginAnimation(RotateTransform.AngleProperty, turtleRotateAnimation);

            // Verschiebung der Turtle (mit Animation)
            if (_turtleTranslate == null)
            {
                _turtleTranslate = new TranslateTransform();
                _turtleTransforms.Children.Add(_turtleTranslate);
            }

            // Neue Animation
            var turtleMoveAnimation = new DoubleAnimation(0, _turtleX2 - turtleX1, TimeSpan.FromSeconds(AnimationDuration))
            {
                AutoReverse = false
            };
            turtleMoveAnimation.Completed += (sender, args) => MoveAnimationIsFinished(sender, args, true);
            _turtleTranslate.BeginAnimation(TranslateTransform.XProperty, turtleMoveAnimation);

            // Gesamt-Transormation der Turtle
            _turtleAnimationIsFinished = false;


            // Die Linie (erst mal noch nicht animiert)
            _currentLine = new Line()
            {
                Stroke = System.Windows.Media.Brushes.LightSteelBlue,
                X1 = x1,
                Y1 = y,
                X2 = x2,
                Y2 = y,
                StrokeThickness = 1
            };

            // Linie animiert bis zu x2 ziehen
            var lineAnimation = new DoubleAnimation
            {
                From = x1,
                To = x2,
                Duration = new Duration(TimeSpan.FromSeconds(AnimationDuration))
            };
            lineAnimation.Completed += (sender, args) => MoveAnimationIsFinished(sender, args, false);
            _currentLine.BeginAnimation(Line.X2Property, lineAnimation);
            _lineAnimationIsFinished = false;


            _canvas.Children.Add(_currentLine);


            // Für den nächsten Durchlauf
            _counter++;

        }
        private void MoveAnimationIsFinished(object _, EventArgs _2, bool isTurtleAnimation)
        {
            if (isTurtleAnimation)
            {
                _turtleAnimationIsFinished = true;
            }
            else
            {
                _lineAnimationIsFinished = true;
                // Animation beenden, so dass sie keine Ressourcen mehr verbraucht:
                _currentLine.BeginAnimation(Line.X2Property, null);
            }

            if (_turtleAnimationIsFinished && _lineAnimationIsFinished)
            {
                if (_counter < 10)
                {
                    PrintNextLineWithShape();
                }
                else
                {
                    // Animation beenden, so dass sie keine Ressourcen mehr verbraucht:
                    // Siehe Using the Compose HandoffBehavior Consumes System Resources
                    // in https://docs.microsoft.com/de-de/dotnet/desktop/wpf/graphics-multimedia/animation-tips-and-tricks?view=netframeworkdesktop-4.8
                    _turtleTranslate.BeginAnimation(TranslateTransform.XProperty, null);

                    // Durch das null-Setzen der Animation, hat das Image wieder den anfänglichen x-Wert. Wir müssen den Ziel-x-Wert explizit setzen.
                    Canvas.SetLeft(_image, _turtleX2);
                    Canvas.SetLeft(_sampleShape, _turtleX2);
                }
            }
        }



        private void PrintNextLineWithImage()
        {
            const int ImageSize = 48; // Das Image ist 48 x 48;
            const double AnimationDuration = 0.9;

            double y = 50 + _counter * 10;
            var leftToRight = (_counter % 2 == 0);
            double x1 = leftToRight ? 100 : 400;
            double x2 = leftToRight ? 400 : 100;
            var angle = leftToRight ? 90 : 270;

            // Turtle auf die richtige y-Position
            var turtleX1 = x1 - ImageSize / 2;
            _turtleX2 = x2 - ImageSize / 2;

            Canvas.SetTop(_sampleShape, y - ImageSize / 2);
            Canvas.SetLeft(_sampleShape, turtleX1);

            // Drehung und Bewegung sind zwei Transformationen. Daher mache ich eine Gruppe:
            if (_turtleTransforms == null)
            {
                _turtleTransforms = new TransformGroup();
                _sampleShape.RenderTransform = _turtleTransforms;
            }

            // Drehung der Turtle (ohne Animation):
            if (_rotateTransform == null)
            {
                _rotateTransform = new RotateTransform(angle, ImageSize / 2, ImageSize / 2);
                _turtleTransforms.Children.Add(_rotateTransform);
            }
            else
            {
                _rotateTransform.Angle = angle;
            }

            // Verschiebung der Turtle (mit Animation)
            if (_turtleTranslate == null)
            {
                _turtleTranslate = new TranslateTransform();
                _turtleTransforms.Children.Add(_turtleTranslate);
            }

            // Neue Animation
            var turtleMoveAnimation = new DoubleAnimation(0, _turtleX2 - turtleX1, TimeSpan.FromSeconds(AnimationDuration))
            {
                AutoReverse = false
            };
            turtleMoveAnimation.Completed += (sender, args) => MoveAnimationIsFinished(sender, args, true);
            _turtleTranslate.BeginAnimation(TranslateTransform.XProperty, turtleMoveAnimation);

            // Gesamt-Transormation der Turtle
            _turtleAnimationIsFinished = false;


            // Die Linie (erst mal noch nicht animiert)
            _currentLine = new Line()
            {
                Stroke = System.Windows.Media.Brushes.LightSteelBlue,
                X1 = x1,
                Y1 = y,
                X2 = x2,
                Y2 = y,
                StrokeThickness = 1
            };

            // Linie animiert bis zu x2 ziehen
            var lineAnimation = new DoubleAnimation
            {
                From = x1,
                To = x2,
                Duration = new Duration(TimeSpan.FromSeconds(AnimationDuration * 1.1))
            };
            lineAnimation.Completed += (sender, args) => MoveAnimationIsFinished(sender, args, false);
            _currentLine.BeginAnimation(Line.X2Property, lineAnimation);
            _lineAnimationIsFinished = false;


            _canvas.Children.Add(_currentLine);


            // Für den nächsten Durchlauf
            _counter++;

        }

        /*
        private void PrintingLinesOldVersion()
        {
            _counter = 0;
            var _canvas = new Canvas() { Width = 300, Height = 1000 };
            this.Content = _canvas;

            var _image = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/Silver/Source/repos/simple-graphics-for-csharp-beginners/WpfAppWithAnimations/Bitmap1.bmp")),
                Name = "TurtleImage",
                RenderTransform = new RotateTransform(90)
            };
            _canvas.Children.Add(_image);
            Canvas.SetLeft(_image, 70);
            Canvas.SetTop(_image, 10);

            double newY = 100;
            double newX = 200;
            var turtleAnimation = new DoubleAnimation();
            var top = Canvas.GetTop(_image);
            var left = Canvas.GetLeft(_image);
            var trans = new TranslateTransform();
            _image.RenderTransform = trans;
            DoubleAnimation anim1 = new DoubleAnimation(top, newY - top, TimeSpan.FromSeconds(5))
            {
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            var anim2 = new DoubleAnimation(left, newX - left, TimeSpan.FromSeconds(5))
            {
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
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
                var myDoubleAnimation = new DoubleAnimation
                {
                    From = 10,
                    To = 250,
                    Duration = new Duration(TimeSpan.FromSeconds(5)),
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever
                };

                line.BeginAnimation(Line.X2Property, myDoubleAnimation);


                _canvas.Children.Add(line);

            }

            // Achtung: Clocks verbrauchen Performance, auch wenn sie zu Ende sind. Daher muss man sie am besten wieder entfernen
            // Siehe Using the Compose HandoffBehavior Consumes System Resources
            // in https://docs.microsoft.com/de-de/dotnet/desktop/wpf/graphics-multimedia/animation-tips-and-tricks?view=netframeworkdesktop-4.8
        }
        */

        /*
         * 
        /// <summary>
        /// Siehe https://docs.microsoft.com/de-de/dotnet/desktop/wpf/graphics-multimedia/animation-overview?view=netframeworkdesktop-4.8
        /// </summary>
        private void Animation1Initialize()
        {
            var myPanel = new StackPanel
            {
                Margin = new Thickness(10)
            };

            var myRectangle = new Rectangle
            {
                Name = "myRectangle"
            };
            this.RegisterName(myRectangle.Name, myRectangle);
            myRectangle.Width = 100;
            myRectangle.Height = 100;
            myRectangle.Fill = Brushes.Blue;

            var myDoubleAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = new Duration(TimeSpan.FromSeconds(5)),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            _myStoryboard = new Storyboard();
            _myStoryboard.Children.Add(myDoubleAnimation);
            Storyboard.SetTargetName(myDoubleAnimation, myRectangle.Name);
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Rectangle.OpacityProperty));

            // Use the Loaded event to start the Storyboard.
            myRectangle.Loaded += new RoutedEventHandler(MyAnimation1EventHandler);
            myPanel.Children.Add(myRectangle);
            this.Content = myPanel;
        }
        *
        */

        /*
         * 
        private void MyAnimation1EventHandler(object sender, RoutedEventArgs e)
        {
            _myStoryboard.Begin(this);
        }
        *
        */
    }
}
