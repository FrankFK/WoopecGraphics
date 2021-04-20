using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TurtleCore
{
    internal class ScreenObjectConsumer : IScreenObjectConsumer
    {
        private readonly IScreenObjectWriter _writer;
        private readonly Channel<ScreenObject> _objectChannel;

        public ScreenObjectConsumer(IScreenObjectWriter writer, Channel<ScreenObject> channel)
        {
            _writer = writer;
            _objectChannel = channel;
        }

        public async Task<ScreenObject> GetNextObjectForWriterAsync()
        {
            var screenObject = await _objectChannel.Reader.ReadAsync();
            return screenObject;
        }

        public void SendNextObjectToWriter(ScreenObject screenObject, Action<int> whenFinished)
        {
            Console.WriteLine($"Consumer: {screenObject.ID} send to writer");
            _writer.StartAnimaton(screenObject, whenFinished);
        }

    }
}
