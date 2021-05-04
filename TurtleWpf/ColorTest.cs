using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TurtleWpf
{
    public class ColorTest
    {
        public static void EnumarateColors()
        {
            var sb = new StringBuilder();
            var colorValueMethods = typeof(Colors).GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach (var colorValueMethod in colorValueMethods)
            {
                var result = colorValueMethod.Invoke(null, null);
                var col = (Color)result;
                var methodName = colorValueMethod.Name[4..]; // das get_ am Anfang abschneiden
                // Beispiel-Code: public static Color AliceBlue { get { return new Color(0, 0, 0); } }

                var code = $"public static Color {methodName} {{ get {{ return new Color({col.R}, {col.G}, {col.B}); }} }} ";
                sb.AppendLine(code);
            }

            var codeComplete = sb.ToString();
        }

    }
}

