using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Woopec.Core.Internal
{
    internal class ScreenObjectProducer : IScreenObjectProducer
    {
        private int _lineCounter;
        private int _figureCounter;
        private readonly IScreenObjectChannel _objectChannel;

        public ScreenObjectProducer(IScreenObjectChannel channel)
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

            Debug.WriteLine($"Producer: Line {line.ID} send to channel {line.AnimationInfoForDebugger()}. {success}");
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
            Debug.WriteLine($"Producer: Update of figure {figure.ID} send to channel {figure.AnimationInfoForDebugger()}. {success}");
        }

        public void ShowDialog(ScreenDialog dialog)
        {
            var success = _objectChannel.TryWrite(dialog) ? "" : "FAILED";
            Debug.WriteLine($"Producer: Dialog {dialog.Title} send to channel. {success}");
        }

        public void ShowNumberDialog(ScreenNumberDialog dialog)
        {
            var success = _objectChannel.TryWrite(dialog) ? "" : "FAILED";
            Debug.WriteLine($"Producer: Number dialog {dialog.Title} send to channel. {success}");
        }

    }
}
