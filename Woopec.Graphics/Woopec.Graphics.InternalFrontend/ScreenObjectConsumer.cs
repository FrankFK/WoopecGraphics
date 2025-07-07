using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Woopec.Graphics.InternalCommunicatedObjects;

namespace Woopec.Graphics.InternalFrontend
{
    internal class ScreenObjectConsumer : IScreenObjectConsumer
    {
        private readonly IScreenObjectWriter _writer;
        private readonly IScreenObjectChannelForReader _objectChannel;
        private readonly AnimationGroupsState _animationGroupsState;
        private static readonly object s_lockObj = new();


        public ScreenObjectConsumer(IScreenObjectWriter writer, IScreenObjectChannelForReader channel)
        {
            _writer = writer;
            _objectChannel = channel;
            _animationGroupsState = new();
        }

        private static int _debugCounter1 = 0;
        private static int _debugCounter2 = 0;

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
                    Debug.WriteLine($"Consumer: Read async started ({_debugCounter1})");
                    screenObject = await _objectChannel.ReadAsync();
                    Debug.WriteLine($"Consumer: Read async finished. object from channel {screenObject.AnimationInfoForDebugger()} ({_debugCounter1++})");
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
                        Debug.WriteLine($"Consumer: returning  ({_debugCounter1})");
                        return;
                    }
                    else
                    {
                        AddObjectToWaitingState(screenObject);
                        Debug.WriteLine($"Consumer: next loop  ({_debugCounter1})");
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
                Debug.WriteLine($"Consumer: Animation of group {groupId} is finished. ({++_debugCounter2})");
                if (_animationGroupsState.TryGetGroupState(groupId, out var groupState))
                {
                    groupState.AnimationsRunning--;
                    if (groupState.HasWaitingObjects())
                    {
                        // if an object is waiting for the finished animation, we have to handle it immediately here.
                        // We can not trust that GetNextObjectForWriterAsync() handles it, because GetNextObjectForWriterAsync() may get no further objects.
                        Debug.WriteLine($"Consumer: Animation of group {groupId} is finished. Handling waiting objects:");

                        SendAllObjectsThatAreReadyToRunToWriter(groupState);
                    }
                    else
                    {
                        Debug.WriteLine($"Consumer: Animation of group {groupId} is finished. But it has no waiting objects!");
                    }
                }
                else
                {
                    Debug.WriteLine($"Consumer: Animation of group {groupId} is finished. But no active animations for this group!");
                }
                Debug.WriteLine($"Consumer: Animation of group {groupId} is handled. ({_debugCounter2})");
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
                var blockersInOtherGroups = groupState.ExtractLeadingOtherGroupsReadyToRun();
                if (blockersInOtherGroups.Count > 0)
                {
                    Debug.WriteLine($"    Group {groupState.GroupID}: Unblocking {blockersInOtherGroups.Count} groups that are waiting for completed movements.");
                }
                _animationGroupsState.RemoveBlockersInOtherGroups(blockersInOtherGroups);

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

                changesFound = (blockersInOtherGroups.Count != 0 || noLongerWaitingScreenObjects.Count != 0);
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
            var screenObjectGroup = screenObject.GroupID;
            var anotherGroupToWaitFor = screenObject.WaitForCompletedAnimationsOfAnotherGroup;
            if (_animationGroupsState.AnimationsOfGroupAreWaiting(screenObjectGroup))
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
                if (screenObject.WaitForCompletedAnimationsOfSameGroup
                    && (_animationGroupsState.AnimationsOfGroupAreWaiting(screenObjectGroup) || _animationGroupsState.AnAnimationOfGroupIsRunning(screenObjectGroup)))
                {
                    return false;
                }
                else if (anotherGroupToWaitFor != ScreenObject.NoGroupId
                         && (_animationGroupsState.AnimationsOfGroupAreWaiting(anotherGroupToWaitFor) || _animationGroupsState.AnAnimationOfGroupIsRunning(anotherGroupToWaitFor)))
                {
                    return false;
                }
                else
                {
                    // All animations of screenObjectGroup and anotherGroupToWaitFor are done. Waiting for these groups make no sense.
                    // go for it
                    return true;
                }
            }
        }


        private void AddObjectToWaitingState(ScreenObject screenObject)
        {
            if (screenObject.WaitsForAnimations)
            {
                if (screenObject.WaitForCompletedAnimationsOfAnotherGroup != ScreenObject.NoGroupId)
                {
                    // Tricky case: This animation waits for a different group to finish all of its current animations
                    _animationGroupsState.AddWaitingStateBetweenTwoGroups(screenObject.GroupID, screenObject.WaitForCompletedAnimationsOfAnotherGroup);
                }

                if (screenObject.WaitForCompletedAnimationsOfSameGroup)
                {
                    // Easy case: When all previous animations of this group are finished this animation will be written
                    Debug.WriteLine($"Consumer: Object {screenObject.ID} of group {screenObject.GroupID} is waiting for an animation of its group");
                }

                _animationGroupsState.AddWaitingScreenObject(screenObject);
            }
            else
            {
                Debug.WriteLine($"Consumer: Object {screenObject.ID} of group {screenObject.GroupID} is waiting (because other objects of its group are waiting)");
                _animationGroupsState.AddWaitingScreenObject(screenObject);
            }
        }


    }
}
