using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    /// <summary>
    /// Interface
    /// Write turtle-graphics objects to a real "screen" (for instance a canvas in WPF).
    /// </summary>
    internal interface IScreenObjectWriter
    {
        public int CreateLine();

        /// <summary>
        /// Später wegschmeißen
        /// </summary>
        /// <param name="line"></param>
        public void DrawLine(ScreenLine line);

        public void StartAnimaton(ScreenObject screenObject, Action<int> whenFinished);

    }
}
