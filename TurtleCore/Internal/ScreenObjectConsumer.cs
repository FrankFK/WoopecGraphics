using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TurtleCore.Internal
{
    internal class ScreenObjectConsumer : IScreenObjectConsumer
    {
        private readonly IScreenObjectWriter _writer;
        private readonly Channel<ScreenObject> _objectChannel;
        private readonly AnimationGroupsState _animationGroupsState;
        private static readonly object s_lockObj = new();


        public ScreenObjectConsumer(IScreenObjectWriter writer, Channel<ScreenObject> channel)
        {
            _writer = writer;
            _objectChannel = channel;
            _animationGroupsState = new();
        }

        public async Task HandleNextScreenObjectAsync()
        {
            lock (s_lockObj)
            {
                // At first: Inform other groups that are ready to run
                var otherGroups = _animationGroupsState.ExtractLeadingOtherGroupsReadyToRun();
                _animationGroupsState.SetAnimationIsRunning(otherGroups, false);

                // Before reading new objects from the channel, take buffered objects which are not waiting anymore.
                // (But i assume that this situation will never arise because AnimationOfGroupIsFinished has called ExtractAllNonWaitingScreenObjects() before)
                var writableObject = _animationGroupsState.ExtractOneNonWaitingScreenObject();
                if (writableObject != null)
                {
                    SendNextObjectToWriter(writableObject);
                    return;
                }
            }

            // Otherwise search for a directly writeable object in the channel
            while (true)
            {
                var screenObject = await _objectChannel.Reader.ReadAsync();

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

        public void AnimationOfGroupIsFinished(int groupId, int screenObjectId)
        {
            lock (s_lockObj)
            {
                Console.WriteLine($"Animation of group {groupId} is finished.");
                if (_animationGroupsState.TryGetGroupState(groupId, out var groupState))
                {
                    groupState.AnimationIsRunning = false;
                    if (groupState.HasWaitingObjects())
                    {
                        // if an object is waiting for the finished animation, we have to handle it immediately here.
                        // We can not trust that GetNextObjectForWriterAsync() handles it, because GetNextObjectForWriterAsync() may get no further objects.
                        Console.WriteLine($"Animation of group {groupId} is finished. Handling waiting objects:");

                        // With one run through this loop many states are changed. It is possible that further objects are not waiting after the first
                        // run through the loop. Therefore we loop until no changes were found.
                        var changesFound = true;
                        while (changesFound)
                        {
                            // At first: Inform other groups that are waiting for this group to be finished
                            var otherGroups = groupState.ExtractLeadingOtherGroupsReadyToRun();
                            _animationGroupsState.SetAnimationIsRunning(otherGroups, false);

                            // Then: Collect all non longer waiting Screen objects and start them
                            var noLongerWaitingScreenObjects = _animationGroupsState.ExtractAllNonWaitingScreenObjects();
                            if (noLongerWaitingScreenObjects.Count == 0)
                            {
                                Console.WriteLine($"    Group has no waiting ScreenObjects, and no other ScreenObject is waiting.");
                            }
                            else
                            {
                                foreach (var screenObject in noLongerWaitingScreenObjects)
                                {
                                    Console.WriteLine($"    Group has waiting ScreenObject {screenObject.ID}. Starting animation of it.");
                                    SendNextObjectToWriter(screenObject);
                                }
                            }

                            changesFound = (otherGroups.Count != 0 || noLongerWaitingScreenObjects.Count != 0);
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Animation of group {groupId} is finished. But no active animations for this group!");
                }
            }
        }


        private void SendNextObjectToWriter(ScreenObject screenObject)
        {
            if (screenObject.HasAnimations)
            {
                _animationGroupsState.AddRunningAnimation(screenObject.GroupID);

                Console.WriteLine($"Consumer: Animated object {screenObject.ID} sent to writer");
                _writer.UpdateWithAnimation(screenObject);
            }
            else
            {
                Console.WriteLine($"Consumer: Unanimated object {screenObject.ID} sent to writer");
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
                    Console.WriteLine($"Consumer: {screenObject.ID} is waiting for animation of the same groupID");
                }
                else
                {
                    // Tricky case: This animation waits for a different group to finish all of its current animations
                    _animationGroupsState.AddWaitingStateBetweenTwoGroups(screenObject.GroupID, screenObject.WaitForAnimationsOfGroupID);
                }
            }
            else
            {
                Console.WriteLine($"Consumer: {screenObject.ID} is waiting (but not for an animation)");
            }
        }


    }
}
