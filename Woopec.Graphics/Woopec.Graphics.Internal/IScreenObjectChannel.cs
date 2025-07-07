using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Woopec.Graphics.LowLevelScreen;

namespace Woopec.Graphics.InternalBackend
{
    /// <summary>
    /// Communication channel for ScreenObjectProducer
    /// </summary>
    internal interface IScreenObjectChannelForWriter
    {
        /// <summary>
        /// See same method in System.Threading.Channels.ChannelWriter
        /// </summary>
        /// <param name="screenObject"></param>
        /// <returns></returns>
        public bool TryWrite(ScreenObject screenObject);

    }

}

namespace Woopec.Graphics.InternalFrontend
{
    /// <summary>
    /// Communication channel for ScreenObjectConsumer
    /// </summary>
    internal interface IScreenObjectChannelForReader
    {
        /// <summary>
        /// See same method in System.Threading.Channels.ChannelReader
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public ValueTask<ScreenObject> ReadAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
