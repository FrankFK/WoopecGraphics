using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Graphics.Interface.Dtos;

namespace Woopec.Graphics.Internal.Frontend
{
    /// <summary>
    /// An instance of this class describes the actually known state of all an animation groups.
    /// </summary>
    internal class AnimationGroupsState
    {
        private readonly Dictionary<int, AnimationGroupState> _groupsWithActiveAnimations;

        public AnimationGroupsState()
        {
            _groupsWithActiveAnimations = new();
        }

        public void AddRunningAnimation(int groupId)
        {
            if (!_groupsWithActiveAnimations.ContainsKey(groupId))
                _groupsWithActiveAnimations.Add(groupId, new AnimationGroupState(groupId));

            _groupsWithActiveAnimations[groupId].AnimationsRunning++;
        }

        public void AddWaitingScreenObject(ScreenObject screenObject)
        {
            if (!_groupsWithActiveAnimations.ContainsKey(screenObject.GroupID))
            {
                _groupsWithActiveAnimations.Add(screenObject.GroupID, new AnimationGroupState(screenObject.GroupID));
            }
            var activeAnimations = _groupsWithActiveAnimations[screenObject.GroupID];
            activeAnimations.AddScreenObject(screenObject);
        }

        public void AddWaitingStateBetweenTwoGroups(int groupIdThatWaits, int groupIdToWaitFor)
        {
            if (TryGetGroupState(groupIdToWaitFor, out var stateOfGroupToWaitFor))
            {
                // We have to wait for the other group. To avoid that objects of this group are written, we add a specific
                // wait-object to this group:
                var stateOfGroupThatWaits = GetGroupState(groupIdThatWaits);
                var waitObject = stateOfGroupThatWaits.AddWaitingForAnotherGroup(groupIdToWaitFor);

                // To the other group we add an object, that will inform the waitObject, when the other group is finished.
                stateOfGroupToWaitFor.AddWaitingOtherGroup(waitObject);
                Debug.WriteLine($"Consumer: Group {groupIdThatWaits} is waiting for animation of group {groupIdToWaitFor}.");
            }
            else
            {
                Debug.WriteLine($"Consumer: Group {groupIdThatWaits} is waiting for animation of group {groupIdToWaitFor}. But group {groupIdToWaitFor} has no active animation");
            }
        }

        public bool TryGetGroupState(int groupID, out AnimationGroupState groupState)
        {
            if (_groupsWithActiveAnimations.ContainsKey(groupID))
            {
                groupState = _groupsWithActiveAnimations[groupID];
                return true;
            }
            else
            {
                groupState = null;
                return false;
            }
        }
        public AnimationGroupState GetGroupState(int groupID)
        {
            if (!_groupsWithActiveAnimations.ContainsKey(groupID))
            {
                _groupsWithActiveAnimations.Add(groupID, new AnimationGroupState(groupID));
            }
            return _groupsWithActiveAnimations[groupID];
        }


        public void RemoveBlockersInOtherGroups(List<AnimationGroupState.BlockerUntilAnotherGroupsMovementsAreCompleted> blockers)
        {
            foreach (var blocker in blockers)
            {
                if (_groupsWithActiveAnimations.ContainsKey(blocker.GroupIdThatWaits))
                {
                    _groupsWithActiveAnimations[blocker.GroupIdThatWaits].ExtractBlocker(blocker);
                }
            }
        }

        public bool AnimationsOfGroupAreWaiting(int groupID)
        {
            if (_groupsWithActiveAnimations.ContainsKey(groupID) && _groupsWithActiveAnimations[groupID].HasWaitingObjects())
                return true;
            else
                return false;
        }

        public bool AnAnimationOfGroupIsRunning(int groupID)
        {
            if (_groupsWithActiveAnimations.ContainsKey(groupID) && (_groupsWithActiveAnimations[groupID].AnimationsRunning > 0))
                return true;
            else
                return false;
        }


        public List<ScreenObject> ExtractLeadingScreenObjectsReadyToRun()
        {
            var list = new List<ScreenObject>();

            var groups = _groupsWithActiveAnimations.Values.Where(group => (group.HasWaitingScreenObject()));
            foreach (var group in groups)
            {
                list.AddRange(group.ExtractLeadingScreenObjectsReadyToRun());
            }

            return list;
        }


    }
}
