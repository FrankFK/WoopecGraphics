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
    internal class CanvasLines
    {
        private readonly double _canvasWidth;
        private readonly double _canvasHeight;

        private readonly List<Line> _lines;

        public CanvasLines(double canvasWidth, double canvasHeigth)
        {
            _canvasWidth = canvasWidth;
            _canvasHeight = canvasHeigth;
            _lines = new();
        }

        public CanvasChildrenChange UpdateWithAnimation(ScreenLine screenLine, AnimationIsFinished finishedEvent)
        {
            while (_lines.Count <= screenLine.ID)
            {
                _lines.Add(new Line());
            }

            var line = _lines[screenLine.ID];
            line.Stroke = new SolidColorBrush(ColorConverter.Convert(screenLine.Color));

            var canvasPoint1 = CanvasHelpers.ConvertToCanvasPoint(screenLine.Point1, _canvasWidth, _canvasHeight);
            var canvasPoint2 = CanvasHelpers.ConvertToCanvasPoint(screenLine.Point2, _canvasWidth, _canvasHeight);
            line.X1 = canvasPoint1.X;
            line.Y1 = canvasPoint1.Y;
            line.X2 = canvasPoint2.X;
            line.Y2 = canvasPoint2.Y;
            line.StrokeThickness = 2;

            var animation = screenLine.Animation;
            var effect = animation.Effects[0] as ScreenAnimationMovement;
            if (effect.AnimatedProperty != ScreenAnimationMovementProperty.Point2)
                throw new NotImplementedException();

            var canvasStartPoint = CanvasHelpers.ConvertToCanvasPoint(effect.StartValue, _canvasWidth, _canvasHeight);
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
            lineYAnimation.Completed += (sender, args) => finishedEvent(screenLine.GroupID, screenLine.ID);
            line.BeginAnimation(Line.Y2Property, lineYAnimation);   // We do not use HandoffBehavior.Compose, therefore we hopefully do not have to reset the animation to null (as described in https://docs.microsoft.com/de-de/dotnet/desktop/wpf/graphics-multimedia/animation-tips-and-tricks?view=netframeworkdesktop-4.8)

            return new(Operation.Add, line);
        }

    }
}
