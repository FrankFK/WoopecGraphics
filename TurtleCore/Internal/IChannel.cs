using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Woopec.Core.Internal
{
    /// <summary>
    /// Communication between ScreenObjectProducer and ScreenObjectConsumer
    /// </summary>
    internal interface IChannel
    {
        /// <summary>
        /// See same method in System.Threading.Channels.ChannelWriter
        /// </summary>
        /// <param name="screenObject"></param>
        /// <returns></returns>
        public bool TryWrite(ScreenObject screenObject);

        /// <summary>
        /// See same method in System.Threading.Channels.ChannelReader
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public ValueTask<ScreenObject> ReadAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
