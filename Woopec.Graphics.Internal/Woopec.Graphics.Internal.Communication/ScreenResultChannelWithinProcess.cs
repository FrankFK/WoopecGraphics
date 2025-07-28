using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Woopec.Graphics.Internal.Backend;
using Woopec.Graphics.Internal.Frontend;
using Woopec.Graphics.Interface.Dtos;

namespace Woopec.Graphics.Internal.Communication
{
    internal class ScreenResultChannelWithinProcess : IScreenResultChannelForWriter, IScreenResultChannelForReader
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
