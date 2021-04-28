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
        public int Milliseconds { get { return Effects.Max(e => e.Milliseconds); } }

        /// <summary>
        /// When true: This animation waits until the predecessor animation has finised.
        /// The predecessor is that animation, which has the same GroupID and is created before this animation
        /// </summary>
        public bool StartWhenPredecessorHasFinished { get; set; }

        /// <summary>
        /// Animation with the same GroupId can wait for the predecssor to be finished.
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// One Animation can consist of several effects (e.g. effect one changes the start-point of a line and effect two changes the end-point of a line)
        /// </summary>
        public List<ScreenAnimationEffect> Effects { get; init; }

        public ScreenAnimation()
        {
            Effects = new();
        }

    }
}
