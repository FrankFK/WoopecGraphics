using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Core;
using System.Windows.Controls;

namespace Woopec.Wpf
{
    internal class CanvasHelpers
    {
        /// <summary>
        /// Convert a vector (in turtle-coordinate-system) to a point on the canvas. 
        /// Canvas point (0, 0) is in the middle of the canvas
        /// </summary>
        /// <param name="turtleVector"></param>
        /// <param name="canvas"></param>
        /// <returns></returns>
        public static Point ConvertToCanvasPoint(Vec2D turtleVector, Canvas canvas)
        {
            // Frank 24.06.2023
            // - I'm using ActualWidth instead of Width, because it may be that Width is not set.
            // - If this method is called to early, before the canvas is rendered, ActualWidth has no value (NaN).
            //   Therefore it is important that the canvas is rendered erarlier than this function is called.
            // - Therefore it is important that the NextTask loop in WoopecCanvas is not called until everything is rendered and the values for ActualWidth are known
            return new Point(canvas.ActualWidth / 2 + turtleVector.X, canvas.ActualHeight / 2 - turtleVector.Y);
        }

        /// <summary>
        /// Convert a vector (in turtle-coordinate-system) to a point on the canvas. 
        /// Point (0, 0) is not changed
        /// </summary>
        /// <param name="turtleVector"></param>
        /// <returns></returns>
        public static Point ConvertToCanvasOrientation(Vec2D turtleVector)
        {
            return new Point(turtleVector.X, -turtleVector.Y);
        }


        public static double ConvertToCanvasAngle(double turtleHeading)
        {
            return 90 - turtleHeading;
        }
    }
}
