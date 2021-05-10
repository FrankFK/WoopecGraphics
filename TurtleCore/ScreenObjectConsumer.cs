using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TurtleCore
{
    internal abstract class WaitingObject
    {
    }

    internal class WaitingScreenObject : WaitingObject
    {
        public ScreenObject ScreenObject;
    }

    internal class WaitingOtherGroup : WaitingObject
    {
        public int OtherGroupId;
    }

    /// <summary>
    /// An instance of this class contains ScreenObjects of a group of objects that have to be drawn one after the other.
    /// </summary>
    internal class AnimationGroupData
    {
        /// <summary>
        /// True if an animation is running at the moment for which WaitingObjects are waiting.
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

        public List<int> ExtractLeadingOtherGroups()
        {
            var otherGroups = new List<int>();
            while (_waitingObjects.Count > 0 && _waitingObjects[0] is WaitingOtherGroup)
            {
                otherGroups.Add((_waitingObjects[0] as WaitingOtherGroup).OtherGroupId);
                _waitingObjects.RemoveAt(0);
            }
            return otherGroups;
        }

        public ScreenObject ExtractLeadingScreenObject()
        {
            ScreenObject screenObject = null;
            if (_waitingObjects.Count > 0 && (_waitingObjects[0] is WaitingScreenObject))
            {
                screenObject = (_waitingObjects[0] as WaitingScreenObject).ScreenObject;
                _waitingObjects.RemoveAt(0);
            }
            return screenObject;
        }

        public AnimationGroupData()
        {
            AnimationIsRunning = false;
            _waitingObjects = new();
        }

        /// <summary>
        /// Objects that are waiting for the animation of this group. If AnimationIsRunning == false, the next object can be handled.
        /// </summary>
        private readonly List<WaitingObject> _waitingObjects;
    }
    internal class ScreenObjectConsumer : IScreenObjectConsumer
    {
        private readonly IScreenObjectWriter _writer;
        private readonly Channel<ScreenObject> _objectChannel;

        private readonly Dictionary<int, AnimationGroupData> _groupsWithActiveAnimations;

        public ScreenObjectConsumer(IScreenObjectWriter writer, Channel<ScreenObject> channel)
        {
            _writer = writer;
            _objectChannel = channel;
            _groupsWithActiveAnimations = new();
        }

        public async Task<ScreenObject> GetNextObjectForWriterAsync()
        {
            // Before reading new objects from the channel, take buffered objects which are not waiting anymore.
            var writableObject = TryToExtractNextNonWaitingBufferedScreenObject();

            // Otherwise search for a directly writeable object in the channel
            while (writableObject == null)
            {
                var screenObject = await _objectChannel.Reader.ReadAsync();

                if (ObjectIsWritable(screenObject))
                {
                    writableObject = screenObject;
                }
                else
                {
                    AddObjectToWaitingState(screenObject);
                }
            }
            return writableObject;
        }

        public void AnimationOfGroupIsFinished(int groupId, int _)
        {
            Console.WriteLine($"Animation of group {groupId} is finished.");
            if (_groupsWithActiveAnimations.ContainsKey(groupId))
            {
                var groupData = _groupsWithActiveAnimations[groupId];
                if (groupData.HasWaitingObjects())
                {
                    Console.WriteLine($"Animation of group {groupId} is finished. Handling waiting objects:");
                    // if an object is waiting for the finished animation, we have to handle it immediately here.
                    // We can not trust that GetNextObjectForWriterAsync() handles it, because GetNextObjectForWriterAsync() may get no further objects.

                    // At first: Inform other groups that are waiting for this group to be finished
                    var otherGroups = groupData.ExtractLeadingOtherGroups();
                    foreach (var otherGroup in otherGroups)
                    {
                        if (_groupsWithActiveAnimations.ContainsKey(otherGroup))
                        {
                            Console.WriteLine($"    Group {otherGroup} is not longer waiting.");
                            _groupsWithActiveAnimations[otherGroup].AnimationIsRunning = false;
                        }
                    }

                    // Then: Look if a ScreenObject of the same group is waiting and animate it
                    if (groupData.HasWaitingObjects())
                    {
                        // Now the first waiting object must be a ScreenObject
                        var screenObject = groupData.ExtractLeadingScreenObject();
                        Console.WriteLine($"    Group has waiting ScreenObject {screenObject.ID}. Starting animation of it.");
                        StartAnimation(screenObject);
                    }
                    else
                    {
                        // Find another ScreenObject that can start its animation (this will be one of otherGroups)
                        var screenObject = TryToExtractNextNonWaitingBufferedScreenObject();
                        if (screenObject != null)
                        {
                            Console.WriteLine($"    Group has no waiting ScreenObjects, but ScreenObject {screenObject.ID} was waiting. Starting animation of it.");
                            StartAnimation(screenObject);
                        }
                        else
                        {
                            Console.WriteLine($"    Group has no waiting ScreenObjects, and no other ScreenObject is waiting.");
                        }
                    }
                }
                if (!groupData.HasWaitingObjects())
                {
                    Console.WriteLine($"Animation of group {groupId} is finished. There are no waiting objects.");
                    _groupsWithActiveAnimations.Remove(groupId);
                }
            }
            else
            {
                Console.WriteLine($"Animation of group {groupId} is finished. But no active animations for this group!");

            }
        }


        public void SendNextObjectToWriter(ScreenObject screenObject)
        {
            if (screenObject.HasAnimation)
            {
                Console.WriteLine($"Consumer: Animated object {screenObject.ID} send to writer");
                StartAnimation(screenObject);
            }
            else
            {
                Console.WriteLine($"Consumer: Unanimated object {screenObject.ID} send to writer");
                _writer.Draw(screenObject);
            }
        }

        private ScreenObject TryToExtractNextNonWaitingBufferedScreenObject()
        {
            var group = _groupsWithActiveAnimations.Values.Where(group => (!group.AnimationIsRunning && group.HasWaitingScreenObject())).FirstOrDefault();
            if (group != null)
            {
                return group.ExtractLeadingScreenObject();
            }
            else
            {
                return null;
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
                if (AnimationsOfGroupAreWaiting(animation.GroupID))
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
                    if (AnimationsOfGroupAreWaiting(groupToWaitFor) || AnAnimationOfGroupIsRunning(groupToWaitFor))
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

        private bool AnimationsOfGroupAreWaiting(int groupID)
        {
            if (_groupsWithActiveAnimations.ContainsKey(groupID) && _groupsWithActiveAnimations[groupID].HasWaitingObjects())
                return true;
            else
                return false;
        }

        private bool AnAnimationOfGroupIsRunning(int groupID)
        {
            if (_groupsWithActiveAnimations.ContainsKey(groupID) && _groupsWithActiveAnimations[groupID].AnimationIsRunning)
                return true;
            else
                return false;
        }

        private void AddObjectToWaitingState(ScreenObject screenObject)
        {
            if (!screenObject.HasAnimation)
                throw new ArgumentException("screenObjects with no animations do not have to wait.");

            var animation = screenObject.Animation;

            // Add this animation to the waiting object of its group:
            if (!_groupsWithActiveAnimations.ContainsKey(animation.GroupID))
            {
                _groupsWithActiveAnimations.Add(animation.GroupID, new AnimationGroupData());
            }
            var activeAnimations = _groupsWithActiveAnimations[animation.GroupID];
            activeAnimations.AddScreenObject(screenObject);

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
                    var otherGroup = animation.WaitForAnimationsOfGroupID;
                    if (_groupsWithActiveAnimations.ContainsKey(otherGroup))
                    {
                        // We have to wait for the other group. To avoid that objects of this group are written. We set this group to
                        // AnimationIsRunning:
                        activeAnimations.AnimationIsRunning = true;

                        // The other group gets a waiting-object, that will set AnimationIsRunning to false, when the other group is finished.
                        _groupsWithActiveAnimations[otherGroup].AddWaitingOtherGroup(animation.GroupID);


                        Console.WriteLine($"Consumer: {screenObject.ID} is waiting for animation of group {otherGroup}.");
                    }
                    else
                    {
                        Console.WriteLine($"Consumer: {screenObject.ID} is waiting for animation of group {otherGroup}. But this group has no active animation!");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Consumer: {screenObject.ID} is waiting (but not for an animation)");
            }
        }

        private void StartAnimation(ScreenObject screenObject)
        {
            var animation = screenObject.Animation;

            if (!_groupsWithActiveAnimations.ContainsKey(animation.GroupID))
                _groupsWithActiveAnimations.Add(animation.GroupID, new AnimationGroupData());

            _groupsWithActiveAnimations[animation.GroupID].AnimationIsRunning = true;

            Console.WriteLine($"Consumer: Animation of {screenObject.ID} is started.");
            _writer.StartAnimaton(screenObject);
        }

    }
}
