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
                // Before reading new objects from the channel, take buffered objects which are not waiting anymore.
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

        public void AnimationOfGroupIsFinished(int groupId, int _)
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

                        // At first: Inform other groups that are waiting for this group to be finished
                        var otherGroups = groupState.ExtractLeadingOtherGroups();
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
            if (screenObject.HasAnimation)
            {
                var animation = screenObject.Animation;

                _animationGroupsState.AddRunningAnimation(animation.GroupID);

                Console.WriteLine($"Consumer: Animated object {screenObject.ID} sent to writer");
                _writer.StartAnimaton(screenObject);
            }
            else
            {
                Console.WriteLine($"Consumer: Unanimated object {screenObject.ID} sent to writer");
                _writer.Draw(screenObject);
            }
        }

        /// <summary>
        /// Return true if the given object is directly writable and has not to wait for others
        /// </summary>
        /// <param name="screenObject"></param>
        /// <returns></returns>
        private bool ObjectIsWritable(ScreenObject screenObject)
        {
            if (!screenObject.HasAnimation)
            {
                // we do not care if there are any screen objects waiting for finished animations.
                // screenObject waits for nothing, it can be written immediately
                return true;
            }
            else
            {
                var animation = screenObject.Animation;
                if (_animationGroupsState.AnimationsOfGroupAreWaiting(animation.GroupID))
                {
                    // There are other objects of this group waiting. We can not write the screenObject because we would change the order of the screenObjects
                    // of this group
                    return false;
                }
                if (!animation.WaitForAnimations)
                {
                    // Animation does not wait for other animations. Go for it.
                    return true;
                }
                else
                {
                    // We have to wait for another animation. Check that group
                    var groupToWaitFor = animation.WaitForAnimationsOfGroupID;
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
        }


        private void AddObjectToWaitingState(ScreenObject screenObject)
        {
            if (!screenObject.HasAnimation)
                throw new ArgumentException("screenObjects with no animations do not have to wait.");

            _animationGroupsState.AddWaitingScreenObject(screenObject);

            var animation = screenObject.Animation;
            if (animation.WaitForAnimations)
            {
                if (animation.WaitForAnimationsOfGroupID == animation.GroupID)
                {
                    // Easy case: When all previous animations of this group are finished this animation will be written
                    Console.WriteLine($"Consumer: {screenObject.ID} is waiting for animation of the same groupID");
                }
                else
                {
                    // Tricky case: This animation waits for a different group to finish all of its current animations
                    _animationGroupsState.AddWaitingStateBetweenTwoGroups(animation.GroupID, animation.WaitForAnimationsOfGroupID);
                }
            }
            else
            {
                Console.WriteLine($"Consumer: {screenObject.ID} is waiting (but not for an animation)");
            }
        }


    }
}
