using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Graphics.LowLevelScreen;

namespace Woopec.Graphics.Internal
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

        /// <summary>
        /// The writer needs a class that can send screen results (e.g. answer of a text input dialog window) back to the code that is waiting for it.
        /// </summary>
        /// <param name="producer"></param>
        public void SetScreenResultProducer(IScreenResultProducer producer);

    }
}
