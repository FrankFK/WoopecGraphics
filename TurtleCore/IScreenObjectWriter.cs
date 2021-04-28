using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    internal delegate void AnimationIsFinished(int chainId, int objectId);

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
        public void StartAnimaton(ScreenObject screenObject);

        /// <summary>
        /// The Writer calls these events for every animation which is finished
        /// </summary>
        public event AnimationIsFinished OnAnimationIsFinished;

        /// <summary>
        /// Draw the object directly, no animation
        /// </summary>
        /// <param name="screenObject"></param>
        public void Draw(ScreenObject screenObject);

    }
}
