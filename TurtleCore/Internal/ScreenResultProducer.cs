﻿using System;
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
            Debug.WriteLine($"ScreenResult: Text {result.Text} is sent to channel");
        }

        public void SendNumber(double? number)
        {
            var result = new ScreenResultNumber() { Value = number};
            _resultChannel.TryWrite(result);
            Debug.WriteLine($"ScreenResult: Number {result.Value} is sent to channel");
        }
        public void SendVec2D(Vec2D value)
        {
            var result = new ScreenResultVec2D() { Value = value};
            _resultChannel.TryWrite(result);
            Debug.WriteLine($"ScreenResult: value ({result.Value} is sent to channel");
        }
    }
}
