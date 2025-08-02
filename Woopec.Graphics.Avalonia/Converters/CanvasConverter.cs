using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Graphics.Interface.Dtos;

namespace Woopec.Graphics.Avalonia.Converters
{
    /// <summary>
    /// Convert coordinates, angles according to a given canvas
    /// </summary>
    internal static class CanvasConverter
    {
        /// <summary>
        /// Convert a vector (in turtle-coordinate-system) to a point on the canvas. 
        /// Canvas point (0, 0) is in the middle of the canvas
        /// </summary>
        /// <param name="woopecVector"></param>
        /// <param name="canvas"></param>
        /// <returns></returns>
        public static Point ConvertToCanvasPoint(DtoVec2D woopecVector, Rect canvasBounds)
        {
            return new Point(canvasBounds.Width / 2 + woopecVector.X, canvasBounds.Height / 2 - woopecVector.Y);
        }
    }
}
