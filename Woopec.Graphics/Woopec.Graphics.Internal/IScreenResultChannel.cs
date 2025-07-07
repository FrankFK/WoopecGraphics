using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Woopec.Graphics.LowLevelScreen;

namespace Woopec.Graphics.InternalFrontend
{
    /// <summary>
    /// A channel for exchange of ScreenResults, from the writer's side
    /// </summary>
    internal interface IScreenResultChannelForWriter
    {
        /// <summary>
        /// See same method in System.Threading.Channels.ChannelWriter
        /// </summary>
        /// <param name="screenObject"></param>
        /// <returns></returns>
        public bool TryWrite(ScreenResult screenObject);

    }
}

namespace Woopec.Graphics.InternalBackend
{
    /// <summary>
    /// A channel for exchange of ScreenResults, from the reader's side
    /// </summary>
    internal interface IScreenResultChannelForReader
    {
        /// <summary>
        /// See same method in System.Threading.Channels.ChannelReader
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public ValueTask<ScreenResult> ReadAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
