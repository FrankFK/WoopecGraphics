using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Woopec.Graphics.InternalObjects;

namespace Woopec.Graphics.Internal
{
    internal class ScreenResultChannelInClientProcess : IScreenResultChannel
    {
        private static readonly JsonSerializerOptions _options = ScreenResultChannelInServerProcess.SerializerOptions();
        private readonly AnonymousPipeClientStream _clientStream;
        private readonly StreamWriter _streamWriter;

        public ScreenResultChannelInClientProcess(string handle)
        {
            _clientStream = new AnonymousPipeClientStream(PipeDirection.Out, handle);
            _streamWriter = new StreamWriter(_clientStream);
        }

        public ValueTask<ScreenResult> ReadAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("ScreenResultChannelInClientProcess does not need a read-method because it is only writing");
            // If we need a read method this method should work like ClientProcessChannel.ReadAsync
        }

        public bool TryWrite(ScreenResult screenResult)
        {
            try
            {
                string serialization = JsonSerializer.Serialize(screenResult, _options);
                _streamWriter.WriteLine(serialization);
                _streamWriter.Flush();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
