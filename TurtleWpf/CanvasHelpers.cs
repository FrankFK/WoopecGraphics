using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Graphics;
using System.Windows.Controls;
using Woopec.Graphics.InternalDtos;

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
        /// Convert a point on the canvas to a vector in turtle-coordinate-system
        /// Canvas point (0, 0) is in the middle of the canvas
        /// </summary>
        /// <param name="canvasPoint"></param>
        /// <param name="canvas"></param>
        /// <returns></returns>
        public static Vec2DValue ConvertToVec2DPoint(Point canvasPoint, Canvas canvas)
        {
            return new Vec2DValue(canvasPoint.X - canvas.ActualWidth / 2, canvas.ActualHeight / 2 - canvasPoint.Y);
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

        public static void SetLowerLeftCornerOfWindowToCanvasPoint(Vec2D position, Canvas canvas, Window window)
        {
            // Place the window such that its lower left corner is at _position.
            // The following code does not work place the window perfectly, but I do not know how to implement it better.
            var positionInCanvasCoordinates = ConvertToCanvasPoint(position, canvas);

            var transform = canvas.TransformToAncestor(window.Owner);

            var positionRelativeToOwner = transform.Transform(positionInCanvasCoordinates);
            var positionOnScreen = window.Owner.PointToScreen(positionRelativeToOwner);

            // normally the upper left corner of the TextInputWindow is positioned, but we want to position the lower left corner.
            // Therefore we have to move it up by its ActualHeight
            var moveWindowToTop = window.ActualHeight;


            window.WindowStartupLocation = WindowStartupLocation.Manual;
            window.Left = positionOnScreen.X;
            window.Top = positionOnScreen.Y - moveWindowToTop;
        }
    }
}
