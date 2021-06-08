using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Core
{
    /// <summary>
    /// Base-Class for ScreenLine and ScreenForm
    /// </summary>
    internal class ScreenObject
    {
        internal const int NoGroupId = 0;
        private int _groupId;
        private int _waitForAnimationsOfGroupId = NoGroupId;

        public int ID { get; set; }

        /// <summary>
        /// Multiple ScreenObjects can belong to a group. For instance: All ScreenObjects created by the same Turtle-instance have the same GroupId.
        /// If ScreenObjects belong to a group they are written to the screen in the same order as they are inserted into the channel.
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

        /// <summary>
        /// true if this screen object belongs to a group
        /// </summary>
        public bool BelongsToAGroup {  get { return _groupId != NoGroupId; } }

        /// <summary>
        /// When set to a value of a GroupID: This object waits until all already produced animations of the given GroupId are finished.
        /// This can be the same group as this.GroupID, it also can be another group.
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

        /// <summary>
        /// true, if this animation waits for other animations.
        /// Set to false, if this animation should not wait for other animations.
        /// </summary>
        public bool WaitsForAnimations
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


        public ScreenAnimation Animation { get; set; }

        /// <summary>
        /// true if this screen object has animations
        /// </summary>
        public bool HasAnimations { get { return (Animation != null && Animation.Effects.Count > 0); } }


        public ScreenObject()
        {
            Animation = null;
            _waitForAnimationsOfGroupId = NoGroupId;
            _groupId = NoGroupId;
        }

    }
}
