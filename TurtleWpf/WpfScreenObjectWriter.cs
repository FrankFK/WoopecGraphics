using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TurtleCore;

namespace TurtleWpf
{
    internal class WpfScreenObjectWriter : IScreenObjectWriter
    {

        private readonly Canvas _canvas;

        private readonly List<Line> _lines;
        private readonly Dictionary<int, Path> _pathes;

        public WpfScreenObjectWriter(Canvas canvas)
        {
            _canvas = canvas;
            _lines = new();
            _pathes = new();
        }

        public void UpdateWithAnimation(ScreenObject screenObject)
        {
            if (!screenObject.HasAnimations)
                // The screenObject only has waited for other animations to be finished. But it has no animations itself.
                // We can handle it with a normal Update:
                Update(screenObject);
            else if (screenObject is ScreenLine)
                UpdateWithAnimationInternally(screenObject as ScreenLine);
            else if (screenObject is ScreenFigure)
                UpdateWithAnimationInternally(screenObject as ScreenFigure);
            else 
                throw new ArgumentOutOfRangeException(nameof(screenObject), "Paramter has wrong type");
        }

        public event AnimationIsFinished OnAnimationIsFinished;

        public void Update(ScreenObject screenObject)
        {
            if (screenObject is ScreenFigureCreate)
                CreateInternally(screenObject as ScreenFigureCreate);
            else if (screenObject is ScreenFigure)
                UpdateInternally(screenObject as ScreenFigure);
            else
                throw new ArgumentOutOfRangeException("ScreenObject has wrong type");
        }

        public void UpdateWithAnimationInternally(ScreenLine screenLine)
        {
            while (_lines.Count <= screenLine.ID)
            {
                _lines.Add(new Line());
            }

            var line = _lines[screenLine.ID];
            line.Stroke = new SolidColorBrush(ColorConverter.Convert(screenLine.Color));

            var canvasPoint1 = ConvertToCanvasPoint(screenLine.Point1);
            var canvasPoint2 = ConvertToCanvasPoint(screenLine.Point2);
            line.X1 = canvasPoint1.X;
            line.Y1 = canvasPoint1.Y;
            line.X2 = canvasPoint2.X;
            line.Y2 = canvasPoint2.Y;
            line.StrokeThickness = 2;

            var animation = screenLine.Animation;
            var effect = animation.Effects[0] as ScreenAnimationMovement;
            if (effect.AnimatedProperty != ScreenAnimationMovementProperty.Point2)
                throw new NotImplementedException();

            var canvasStartPoint = ConvertToCanvasPoint(effect.StartValue);
            var lineXAnimation = new DoubleAnimation
            {
                From = canvasStartPoint.X,
                To = canvasPoint2.X,
                Duration = new Duration(TimeSpan.FromMilliseconds(effect.Milliseconds))
            };
            line.BeginAnimation(Line.X2Property, lineXAnimation);
            var lineYAnimation = new DoubleAnimation
            {
                From = canvasStartPoint.Y,
                To = canvasPoint2.Y,
                Duration = new Duration(TimeSpan.FromMilliseconds(effect.Milliseconds))
            };
            lineYAnimation.Completed += (sender, args) => OnAnimationIsFinished(screenLine.GroupID, screenLine.ID);
            line.BeginAnimation(Line.Y2Property, lineYAnimation);

            _canvas.Children.Add(line);
        }

        private void UpdateWithAnimationInternally(ScreenFigure screenFigure)
        {
            if (_pathes.TryGetValue(screenFigure.ID, out var path))
            {
                path.Fill = new SolidColorBrush(ColorConverter.Convert(screenFigure.FillColor));
                path.Stroke = new SolidColorBrush(ColorConverter.Convert(screenFigure.OutlineColor));

                var positionOnCanvas = ConvertToCanvasPoint(screenFigure.Position);

                var transforms = new TransformGroup();
                path.RenderTransform = transforms;

                if (screenFigure.Animation.Effects[0] is ScreenAnimationMovement)
                {
                    var move = screenFigure.Animation.Effects[0] as ScreenAnimationMovement;

                    var oldPositionOnCanvas = ConvertToCanvasPoint(move.StartValue);

                    // We change left and top in canvas, because this way is the way we do in case of a rotation:
                    Canvas.SetLeft(path, oldPositionOnCanvas.X);
                    Canvas.SetTop(path, oldPositionOnCanvas.Y);

                    var rotateTransform = new RotateTransform(ConvertToCanvasAngle(screenFigure.Heading));
                    transforms.Children.Add(rotateTransform);

                    var translateTransform = new TranslateTransform();
                    transforms.Children.Add(translateTransform);


                    var positionXAnimation = new DoubleAnimation
                    {
                        From = 0,
                        To = positionOnCanvas.X - oldPositionOnCanvas.X,
                        Duration = new Duration(TimeSpan.FromMilliseconds(move.Milliseconds))
                    };
                    translateTransform.BeginAnimation(TranslateTransform.XProperty, positionXAnimation);
                    var positionYAnimation = new DoubleAnimation
                    {
                        From = 0,
                        To = positionOnCanvas.Y - oldPositionOnCanvas.Y,
                        Duration = new Duration(TimeSpan.FromMilliseconds(move.Milliseconds))
                    };
                    positionYAnimation.Completed += (sender, args) => OnAnimationIsFinished(screenFigure.GroupID, screenFigure.ID);
                    translateTransform.BeginAnimation(TranslateTransform.YProperty, positionYAnimation);
                }
                else if (screenFigure.Animation.Effects[0] is ScreenAnimationRotation)
                {
                    // We have to set (0, 0) to the actual position of the figure
                    // (Otherwise an animated rotation of the figure rotates around the upper left corner of the canvas)
                    Canvas.SetLeft(path, positionOnCanvas.X);
                    Canvas.SetTop(path, positionOnCanvas.Y);

                    var rotate = screenFigure.Animation.Effects[0] as ScreenAnimationRotation;

                    var rotateTransform = new RotateTransform();
                    transforms.Children.Add(rotateTransform);

                    var oldAngle = ConvertToCanvasAngle(rotate.StartValue);
                    var newAngle = ConvertToCanvasAngle(screenFigure.Heading);

                    var rotateAnimation = new DoubleAnimation
                    {
                        From = oldAngle,
                        To = newAngle,
                        Duration = new Duration(TimeSpan.FromMilliseconds(rotate.Milliseconds))
                    };
                    rotateAnimation.Completed += (sender, args) => OnAnimationIsFinished(screenFigure.GroupID, screenFigure.ID);
                    rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
                }

                if (!_canvas.Children.Contains(path))
                {
                    _canvas.Children.Add(path);
                }
            }
            else
            {
                throw new KeyNotFoundException($"No WPF path object found for figure id {screenFigure.ID}.");
            }
        }


        private void CreateInternally(ScreenFigureCreate figureCreate)
        {
            if (!(figureCreate.Shape is TurtleCore.Shape))
                throw new NotImplementedException($"Shapes of type {figureCreate.Shape.Type} are not implemented yet.");

            var shape = figureCreate.Shape as TurtleCore.Shape;

            var path = new Path();
            var pathGeometry = new PathGeometry();
            pathGeometry.FillRule = FillRule.Nonzero;
            path.Data = pathGeometry;
            foreach (var component in shape.Components)
            {
                if (component.Polygon.Count > 0)
                {
                    var pathFigure = new PathFigure();
                    pathFigure.StartPoint = ConvertToCanvasOrientation(component.Polygon[0]);
                    for (int i = 1; i < component.Polygon.Count; i++)
                    {
                        pathFigure.Segments.Add(new LineSegment() { Point = ConvertToCanvasOrientation(component.Polygon[i]) });
                    }
                    pathGeometry.Figures.Add(pathFigure);
                }
            }

            // Microsoft Documentation:
            //      The greater the value of a given element, the more likely the element is to appear in the foreground
            //      Members of a Children collection that have equal ZIndex values are rendered in the order in which they appear in the visual tree.
            //      You can determine the index position of a child by iterating the members of the Children collection.
            //
            // With SetZIndex we can set the ZIndex value explicitely. All other elements have ZIndex == 0
            // Alle lines have ZIndex == 0. If we set the ZIndex of the shape to 1 it appears in the foreground.
            Canvas.SetZIndex(path, 1);

            _pathes.Add(figureCreate.ID, path);
        }

        private void UpdateInternally(ScreenFigure screenFigure)
        {
            if (_pathes.TryGetValue(screenFigure.ID, out var path))
            {
                if (screenFigure.IsVisible)
                {
                    path.Fill = new SolidColorBrush(ColorConverter.Convert(screenFigure.FillColor));
                    path.Stroke = new SolidColorBrush(ColorConverter.Convert(screenFigure.OutlineColor));

                    var positionOnCanvas = ConvertToCanvasPoint(screenFigure.Position);

                    var transforms = new TransformGroup();
                    path.RenderTransform = transforms;

                    var rotateTransform = new RotateTransform(ConvertToCanvasAngle(screenFigure.Heading));
                    transforms.Children.Add(rotateTransform);

                    // We have to set (0, 0) to the actual position of the figure
                    // (Otherwise an animated rotation of the figure rotates around the upper left corner of the canvas)
                    Canvas.SetLeft(path, positionOnCanvas.X);
                    Canvas.SetTop(path, positionOnCanvas.Y);

                    // var translateTransform = new TranslateTransform(positionOnCanvas.X, positionOnCanvas.Y);
                    // transforms.Children.Add(translateTransform);

                    if (!_canvas.Children.Contains(path))
                    {
                        _canvas.Children.Add(path);
                    }
                }
                else
                {
                    _canvas.Children.Remove(path);
                }
            }
            else
            {
                throw new KeyNotFoundException($"No WPF path object found for figure id {screenFigure.ID}.");
            }
        }

        /// <summary>
        /// Convert a vector (in turtle-coordinate-system) to a point on the canvas. 
        /// Canvas point (0, 0) is in the middle of the canvas
        /// </summary>
        /// <param name="turtleVector"></param>
        /// <returns></returns>
        private Point ConvertToCanvasPoint(Vec2D turtleVector)
        {
            return new Point(_canvas.Width / 2 + turtleVector.XCor, _canvas.Height / 2 - turtleVector.YCor);
        }

        /// <summary>
        /// Convert a vector (in turtle-coordinate-system) to a point on the canvas. 
        /// Point (0, 0) is not changed
        /// </summary>
        /// <param name="turtleVector"></param>
        /// <returns></returns>
        private static Point ConvertToCanvasOrientation(Vec2D turtleVector)
        {
            return new Point(turtleVector.XCor, -turtleVector.YCor);
        }


        private static double ConvertToCanvasAngle(double turtleHeading)
        {
            return 90 - turtleHeading;
        }


    }
}
