using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TurtleCore
{
    internal class AnimationGroupData
    {
        public bool AnimationIsRunning { get; set; }

        public List<ScreenObject> WaitingObjects { get; set; }

        public AnimationGroupData()
        {
            AnimationIsRunning = false;
            WaitingObjects = new();
        }
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
            var writableObject = GetNextNonWaitingBufferedObject();

            // Otherwise search for a directly writeable object in the channel
            while (writableObject == null)
            {
                var screenObject = await _objectChannel.Reader.ReadAsync();

                if (!screenObject.HasAnimation)
                {
                    // we do not care if there are any screen objects waiting for finished animations.
                    // screenObject waits for nothing, it can be written immediately
                    writableObject = screenObject;
                }
                else
                {
                    var animation = screenObject.Animation;
                    if (!_groupsWithActiveAnimations.ContainsKey(animation.GroupID))
                    {
                        // there is no active animation of the same group.
                        // go for it
                        writableObject = screenObject;
                    }
                    else
                    {
                        var groupData = _groupsWithActiveAnimations[animation.GroupID];
                        if (groupData.WaitingObjects.Count > 0)
                        {
                            // we have to wait until all other waiting objects are finished
                            groupData.WaitingObjects.Add(screenObject);
                        }
                        else
                        {
                            if (groupData.AnimationIsRunning && animation.StartWhenPredecessorHasFinished)
                            {
                                // we have to wait
                                groupData.WaitingObjects.Add(screenObject);
                            }
                            else
                            {
                                // go for it
                                writableObject = screenObject;
                            }
                        }
                    }
                }
            }

            return writableObject;
        }

        public void AnimationOfGroupIsFinished(int groupId, int _)
        {
            if (_groupsWithActiveAnimations.ContainsKey(groupId))
            {
                var groupData = _groupsWithActiveAnimations[groupId];
                if (groupData.WaitingObjects.Count > 0)
                {
                    // if an object is waiting for the finished animation, we have to handle it immediately here.
                    // We can not trust, that GetNextObjectForWriterAsync() because GetNextObjectForWriterAsync() may get no further objects.
                    var screenObject = groupData.WaitingObjects[0];
                    groupData.WaitingObjects.RemoveAt(0);
                    StartAnimation(screenObject);
                }
                else
                {
                    _groupsWithActiveAnimations.Remove(groupId);
                }
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

        private ScreenObject GetNextNonWaitingBufferedObject()
        {
            var screenObject = _groupsWithActiveAnimations.Values.Where(group => (!group.AnimationIsRunning && group.WaitingObjects.Count > 0))
                .Select(groupWithObjects => groupWithObjects.WaitingObjects.First()).FirstOrDefault();
            return screenObject;
        }


        private void StartAnimation(ScreenObject screenObject)
        {
            var animation = screenObject.Animation;

            if (!_groupsWithActiveAnimations.ContainsKey(animation.GroupID))
                _groupsWithActiveAnimations.Add(animation.GroupID, new AnimationGroupData());

            _groupsWithActiveAnimations[animation.GroupID].AnimationIsRunning = true;

            _writer.StartAnimaton(screenObject);

        }

    }
}
