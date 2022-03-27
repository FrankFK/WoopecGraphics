using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Woopec.Core.Internal
{
    /// <summary>
    /// An instance of this class transports ScreenObjects from (one or many) producers to (one) consumer
    /// </summary>
    internal class ScreenObjectBroker
    {
        public IChannel ObjectChannel { get; init; }

        public IScreenObjectConsumer Consumer { get; init; }


        public ScreenObjectBroker(IScreenObjectWriter writer, int capacity)
        {
            var channelOptions = new BoundedChannelOptions(capacity) { SingleReader = true, SingleWriter = false };
            ObjectChannel = new ChannelBetweenThreads(channelOptions); 
            var screenObjectConsumer = new ScreenObjectConsumer(writer, ObjectChannel);
            Consumer = screenObjectConsumer;
            writer.OnAnimationIsFinished += screenObjectConsumer.AnimationOfGroupIsFinished;
        }

    }
}
