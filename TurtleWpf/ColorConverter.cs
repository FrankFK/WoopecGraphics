using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Wpf
{
    /// <summary>
    /// Convert Turtle-Colors to WPF-Colors.
    /// For more informations see "Painting with Solid Colors and Gradients Overview" https://docs.microsoft.com/en-us/dotnet/desktop/wpf/graphics-multimedia/painting-with-solid-colors-and-gradients-overview?view=netframeworkdesktop-4.8
    /// </summary>
    internal static class ColorConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        /// <remarks>
        /// At the moment Turtle-Color does not contain a value for alpha-channel (transparency)
        /// </remarks>
        public static System.Windows.Media.Color Convert(Woopec.Graphics.Color color)
        {
            return System.Windows.Media.Color.FromArgb((byte)color.Alpha, (byte)color.R, (byte)color.G, (byte)color.B);
        }
    }
}
