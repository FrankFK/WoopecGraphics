using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Graphics.Interface.Dtos;

namespace Woopec.Graphics.Avalonia.Converters
{
    internal static class ColorConverter
    {
        public static Color ConvertDto(DtoColor woopecColor)
        {
            return new Color(Convert.ToByte(woopecColor.Alpha), Convert.ToByte(woopecColor.R), Convert.ToByte(woopecColor.G), Convert.ToByte(woopecColor.B));
        }
    }
}
