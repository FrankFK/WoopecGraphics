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
using Woopec.Graphics.InternalBackend;
using Woopec.Graphics.InternalFrontend;
using Woopec.Graphics.LowLevelScreen;

namespace Woopec.Graphics.Internal
{
    internal class ScreenObjectChannelInClientProcess : IScreenObjectChannelForWriter, IScreenObjectChannelForReader
    {
        private static readonly JsonSerializerOptions _options = ScreenObjectChannelInServerProcess.SerializerOptions();
        private readonly AnonymousPipeClientStream _clientStream;
        private readonly StreamReader _streamReader;

        public ScreenObjectChannelInClientProcess(string handle)
        {
            _clientStream = new AnonymousPipeClientStream(PipeDirection.In, handle);
            _streamReader = new StreamReader(_clientStream);
        }

        public async ValueTask<ScreenObject> ReadAsync(CancellationToken cancellationToken = default)
        {
            var serialization = await _streamReader.ReadLineAsync();
            if (!_clientStream.IsConnected)
            {
                // High probability that the process which produces the screen objects is terminated
                Environment.Exit(43);
            }
            var screenObject = JsonSerializer.Deserialize<ScreenObject>(serialization, _options);
            return screenObject;
        }

        public bool TryWrite(ScreenObject screenObject)
        {
            throw new NotImplementedException("ClientProcessChannel does not need a read-method because it is only reading");
            // If we need a write method this method should work like ServerProcessChannel.TryWrite
        }

    }
}
