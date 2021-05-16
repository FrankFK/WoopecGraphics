using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    internal delegate void AnimationIsFinished(int groupId, int objectId);

    /// <summary>
    /// Interface
    /// Write turtle-graphics objects to a real "screen" (for instance a canvas in WPF).
    /// </summary>
    internal interface IScreenObjectWriter
    {
        /// <summary>
        /// Handles the object with an animation
        /// </summary>
        /// <param name="screenObject"></param>
        public void UpdateWithAnimation(ScreenObject screenObject);

        /// <summary>
        /// The Writer calls these events for every animation which is finished
        /// </summary>
        public event AnimationIsFinished OnAnimationIsFinished;

        /// <summary>
        /// Handles the object directly, no animation
        /// </summary>
        /// <param name="screenObject"></param>
        public void Update(ScreenObject screenObject);

    }
}
