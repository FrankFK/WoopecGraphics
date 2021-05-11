﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore.Internal
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

            _groupsWithActiveAnimations[groupId].AnimationIsRunning = true;
        }

        public void AddWaitingScreenObject(ScreenObject screenObject)
        {
            var animation = screenObject.Animation;

            if (!_groupsWithActiveAnimations.ContainsKey(animation.GroupID))
            {
                _groupsWithActiveAnimations.Add(animation.GroupID, new AnimationGroupState(animation.GroupID));
            }
            var activeAnimations = _groupsWithActiveAnimations[animation.GroupID];
            activeAnimations.AddScreenObject(screenObject);
        }

        public void AddWaitingStateBetweenTwoGroups(int groupIdThatWaits, int groupIdToWaitFor)
        {
            if (TryGetGroupState(groupIdToWaitFor, out var stateOfGroupToWaitFor))
            {
                // We have to wait for the other group. To avoid that objects of this group are written. We set this group to
                // AnimationIsRunning:
                if (TryGetGroupState(groupIdThatWaits, out var stateOfGroupThatWaits))
                {
                    stateOfGroupThatWaits.AnimationIsRunning = true;
                }

                // The other group gets a waiting-object, that will set AnimationIsRunning to false, when the other group is finished.
                stateOfGroupToWaitFor.AddWaitingOtherGroup(groupIdThatWaits);
                Console.WriteLine($"Consumer: {groupIdThatWaits} is waiting for animation of group {groupIdToWaitFor}.");
            }
            else
            {
                Console.WriteLine($"Consumer: Group {groupIdThatWaits} is waiting for animation of group {groupIdToWaitFor}. But this group has no active animation");
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

        public void SetAnimationIsRunning(List<int> groupIDs, bool state)
        {
            foreach (var groupID in groupIDs)
            {
                if (_groupsWithActiveAnimations.ContainsKey(groupID))
                {
                    Console.WriteLine($"    Group {groupID} is not longer waiting.");
                    _groupsWithActiveAnimations[groupID].AnimationIsRunning = state;
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
            if (_groupsWithActiveAnimations.ContainsKey(groupID) && _groupsWithActiveAnimations[groupID].AnimationIsRunning)
                return true;
            else
                return false;
        }


        public List<ScreenObject> ExtractAllNonWaitingScreenObjects()
        {
            var list = new List<ScreenObject>();

            var groups = _groupsWithActiveAnimations.Values.Where(group => (!group.AnimationIsRunning && group.HasWaitingScreenObject()));
            foreach (var group in groups)
            {
                var screenObject = group.ExtractLeadingScreenObject();
                if (screenObject != null)
                {
                    list.Add(screenObject);
                }
            }

            return list;
        }

        public ScreenObject ExtractOneNonWaitingScreenObject()
        {
            ScreenObject found = null;

            var group = _groupsWithActiveAnimations.Values.Where(group => (!group.AnimationIsRunning && group.HasWaitingScreenObject())).FirstOrDefault();
            if (group != null)
            {
                found = group.ExtractLeadingScreenObject();
            }

            return found;
        }

    }
}