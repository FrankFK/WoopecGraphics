using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Woopec.Core.Internal
{
    /// <summary>
    /// An instance of this class transports ScreenResults from the screen to the consumers
    /// </summary>
    internal class ScreenResultBroker
    {
        public IScreenResultChannel ScreenResultChannel { get; init; }


        /// <summary>
        /// Broker for the case where producers and consumers are in the same process
        /// </summary>
        /// <param name="capacity"></param>
        public ScreenResultBroker(int capacity)
        {
            var channelOptions = new BoundedChannelOptions(capacity) { SingleReader = true, SingleWriter = true};
            ScreenResultChannel = new ScreenResultChannelWithinProcess(channelOptions);
        }
    }
}
