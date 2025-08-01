﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Woopec.Graphics.Interface.Dtos;

namespace Woopec.Graphics.Internal.Backend
{
    internal class ScreenObjectProducer : IScreenObjectProducer
    {
        private int _lineCounter;
        private int _figureCounter;
        private readonly IScreenObjectChannelForWriter _objectChannel;

        public ScreenObjectProducer(IScreenObjectChannelForWriter channel)
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
            var success = _objectChannel.TryWrite(line) ? "" : "FAILED";

            Debug.WriteLine($"Producer: Line {line.ID} is sent to channel {line.AnimationInfoForDebugger()}. {success}");
        }


        public int CreateFigure()
        {
            _figureCounter++;
            var figureId = _figureCounter - 1;

            return figureId;
        }


        public void UpdateFigure(ScreenFigure figure)
        {
            var success = _objectChannel.TryWrite(figure) ? "" : "FAILED"; ;
            Debug.WriteLine($"Producer: Update of figure {figure.ID} is sent to channel {figure.AnimationInfoForDebugger()}. {success}");
        }

        public void ShowDialog(ScreenDialog dialog)
        {
            var success = _objectChannel.TryWrite(dialog) ? "" : "FAILED";
            Debug.WriteLine($"Producer: Dialog {dialog.Title} is sent to channel. {success}");
        }

        public void ShowNumberDialog(ScreenNumberDialog dialog)
        {
            var success = _objectChannel.TryWrite(dialog) ? "" : "FAILED";
            Debug.WriteLine($"Producer: Number dialog {dialog.Title} is sent to channel. {success}");
        }

        public void ShowTextBlock(ScreenTextBlock textBlock)
        {
            var success = _objectChannel.TryWrite(textBlock) ? "" : "FAILED";
            Debug.WriteLine($"Producer: Text block {textBlock.Text.Substring(0, Math.Min(10, textBlock.Text.Length))} is sent to channel. {success}");
        }
    }
}
