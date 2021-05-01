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

        public WpfScreenObjectWriter(Canvas canvas)
        {
            _canvas = canvas;
            _lines = new();
        }

        public void StartAnimaton(ScreenObject screenObject)
        {
            var screenLine = screenObject as ScreenLine;

            while (_lines.Count <= screenLine.ID)
            {
                _lines.Add(new Line());
            }

            var line = _lines[screenLine.ID];
            line.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString("green");
            var canvasPoint1 = ConvertToCanvasVector(screenLine.Point1);
            var canvasPoint2 = ConvertToCanvasVector(screenLine.Point2);
            line.X1 = canvasPoint1.XCor;
            line.Y1 = canvasPoint1.YCor;
            line.X2 = canvasPoint2.XCor;
            line.Y2 = canvasPoint2.YCor;
            line.StrokeThickness = 2;

            var animation = screenObject.Animation;
            var effect = animation.Effects[0] as ScreenAnimationMovement;
            if (effect.AnimatedProperty != ScreenAnimationMovementProperty.Point2)
                throw new NotImplementedException();

            var canvasStartVector = ConvertToCanvasVector(effect.StartValue);
            var lineXAnimation = new DoubleAnimation
            {
                From = canvasStartVector.XCor,
                To = canvasPoint2.XCor,
                Duration = new Duration(TimeSpan.FromMilliseconds(effect.Milliseconds))
            };
            line.BeginAnimation(Line.X2Property, lineXAnimation);
            var lineYAnimation = new DoubleAnimation
            {
                From = canvasStartVector.YCor,
                To = canvasPoint2.YCor,
                Duration = new Duration(TimeSpan.FromMilliseconds(effect.Milliseconds))
            };
            lineYAnimation.Completed += (sender, args) => OnAnimationIsFinished(animation.GroupID, screenObject.ID);
            line.BeginAnimation(Line.Y2Property, lineYAnimation);

            _canvas.Children.Add(line);
        }

        public event AnimationIsFinished OnAnimationIsFinished;

        public void Draw(ScreenObject screenObject)
        {
            throw new NotImplementedException();
        }

        private Vec2D ConvertToCanvasVector(Vec2D turtleVector)
        {
            return new Vec2D(_canvas.Width / 2 + turtleVector.XCor, _canvas.Height / 2 - turtleVector.YCor);
        }

    }
}
