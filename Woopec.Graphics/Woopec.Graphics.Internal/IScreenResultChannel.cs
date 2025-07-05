using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Woopec.Graphics.Internal
{
    /// <summary>
    /// A channel for exchange of ScreenResults
    /// </summary>
    internal interface IScreenResultChannel
    {
        /// <summary>
        /// See same method in System.Threading.Channels.ChannelWriter
        /// </summary>
        /// <param name="screenObject"></param>
        /// <returns></returns>
        public bool TryWrite(ScreenResult screenObject);

        /// <summary>
        /// See same method in System.Threading.Channels.ChannelReader
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public ValueTask<ScreenResult> ReadAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
