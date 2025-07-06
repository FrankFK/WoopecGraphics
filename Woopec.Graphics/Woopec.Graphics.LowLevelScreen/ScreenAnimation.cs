using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Graphics.LowLevelScreen
{
    /// <summary>
    /// Base class for ScreenAnimationMovement and so on.
    /// </summary>
    internal class ScreenAnimation
    {
        /// <summary>
        /// Duration of the Animation
        /// </summary>
        public int Milliseconds { get { return Effects.Max(e => e.Milliseconds); } }




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
