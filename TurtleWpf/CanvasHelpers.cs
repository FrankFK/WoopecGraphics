using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Core;

namespace Woopec.Wpf
{
    class CanvasHelpers
    {
        /// <summary>
        /// Convert a vector (in turtle-coordinate-system) to a point on the canvas. 
        /// Canvas point (0, 0) is in the middle of the canvas
        /// </summary>
        /// <param name="turtleVector"></param>
        /// <returns></returns>
        public static Point ConvertToCanvasPoint(Vec2D turtleVector, double canvasWidth, double canvasHeigth)
        {
            return new Point(canvasWidth / 2 + turtleVector.XCor, canvasHeigth / 2 - turtleVector.YCor);
        }

        /// <summary>
        /// Convert a vector (in turtle-coordinate-system) to a point on the canvas. 
        /// Point (0, 0) is not changed
        /// </summary>
        /// <param name="turtleVector"></param>
        /// <returns></returns>
        public static Point ConvertToCanvasOrientation(Vec2D turtleVector)
        {
            return new Point(turtleVector.XCor, -turtleVector.YCor);
        }


        public static double ConvertToCanvasAngle(double turtleHeading)
        {
            return 90 - turtleHeading;
        }
    }
}
