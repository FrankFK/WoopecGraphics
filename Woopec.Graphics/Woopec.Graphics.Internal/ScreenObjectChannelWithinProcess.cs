using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Woopec.Graphics.Internal
{
    internal class ScreenObjectChannelWithinProcess : IScreenObjectChannel
    {
        private readonly Channel<ScreenObject> _channel;

        public ScreenObjectChannelWithinProcess(BoundedChannelOptions options)
        {
            _channel = Channel.CreateBounded<ScreenObject>(options);
        }
        public ValueTask<ScreenObject> ReadAsync(CancellationToken cancellationToken = default) => _channel.Reader.ReadAsync(cancellationToken);

        public bool TryWrite(ScreenObject screenObject) => _channel.Writer.TryWrite(screenObject);
    }
}
