using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Woopec.Core.Internal
{
    internal class ScreenResultChannelWithinProcess : IScreenResultChannel
    {
        private readonly Channel<ScreenResult> _channel;

        public ScreenResultChannelWithinProcess(BoundedChannelOptions options)
        {
            _channel = Channel.CreateBounded<ScreenResult>(options);
        }
        public ValueTask<ScreenResult> ReadAsync(CancellationToken cancellationToken = default) => _channel.Reader.ReadAsync(cancellationToken);

        public bool TryWrite(ScreenResult screenResult) => _channel.Writer.TryWrite(screenResult);
    }
}
