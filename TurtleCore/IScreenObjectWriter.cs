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

        /// <summary>
        /// Draws the object with an animation
        /// </summary>
        /// <param name="screenObject"></param>
        /// <param name="whenFinished">This action must be called, when the animation is finished</param>
        public void StartAnimaton(ScreenObject screenObject, Action<int, int> whenFinished);

        /// <summary>
        /// Draw the object directly, no animation
        /// </summary>
        /// <param name="screenObject"></param>
        public void Draw(ScreenObject screenObject);

    }
}
