using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Graphics.InternalObjects;

namespace Woopec.Graphics.Internal
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

        /// <summary>
        /// When the state reaches this object, the blocker in another group-state can be released.
        /// </summary>
        internal class MarkerForAGroupThatWaitsForCompletion : WaitingObject
        {
            public BlockerUntilAnotherGroupsMovementsAreCompleted WaitingGroup;
        }

        /// <summary>
        /// A blocker that blocks all elements behind it. The blocker is released when specific movements of another group are completed.
        /// </summary>
        internal class BlockerUntilAnotherGroupsMovementsAreCompleted : WaitingObject
        {
            public int GroupIdThatWaits;
            public int GroupIdToWaitFor;
        }

        /// <summary>
        /// Objects that are waiting for the animation of this group. If AnimationIsRunning == false, the next object can be handled.
        /// </summary>
        private readonly List<WaitingObject> _waitingObjects;

        public int GroupID { get; init; }

        /// <summary>
        /// The count of actually running animations
        /// </summary>
        public int AnimationsRunning { get; set; }

        /// <summary>
        /// Normal case: Add a ScreenObject
        /// </summary>
        /// <param name="screenObject"></param>
        public void AddScreenObject(ScreenObject screenObject)
        {
            _waitingObjects.Add(new WaitingScreenObject() { ScreenObject = screenObject });
        }

        public BlockerUntilAnotherGroupsMovementsAreCompleted AddWaitingForAnotherGroup(int groupIdToWaitFor)
        {
            var waitingMarker = new BlockerUntilAnotherGroupsMovementsAreCompleted() { GroupIdToWaitFor = groupIdToWaitFor, GroupIdThatWaits = GroupID };
            _waitingObjects.Add(waitingMarker);
            return waitingMarker;
        }

        /// <summary>
        /// Special case: Another group is waiting until the current objects of this group are finished
        /// </summary>
        /// <param name="waitingInOtherGroup">An object in the group state of the waiting group. This object is released if all actual objects of 'this' are finished</param>
        public void AddWaitingOtherGroup(BlockerUntilAnotherGroupsMovementsAreCompleted waitingInOtherGroup)
        {
            _waitingObjects.Add(new MarkerForAGroupThatWaitsForCompletion() { WaitingGroup = waitingInOtherGroup });
        }

        public bool HasWaitingObjects()
        {
            return (_waitingObjects.Count > 0);
        }

        public bool HasWaitingScreenObject()
        {
            return (_waitingObjects.Count > 0 && _waitingObjects[0] is WaitingScreenObject);
        }


        public List<BlockerUntilAnotherGroupsMovementsAreCompleted> ExtractLeadingOtherGroupsReadyToRun()
        {
            var blockersInOtherGroups = new List<BlockerUntilAnotherGroupsMovementsAreCompleted>();
            if (AnimationsRunning == 0)
            {
                while (_waitingObjects.Count > 0 && _waitingObjects[0] is MarkerForAGroupThatWaitsForCompletion)
                {
                    blockersInOtherGroups.Add((_waitingObjects[0] as MarkerForAGroupThatWaitsForCompletion).WaitingGroup);
                    _waitingObjects.RemoveAt(0);
                }
            }
            return blockersInOtherGroups;
        }

        public List<ScreenObject> ExtractLeadingScreenObjectsReadyToRun()
        {
            List<ScreenObject> list = new();

            if (AnimationsRunning == 0 && _waitingObjects.Count > 0 && _waitingObjects[0] is WaitingScreenObject)
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

        internal void ExtractBlocker(BlockerUntilAnotherGroupsMovementsAreCompleted blocker)
        {
            var blockerExtracted = _waitingObjects.Remove(blocker);
            if (blockerExtracted)
            {
                Debug.WriteLine($"        Group {blocker.GroupIdThatWaits} is not longer waiting for completed animations of group {blocker.GroupIdToWaitFor}");
            }
        }



        public AnimationGroupState(int groupId)
        {
            AnimationsRunning = 0;
            _waitingObjects = new();
            GroupID = groupId;
        }

    }
}
