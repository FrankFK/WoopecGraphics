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
using Woopec.Graphics.InternalFrontend;
using Woopec.Graphics.InternalDtos;

namespace Woopec.Wpf
{
    internal class CanvasPathes
    {
        private readonly Canvas _canvas;

        private readonly Dictionary<int, Path> _pathes;

        public CanvasPathes(Canvas canvas)
        {
            _canvas = canvas;
            _pathes = new();
        }



        public CanvasChildrenChange Update(ScreenFigure screenFigure)
        {
            var path = GetPathForId(screenFigure.ID);

            if (screenFigure.IsVisible)
            {
                UpdatePathUnanimated(path, screenFigure);

                return new(Operation.Add, path);
            }
            else
            {
                return new(Operation.Remove, path);
            }
        }

        public CanvasChildrenChange UpdateWithAnimation(ScreenFigure screenFigure, AnimationIsFinished finishedEvent)
        {
            var path = GetPathForId(screenFigure.ID);

            if (screenFigure.Animation.Effects[0] is ScreenAnimationMovement)
                UpdatePathWithMovementAnimation(path, screenFigure, finishedEvent);
            else if (screenFigure.Animation.Effects[0] is ScreenAnimationRotation)
                UpdatePathWithRotationAnimation(path, screenFigure, finishedEvent);
            else
                throw new ArgumentOutOfRangeException(nameof(screenFigure), $"Unexpected Animation-Type {screenFigure.Animation.Effects[0].GetType()}");

            return new(Operation.Add, path);
        }


        private void UpdatePathUnanimated(Path path, ScreenFigure screenFigure)
        {
            if (screenFigure.Shape != null)
                UpdatePathWithShape(path, screenFigure.Shape);

            path.Fill = new SolidColorBrush(ColorConverter.Convert(screenFigure.FillColor));
            if (screenFigure.OutlineColor != null)
                path.Stroke = new SolidColorBrush(ColorConverter.Convert(screenFigure.OutlineColor));

            var positionOnCanvas = CanvasHelpers.ConvertToCanvasPoint(screenFigure.Position, _canvas);

            var transforms = new TransformGroup();
            path.RenderTransform = transforms;

            var rotateTransform = new RotateTransform(CanvasHelpers.ConvertToCanvasAngle(screenFigure.Heading));
            transforms.Children.Add(rotateTransform);

            // We have to set (0, 0) to the actual position of the figure
            // (Otherwise an animated rotation of the figure rotates around the upper left corner of the canvas)
            Canvas.SetLeft(path, positionOnCanvas.X);
            Canvas.SetTop(path, positionOnCanvas.Y);
        }

        private void UpdatePathWithMovementAnimation(Path path, ScreenFigure screenFigure, AnimationIsFinished finishedEvent)
        {
            if (screenFigure.Shape != null)
                UpdatePathWithShape(path, screenFigure.Shape);

            path.Fill = new SolidColorBrush(ColorConverter.Convert(screenFigure.FillColor));
            if (screenFigure.OutlineColor != null)
                path.Stroke = new SolidColorBrush(ColorConverter.Convert(screenFigure.OutlineColor));

            var positionOnCanvas = CanvasHelpers.ConvertToCanvasPoint(screenFigure.Position, _canvas);

            var transforms = new TransformGroup();
            path.RenderTransform = transforms;

            var move = screenFigure.Animation.Effects[0] as ScreenAnimationMovement;

            var oldPositionOnCanvas = CanvasHelpers.ConvertToCanvasPoint(move.StartValue, _canvas);

            // We change left and top in canvas, because this way is the way we do in case of a rotation:
            Canvas.SetLeft(path, oldPositionOnCanvas.X);
            Canvas.SetTop(path, oldPositionOnCanvas.Y);

            var rotateTransform = new RotateTransform(CanvasHelpers.ConvertToCanvasAngle(screenFigure.Heading));
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
            positionYAnimation.Completed += (sender, args) => finishedEvent(screenFigure.GroupID, screenFigure.ID);
            translateTransform.BeginAnimation(TranslateTransform.YProperty, positionYAnimation);    // We do not use HandoffBehavior.Compose, therefore we hopefully do not have to reset the animation to null (as described in https://docs.microsoft.com/de-de/dotnet/desktop/wpf/graphics-multimedia/animation-tips-and-tricks?view=netframeworkdesktop-4.8)
        }

        private void UpdatePathWithRotationAnimation(Path path, ScreenFigure screenFigure, AnimationIsFinished finishedEvent)
        {
            if (screenFigure.Shape != null)
                UpdatePathWithShape(path, screenFigure.Shape);

            path.Fill = new SolidColorBrush(ColorConverter.Convert(screenFigure.FillColor));
            if (screenFigure.OutlineColor != null)
                path.Stroke = new SolidColorBrush(ColorConverter.Convert(screenFigure.OutlineColor));

            var positionOnCanvas = CanvasHelpers.ConvertToCanvasPoint(screenFigure.Position, _canvas);

            var transforms = new TransformGroup();
            path.RenderTransform = transforms;

            // We have to set (0, 0) to the actual position of the figure
            // (Otherwise an animated rotation of the figure rotates around the upper left corner of the canvas)
            Canvas.SetLeft(path, positionOnCanvas.X);
            Canvas.SetTop(path, positionOnCanvas.Y);

            var rotate = screenFigure.Animation.Effects[0] as ScreenAnimationRotation;

            var rotateTransform = new RotateTransform();
            transforms.Children.Add(rotateTransform);

            var oldAngle = CanvasHelpers.ConvertToCanvasAngle(rotate.StartValue);
            var newAngle = CanvasHelpers.ConvertToCanvasAngle(screenFigure.Heading);

            var rotateAnimation = new DoubleAnimation
            {
                From = oldAngle,
                To = newAngle,
                Duration = new Duration(TimeSpan.FromMilliseconds(rotate.Milliseconds))
            };
            rotateAnimation.Completed += (sender, args) => finishedEvent(screenFigure.GroupID, screenFigure.ID);
            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation); // We do not use HandoffBehavior.Compose, therefore we hopefully do not have to reset the animation to null (as described in https://docs.microsoft.com/de-de/dotnet/desktop/wpf/graphics-multimedia/animation-tips-and-tricks?view=netframeworkdesktop-4.8)
        }

        private void UpdatePathWithShape(Path path, DtoShapeBase screenFigureShape)
        {
            if (!(screenFigureShape is Woopec.Graphics.InternalDtos.DtoShape))
                throw new NotImplementedException($"Shapes of type {screenFigureShape.Type} are not implemented yet.");

            var shape = screenFigureShape as Woopec.Graphics.InternalDtos.DtoShape;

            // Set or change the pahtGeometry
            var pathGeometry = new PathGeometry
            {
                FillRule = FillRule.Nonzero
            };
            path.Data = pathGeometry;
            foreach (var component in shape.Components)
            {
                if (component.Polygon.Count > 0)
                {
                    var pathFigure = new PathFigure
                    {
                        StartPoint = CanvasHelpers.ConvertToCanvasOrientation(component.Polygon[0])
                    };
                    for (var i = 1; i < component.Polygon.Count; i++)
                    {
                        pathFigure.Segments.Add(new LineSegment() { Point = CanvasHelpers.ConvertToCanvasOrientation(component.Polygon[i]) });
                    }
                    pathGeometry.Figures.Add(pathFigure);
                }
            }
        }

        private Path GetPathForId(int figureId)
        {
            Path path;

            if (_pathes.TryGetValue(figureId, out var foundPath))
            {
                path = foundPath;
            }
            else
            {
                path = new Path();

                // Microsoft Documentation:
                //      The greater the value of a given element, the more likely the element is to appear in the foreground
                //      Members of a Children collection that have equal ZIndex values are rendered in the order in which they appear in the visual tree.
                //      You can determine the index position of a child by iterating the members of the Children collection.
                //
                // With SetZIndex we can set the ZIndex value explicitely. All other elements have ZIndex == 0
                // Alle lines have ZIndex == 0. If we set the ZIndex of the shape to 1 it appears in the foreground.
                Canvas.SetZIndex(path, 1);

                _pathes.Add(figureId, path);
            }

            return path;
        }
    }


}
