// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using TurtleCore;


namespace TurtleWpf
{
    internal class TurtleOutput : ITurtleOutput
    {
        private readonly Canvas _canvas;

        public TurtleOutput(Canvas canvas)
        {
            _canvas = canvas;
        }

        public void Move(Vec2D from, Vec2D to)
        {
            var line = new Line()
            {
                Stroke = System.Windows.Media.Brushes.LightSteelBlue,
                X1 = _canvas.Width / 2 + from.XCor,
                Y1 = _canvas.Height / 2 - from.YCor,
                X2 = _canvas.Width / 2 + to.XCor,
                Y2 = _canvas.Height / 2 - to.YCor,
                StrokeThickness = 1
            };

            _canvas.Children.Add(line);
        }
    }
}
