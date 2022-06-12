using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Woopec.Core.Internal
{
    internal class ScreenResultProducer : IScreenResultProducer
    {
        private readonly IScreenResultChannel _resultChannel;

        public ScreenResultProducer(IScreenResultChannel channel)
        {
            _resultChannel = channel;
        }

        public void SendText(string text)
        {
            var result = new ScreenResultText() { Text = text };
            _resultChannel.TryWrite(result);
            Debug.WriteLine($"ScreenResult: Text {result.Text} send to channel");
        }

        public void SendNumber(double? number)
        {
            var result = new ScreenResultNumber() { Value = number};
            _resultChannel.TryWrite(result);
            Debug.WriteLine($"ScreenResult: Number {result.Value} send to channel");
        }
    }
}
