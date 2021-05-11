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
        internal const int NoGroupId = 0;

        /// <summary>
        /// Duration of the Animation
        /// </summary>
        public int Milliseconds { get { return Effects.Max(e => e.Milliseconds); } }


        /// <summary>
        /// Each animation has a GroupId
        /// An animation can wait until all previous animations of a specific group are finished
        /// </summary>
        public int GroupID
        {
            get
            {
                return _groupId;
            }
            init
            {
                if (value == NoGroupId)
                    throw new ArgumentOutOfRangeException($"{NoGroupId} is not allowed for GroupID");
                _groupId = value;
            }
        }
        private int _groupId;

        /// <summary>
        /// When set to a value of a GroupID: This animation waits until all already produced animations of the given GroupId are finished-
        /// </summary>
        public int WaitForAnimationsOfGroupID
        {
            get
            {
                return _waitForAnimationsOfGroupId;
            }
            set
            {
                if (value == NoGroupId)
                    throw new ArgumentOutOfRangeException($"{NoGroupId} is not allowed for GroupId");
                _waitForAnimationsOfGroupId = value;
            }
        }
        private int _waitForAnimationsOfGroupId = NoGroupId;

        /// <summary>
        /// true, if this animation waits for other animations.
        /// Set to false, if this animation should not wait for other animations.
        /// </summary>
        public bool WaitForAnimations
        {
            get
            {
                return (_waitForAnimationsOfGroupId != NoGroupId);
            }

            set
            {
                if (value == true)
                    throw new ArgumentOutOfRangeException("Only false is allowed. Use WaitForAnimationsOfGroupId if you want to wait for animations");
                else
                    _waitForAnimationsOfGroupId = NoGroupId;
            }
        }

        /// <summary>
        /// One Animation can consist of several effects (e.g. effect one changes the start-point of a line and effect two changes the end-point of a line)
        /// </summary>
        public List<ScreenAnimationEffect> Effects { get; init; }

        public ScreenAnimation(int groupId)
        {
            GroupID = groupId;
            Effects = new();
            _waitForAnimationsOfGroupId = NoGroupId;
        }

    }
}
