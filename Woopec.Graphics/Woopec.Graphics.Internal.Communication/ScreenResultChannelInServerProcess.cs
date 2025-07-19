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
using Woopec.Graphics.Internal.Backend;
using Woopec.Graphics.Internal.Frontend;
using Woopec.Graphics.Interface.Dtos;

namespace Woopec.Graphics.InternalCommunication
{
    internal class ScreenResultChannelInServerProcess : IScreenResultChannelForWriter, IScreenResultChannelForReader
    {
        private static readonly JsonSerializerOptions _options = InitOptions();
        private readonly AnonymousPipeServerStream _serverStream;
        private readonly StreamReader _streamReader;

        public string Handle { get; init; }

        public ScreenResultChannelInServerProcess()
        {
            _serverStream = new AnonymousPipeServerStream(PipeDirection.In, HandleInheritability.Inheritable);
            _streamReader = new StreamReader(_serverStream);
            Handle = _serverStream.GetClientHandleAsString();
        }

        public void DisposeHandle()
        {
            _serverStream.DisposeLocalCopyOfClientHandle();
        }

        public static JsonSerializerOptions SerializerOptions()
        {
            return _options;
        }

        public async ValueTask<ScreenResult> ReadAsync(CancellationToken cancellationToken = default)
        {
            var serialization = await _streamReader.ReadLineAsync();
            if (!_serverStream.IsConnected)
            {
                // High probability that the process which produces the screen results is terminated
                Environment.Exit(43);
            }
            var screenResult = JsonSerializer.Deserialize<ScreenResult>(serialization, _options);
            return screenResult;
        }

        public bool TryWrite(ScreenResult screenResult)
        {
            throw new NotImplementedException("ScreenResultChannelInServerProcess does not need a write-method because it is only reading");
            // If we need a write method this method should work like ServerProcessChannel.TryWrite
        }

        private static JsonSerializerOptions InitOptions()
        {
            // See ADR 003 for informations
            var screenResultConverter = new ProcessChannelConverter<ScreenResult>(ScreenResult.JsonTypeDiscriminatorAsInt, ScreenResult.JsonWrite, ScreenResult.JsonRead);

            // Using: System.Text.Json
            var options = new JsonSerializerOptions
            {
                Converters = { screenResultConverter },
                WriteIndented = false // important because we are using "\n" an separator between json objects.
            };

            return options;
        }


    }
}
