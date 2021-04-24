using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TurtleCore
{
    internal class ChainData
    {
        public bool AnimationIsRunning { get; set; }

        public List<ScreenObject> WaitingObjects { get; set; }

        public ChainData()
        {
            AnimationIsRunning = false;
            WaitingObjects = new();
        }
    }
    internal class ScreenObjectConsumer : IScreenObjectConsumer
    {
        private readonly IScreenObjectWriter _writer;
        private readonly Channel<ScreenObject> _objectChannel;

        private readonly Dictionary<int, ChainData> _chainsWithActiveAnimations;

        public ScreenObjectConsumer(IScreenObjectWriter writer, Channel<ScreenObject> channel)
        {
            _writer = writer;
            _objectChannel = channel;
            _chainsWithActiveAnimations = new();
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
                    if (!_chainsWithActiveAnimations.ContainsKey(animation.ChainID))
                    {
                        // there is no active animation of the same chain.
                        // go for it
                        writableObject = screenObject;
                    }
                    else
                    {
                        var chainData = _chainsWithActiveAnimations[animation.ChainID];
                        if (chainData.WaitingObjects.Count > 0)
                        {
                            // we have to wait until all other waiting objects are finished
                            chainData.WaitingObjects.Add(screenObject);
                        }
                        else
                        {
                            if (chainData.AnimationIsRunning && animation.StartWhenPredecessorHasFinished)
                            {
                                // we have to wait
                                chainData.WaitingObjects.Add(screenObject);
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

        public void AnimationIsFinished(int chainId, Action<int /*chainId*/, int /*objectId*/ > whenFinished)
        {
            if (_chainsWithActiveAnimations.ContainsKey(chainId))
            {
                var chainData = _chainsWithActiveAnimations[chainId];
                if (chainData.WaitingObjects.Count > 0)
                {
                    // if an object is waiting for the finished animation, we have to handle it immediately here.
                    // We can not trust, that GetNextObjectForWriterAsync() because GetNextObjectForWriterAsync() may get no further objects.
                    var screenObject = chainData.WaitingObjects[0];
                    chainData.WaitingObjects.RemoveAt(0);
                    StartAnimation(screenObject, whenFinished);
                }
                else
                {
                    _chainsWithActiveAnimations.Remove(chainId);
                }
            }
        }

        public void SendNextObjectToWriter(ScreenObject screenObject, Action<int, int> whenFinished)
        {
            if (screenObject.HasAnimation)
            {
                Console.WriteLine($"Consumer: Animated object {screenObject.ID} send to writer");
                StartAnimation(screenObject, whenFinished);
            }
            else
            {
                Console.WriteLine($"Consumer: Unanimated object {screenObject.ID} send to writer");
                _writer.Draw(screenObject);
            }
        }

        private ScreenObject GetNextNonWaitingBufferedObject()
        {
            var screenObject = _chainsWithActiveAnimations.Values.Where(chain => (!chain.AnimationIsRunning && chain.WaitingObjects.Count > 0))
                .Select(chainWithObjects => chainWithObjects.WaitingObjects.First()).FirstOrDefault();
            return screenObject;
        }


        private void StartAnimation(ScreenObject screenObject, Action<int, int> whenFinished)
        {
            var animation = screenObject.Animation;

            if (!_chainsWithActiveAnimations.ContainsKey(animation.ChainID))
                _chainsWithActiveAnimations.Add(animation.ChainID, new ChainData());

           _chainsWithActiveAnimations[animation.ChainID].AnimationIsRunning = true;

            _writer.StartAnimaton(screenObject, whenFinished);

        }

    }
}
