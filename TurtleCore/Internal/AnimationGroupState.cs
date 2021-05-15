using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore.Internal
{
    /// <summary>
    /// An instance of this class describes the actually known state of an animation group.
    /// - Is an animation running at the moment?
    /// - Are there other ScreenObjects that we already have consumed but that have to wait? 
    /// </summary>
    internal class AnimationGroupState
    {
        internal abstract class WaitingObject
        {
        }

        internal class WaitingScreenObject : WaitingObject
        {
            public ScreenObject ScreenObject;

            public bool WaitsForAnAnimation()
            {
                return ScreenObject.WaitsForAnimations;
            }

        }

        internal class WaitingOtherGroup : WaitingObject
        {
            public int OtherGroupId;
        }

        public int GroupID { get; init; }

        /// <summary>
        /// True if an animation of this group is running at the moment
        /// </summary>
        public bool AnimationIsRunning { get; set; }

        /// <summary>
        /// Normal case: Add a ScreenObject
        /// </summary>
        /// <param name="screenObject"></param>
        public void AddScreenObject(ScreenObject screenObject)
        {
            _waitingObjects.Add(new WaitingScreenObject() { ScreenObject = screenObject });
        }

        /// <summary>
        /// Special case: Another group is waiting until the current objects of this group are finished
        /// </summary>
        /// <param name="groupId"></param>
        public void AddWaitingOtherGroup(int groupId)
        {
            _waitingObjects.Add(new WaitingOtherGroup() { OtherGroupId = groupId });
        }

        public bool HasWaitingObjects()
        {
            return (_waitingObjects.Count > 0);
        }

        public bool HasWaitingScreenObject()
        {
            return (_waitingObjects.Count > 0 && _waitingObjects[0] is WaitingScreenObject);
        }


        public List<int> ExtractLeadingOtherGroupsReadyToRun()
        {
            var otherGroups = new List<int>();
            if (!AnimationIsRunning)
            {
                while (_waitingObjects.Count > 0 && _waitingObjects[0] is WaitingOtherGroup)
                {
                    otherGroups.Add((_waitingObjects[0] as WaitingOtherGroup).OtherGroupId);
                    _waitingObjects.RemoveAt(0);
                }
            }
            return otherGroups;
        }


        public List<ScreenObject> ExtractLeadingScreenObjectsReadyToRun()
        {
            List<ScreenObject> list = new();

            if (!AnimationIsRunning && _waitingObjects.Count > 0 && _waitingObjects[0] is WaitingScreenObject)
            {
                // Because animation is not running we can take the first screen object, even it is waiting for an animation
                var screenObject = (_waitingObjects[0] as WaitingScreenObject).ScreenObject;
                _waitingObjects.RemoveAt(0);
                list.Add(screenObject);
            }

            // Additionally we can take all consecutively screen objects that are not waiting for an animation
            while (_waitingObjects.Count > 0 && _waitingObjects[0] is WaitingScreenObject && !(_waitingObjects[0] as WaitingScreenObject).WaitsForAnAnimation())
            {
                var screenObject = (_waitingObjects[0] as WaitingScreenObject).ScreenObject;
                _waitingObjects.RemoveAt(0);
                list.Add(screenObject);
            }
            return list;
        }



        public AnimationGroupState(int groupId)
        {
            AnimationIsRunning = false;
            _waitingObjects = new();
            GroupID = groupId;
        }

        /// <summary>
        /// Objects that are waiting for the animation of this group. If AnimationIsRunning == false, the next object can be handled.
        /// </summary>
        private readonly List<WaitingObject> _waitingObjects;
    }
}
