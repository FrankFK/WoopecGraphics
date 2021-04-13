using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TurtleCore;


namespace TurtleWpf
{
    internal class ScreenOutput : IScreenOutput
    {
        private readonly Canvas _canvas;

        private List<Line> _lines;

        public ScreenOutput(Canvas canvas)
        {
            _canvas = canvas;
            _lines = new();
        }

        public int CreateLine()
        {
            var line = new Line();
            _lines.Add(line);
            return _lines.Count - 1;

        }

        public void DrawLine(ScreenLine screenLine)
        {
            var line = _lines[screenLine.ID];
            line.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString("green");
            line.X1 = _canvas.Width / 2 + screenLine.StartPoint.XCor;
            line.Y1 = _canvas.Height / 2 - screenLine.StartPoint.YCor;
            line.X2 = _canvas.Width / 2 + screenLine.EndPoint.XCor;
            line.Y2 = _canvas.Height / 2 - screenLine.EndPoint.YCor;
            line.StrokeThickness = 2;

            // TODO: Nicht direkt zum Canvas, sondern in einen internen Puffer
            _canvas.Children.Add(line);
        }

    }
}
