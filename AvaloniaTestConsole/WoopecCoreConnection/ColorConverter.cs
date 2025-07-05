using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;

namespace AvaloniaTestConsole.WoopecCoreConnection
{
    internal class ColorConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color Convert(Woopec.Core.Color color)
        {
            return Color.FromArgb((byte)color.Alpha, (byte)color.R, (byte)color.G, (byte)color.B);
        }
    }
}
