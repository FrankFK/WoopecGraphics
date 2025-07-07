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
    internal class ScreenObjectChannelInServerProcess : IScreenObjectChannelForWriter, IScreenObjectChannelForReader
    {
        private static readonly JsonSerializerOptions _options = InitOptions();
        private readonly AnonymousPipeServerStream _serverStream;
        private readonly StreamWriter _streamWriter;

        public string Handle { get; init; }

        public ScreenObjectChannelInServerProcess()
        {
            _serverStream = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);
            _streamWriter = new StreamWriter(_serverStream);
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

        public ValueTask<ScreenObject> ReadAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("ServerProcessChannel does not need a read-method because it is only writing");
            // If we need a read method this method should work like ClientProcessChannel.ReadAsync
        }

        public bool TryWrite(ScreenObject screenObject)
        {
            try
            {
                string serialization = JsonSerializer.Serialize(screenObject, _options);
                _streamWriter.WriteLine(serialization);
                _streamWriter.Flush();
                _serverStream.WaitForPipeDrain();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static JsonSerializerOptions InitOptions()
        {
            // See ADR 003 for informations

            var shapeBaseConverter = new ProcessChannelConverter<ShapeBase>(ShapeBase.JsonTypeDiscriminatorAsInt, ShapeBase.JsonWrite, ShapeBase.JsonRead);
            var effectConverter = new ProcessChannelConverter<ScreenAnimationEffect>(ScreenAnimationEffect.JsonTypeDiscriminatorAsInt, ScreenAnimationEffect.JsonWrite, ScreenAnimationEffect.JsonRead);
            var screenObjetctConverter = new ProcessChannelConverter<ScreenObject>(ScreenObject.JsonTypeDiscriminatorAsInt, ScreenObject.JsonWrite, ScreenObject.JsonRead);

            // Using: System.Text.Json
            var options = new JsonSerializerOptions
            {
                Converters = { shapeBaseConverter, effectConverter, screenObjetctConverter },
                WriteIndented = false // important because we are using "\n" an separator between json objects.
            };

            return options;
        }


    }
}
