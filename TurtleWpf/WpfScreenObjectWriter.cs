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

            _pathes.Add(figureCreate.ID, path);
        }

        private void UpdateInternally(ScreenFigure screenFigure)
        {
            if (_pathes.TryGetValue(screenFigure.ID, out var path))
            {
                path.Fill = new SolidColorBrush(ColorConverter.Convert(screenFigure.FillColor));
                path.Stroke = new SolidColorBrush(ColorConverter.Convert(screenFigure.OutlineColor));

                var positionOnCanvas = ConvertToCanvasPoint(screenFigure.Position);
                Canvas.SetLeft(path, positionOnCanvas.X);
                Canvas.SetTop(path, positionOnCanvas.Y);
                var transforms = new TransformGroup();
                var rotateTransform = new RotateTransform(screenFigure.Heading + 90);
                transforms.Children.Add(rotateTransform);
                var translateTransform = new TranslateTransform(50, 50);
                transforms.Children.Add(translateTransform);
                path.RenderTransform = transforms;

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



    }
}
