﻿using System;
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
    internal class ScreenObjectChannelWithinProcess : IScreenObjectChannelForWriter, IScreenObjectChannelForReader
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
