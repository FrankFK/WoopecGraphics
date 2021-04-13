using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    /// <summary>
    /// Interface for output of turtle-graphics to a real "screen" (canvas in WPF).
    /// </summary>
    internal interface IScreenOutput
    {
        public int CreateLine();

        public void DrawLine(ScreenLine line);


    }
}
