using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TurtleCore
{
    internal class ScreenObjectProducer : IScreenObjectProducer
    {
        private int _lineCounter;
        private readonly Channel<ScreenObject> _objectChannel;

        public ScreenObjectProducer(Channel<ScreenObject> channel)
        {
            _objectChannel = channel;
        }

        public int CreateLine()
        {
            _lineCounter++;
            return _lineCounter - 1;
        }

        public void DrawLine(ScreenLine line)
        {
            Console.WriteLine($"Producer: {line.ID} send to channel");
            _objectChannel.Writer.TryWrite(line);
        }
    }
}
