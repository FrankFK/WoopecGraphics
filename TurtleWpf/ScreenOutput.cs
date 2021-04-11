// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

        public ScreenOutput(Canvas canvas)
        {
            _canvas = canvas;
        }

        public void DrawLine(LineOnScreen lineOnScreen)
        {
            var line = new Line()
            {
                Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(lineOnScreen.Color),
                X1 = _canvas.Width / 2 + lineOnScreen.From.XCor,
                Y1 = _canvas.Height / 2 - lineOnScreen.From.YCor,
                X2 = _canvas.Width / 2 + lineOnScreen.To.XCor,
                Y2 = _canvas.Height / 2 - lineOnScreen.To.YCor,
                StrokeThickness = lineOnScreen.Width
            };

            // TODO: Nicht direkt zum Canvas, sondern in einen internen Puffer
            _canvas.Children.Add(line);
        }

    }
}
