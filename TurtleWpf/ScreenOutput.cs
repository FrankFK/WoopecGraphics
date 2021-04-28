using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TurtleCore;


namespace TurtleWpf
{
    internal class ScreenOutput : IScreenObjectProducer
    {
        private readonly Canvas _canvas;
        private readonly Channel<ScreenObject> _objectChannel;

        private int _lineCounter;
        private readonly List<Line> _lines;

        public ScreenOutput(Canvas canvas, Channel<ScreenObject> objectChannel)
        {
            _canvas = canvas;
            _objectChannel = objectChannel;
            _lineCounter = 0;
            _lines = new();
        }

        public int CreateLine()
        {
            _lineCounter++;
            return _lineCounter - 1;

        }

        public void DrawLine(ScreenLine screenLine)
        {
            _objectChannel.Writer.TryWrite(screenLine);
        }

        public async Task<ScreenObject> ReadScreenObjectAsync()
        {
            var screenObject = await _objectChannel.Reader.ReadAsync();
            return screenObject;
        }


        public void DrawScreenObject(ScreenObject screenObject)
        {
            var screenLine = screenObject as ScreenLine;

            while (_lines.Count <= screenLine.ID)
            {
                _lines.Add(new Line());
            }

            var line = _lines[screenLine.ID];
            line.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString("green");
            line.X1 = _canvas.Width / 2 + screenLine.Point1.XCor;
            line.Y1 = _canvas.Height / 2 - screenLine.Point1.YCor;
            line.X2 = _canvas.Width / 2 + screenLine.Point2.XCor;
            line.Y2 = _canvas.Height / 2 - screenLine.Point2.YCor;
            line.StrokeThickness = 2;

            var lineXAnimation = new DoubleAnimation
            {
                From = line.X1,
                To = line.X2,
                Duration = new Duration(TimeSpan.FromSeconds(2))
            };
            lineXAnimation.Completed += (sender, args) => AnimationIsFinished(sender, args, false);
            line.BeginAnimation(Line.X2Property, lineXAnimation);

            var lineYAnimation = new DoubleAnimation
            {
                From = line.Y1,
                To = line.Y2,
                Duration = new Duration(TimeSpan.FromSeconds(2))
            };
            line.BeginAnimation(Line.Y2Property, lineYAnimation);

            _canvas.Children.Add(line);
        }


        private void AnimationIsFinished(object _, EventArgs _2, bool isTurtleAnimation)
        {
            var task = ReadScreenObjectAsync();

            task.ContinueWith((t) =>
            {
                // Aus https://igorpopov.io/2018/06/16/asynchronous-programming-in-csharp-with-wpf/
                _canvas.Dispatcher.Invoke(() =>
                {
                    DrawScreenObject(t.Result);
                });
            });
        }

    }
}
