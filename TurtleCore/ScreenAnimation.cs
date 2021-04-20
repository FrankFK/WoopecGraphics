using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    /// <summary>
    /// Base class for ScreenAnimationMovement and so on.
    /// </summary>
    public class ScreenAnimation
    {
        /// <summary>
        /// Duration of the Animation
        /// </summary>
        public int Milliseconds { get; set; }

        /// <summary>
        /// When true: This animation waits until the predecessor animation has finised.
        /// The predecessor is that animation, which has the same ChainID and is created before this animation
        /// </summary>
        public bool StartWhenPredecessorHasFinished { get; set; }

        /// <summary>
        /// Animation with the same ChainId can wait for the predecssor to be finished.
        /// </summary>
        public int ChainID { get; set; }

    }
}
