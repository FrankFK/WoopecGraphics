using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.Shapes;
using Woopec.Graphics.Avalonia.Converters;
using Woopec.Graphics.Interface.Dtos;

namespace Woopec.Graphics.Avalonia.CanvasContent
{
    /// <summary>
    /// One instance of this class contains all Avalonia Lines which are shown in the Canvas
    /// </summary>
    internal class CanvasLines
    {
        private readonly ICanvas _canvas;

        private readonly List<Line> _lines;

        public CanvasLines(ICanvas canvas)
        {
            _canvas = canvas;
            _lines = new();
        }
        public CanvasChildrenChange Update(ScreenLine screenLine)
        {
            var line = CreateLine(screenLine);

            return new(CanvasOperation.Add, line);
        }

        private Line CreateLine(ScreenLine screenLine)
        {
            while (_lines.Count <= screenLine.ID)
            {
                _lines.Add(new Line());
            }

            var line = _lines[screenLine.ID];
            line.Stroke = new SolidColorBrush(ColorConverter.ConvertDto(screenLine.Color));

            line.StartPoint = CanvasConverter.ConvertToCanvasPoint(screenLine.Point1, _canvas.Bounds);
            line.EndPoint = CanvasConverter.ConvertToCanvasPoint(screenLine.Point2, _canvas.Bounds);

            line.StrokeThickness = 1;

            return line;
        }
    }
}
