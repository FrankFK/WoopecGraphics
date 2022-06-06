using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Woopec.Core.Internal
{
    internal class ScreenObjectConsumer : IScreenObjectConsumer
    {
        private readonly IScreenObjectWriter _writer;
        private readonly IScreenObjectChannel _objectChannel;
        private readonly AnimationGroupsState _animationGroupsState;
        private static readonly object s_lockObj = new();


        public ScreenObjectConsumer(IScreenObjectWriter writer, IScreenObjectChannel channel)
        {
            _writer = writer;
            _objectChannel = channel;
            _animationGroupsState = new();
        }

        /// <summary>
        /// Return a task that waits for the next ScreenObject.
        /// This task can be "plugged" into the Main-Loop of the actual thread (UI thread of WPF for instance)
        /// </summary>
        /// <returns></returns>
        public async Task HandleNextScreenObjectAsync()
        {
            while (true)
            {
                ScreenObject screenObject = null;
                try
                {
                    screenObject = await _objectChannel.ReadAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Consumer: Exception " + ex.Message);
                    throw;
                }

                lock (s_lockObj)
                {
                    if (ObjectIsWritable(screenObject))
                    {
                        SendNextObjectToWriter(screenObject);
                        return;
                    }
                    else
                    {
                        AddObjectToWaitingState(screenObject);
                    }
                }
            }
        }

        /// <summary>
        /// Animation of an object is finished. Check if other objects are waiting for that and
        /// update these objects.
        /// </summary>
        /// <param name="groupId">The groupId of the finished screen object</param>
        /// <param name="screenObjectId">the Id of the finished screen object</param>
        public void AnimationOfGroupIsFinished(int groupId, int screenObjectId)
        {
            lock (s_lockObj)
            {
                Debug.WriteLine($"Animation of group {groupId} is finished.");
                if (_animationGroupsState.TryGetGroupState(groupId, out var groupState))
                {
                    groupState.AnimationsRunning--;
                    if (groupState.HasWaitingObjects())
                    {
                        // if an object is waiting for the finished animation, we have to handle it immediately here.
                        // We can not trust that GetNextObjectForWriterAsync() handles it, because GetNextObjectForWriterAsync() may get no further objects.
                        Debug.WriteLine($"Animation of group {groupId} is finished. Handling waiting objects:");

                        SendAllObjectsThatAreReadyToRunToWriter(groupState);
                    }
                }
                else
                {
                    Debug.WriteLine($"Animation of group {groupId} is finished. But no active animations for this group!");
                }
            }
        }

        /// <summary>
        /// Find out (recursively) all screen objects that wait for the animation of the given group and send them to the writer
        /// </summary>
        /// <param name="groupState">State of the group whose animation has ended</param>
        private void SendAllObjectsThatAreReadyToRunToWriter(AnimationGroupState groupState)
        {
            // With one run through this loop many states are changed. It is possible that further objects are not waiting after the first
            // run through the loop. Therefore we loop until no changes were found.
            var changesFound = true;
            while (changesFound)
            {
                // At first: Inform other groups that are waiting for this group to be finished
                var otherGroups = groupState.ExtractLeadingOtherGroupsReadyToRun();
                if (otherGroups.Count > 0)
                {
                    Debug.WriteLine($"    Group {groupState.GroupID}: Following waiting groups can start: {string.Join(", ", otherGroups)}.");
                }
                _animationGroupsState.DecrementAnimationsRunning(otherGroups);

                // Then: Collect all non longer waiting Screen objects and start them
                var noLongerWaitingScreenObjects = _animationGroupsState.ExtractLeadingScreenObjectsReadyToRun();
                if (noLongerWaitingScreenObjects.Count == 0)
                {
                    Debug.WriteLine($"    Group {groupState.GroupID} has no waiting ScreenObjects, and no other ScreenObject is waiting.");
                }
                else
                {
                    foreach (var screenObject in noLongerWaitingScreenObjects)
                    {
                        SendNextObjectToWriter(screenObject);
                    }
                }

                changesFound = (otherGroups.Count != 0 || noLongerWaitingScreenObjects.Count != 0);
            }
        }

        private void SendNextObjectToWriter(ScreenObject screenObject)
        {
            if (screenObject.HasAnimations)
            {
                _animationGroupsState.AddRunningAnimation(screenObject.GroupID);

                Debug.WriteLine($"Consumer: Group {screenObject.GroupID},   animated object {screenObject.ID} sent to writer");
                _writer.UpdateWithAnimation(screenObject);
            }
            else
            {
                Debug.WriteLine($"Consumer: Group {screenObject.GroupID}, unanimated object {screenObject.ID} sent to writer");
                _writer.Update(screenObject);
            }
        }

        /// <summary>
        /// Return true if the given object is directly writable and has not to wait for others
        /// </summary>
        /// <param name="screenObject"></param>
        /// <returns></returns>
        private bool ObjectIsWritable(ScreenObject screenObject)
        {
            if (_animationGroupsState.AnimationsOfGroupAreWaiting(screenObject.GroupID))
            {
                // There are other objects of this group waiting. We can not write the screenObject because we would change the order of the screenObjects
                // of this group
                return false;
            }
            if (!screenObject.WaitsForAnimations)
            {
                // Animation does not wait for other animations. Go for it.
                return true;
            }
            else
            {
                // We have to wait for another animation. Check that group
                var groupToWaitFor = screenObject.WaitForAnimationsOfGroupID;
                if (_animationGroupsState.AnimationsOfGroupAreWaiting(groupToWaitFor) || _animationGroupsState.AnAnimationOfGroupIsRunning(groupToWaitFor))
                {
                    return false;
                }
                else
                {
                    // there is no active animation of the this group.
                    // go for it
                    return true;
                }
            }
        }


        private void AddObjectToWaitingState(ScreenObject screenObject)
        {
            _animationGroupsState.AddWaitingScreenObject(screenObject);

            if (screenObject.WaitsForAnimations)
            {
                if (screenObject.WaitForAnimationsOfGroupID == screenObject.GroupID)
                {
                    // Easy case: When all previous animations of this group are finished this animation will be written
                    Debug.WriteLine($"Consumer: Object {screenObject.ID} of group {screenObject.GroupID} is waiting for an animation its group");
                }
                else
                {
                    // Tricky case: This animation waits for a different group to finish all of its current animations
                    _animationGroupsState.AddWaitingStateBetweenTwoGroups(screenObject.GroupID, screenObject.WaitForAnimationsOfGroupID);
                }
            }
            else
            {
                Debug.WriteLine($"Consumer: Object {screenObject.ID} of group {screenObject.GroupID} is waiting (but not for an animation)");
            }
        }


    }
}
