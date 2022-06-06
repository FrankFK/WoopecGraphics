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
        private readonly IChannel _objectChannel;

        public ScreenObjectProducer(IChannel channel)
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
            Debug.WriteLine($"Producer: Line {line.ID} send to channel");
            _objectChannel.TryWrite(line);
        }


        public int CreateFigure()
        {
            _figureCounter++;
            var figureId = _figureCounter - 1;

            return figureId;
        }


        public void UpdateFigure(ScreenFigure figure)
        {
            _objectChannel.TryWrite(figure);
            Debug.WriteLine($"Producer: Update of figure {figure.ID} send to channel");
        }

        public void ShowDialog(ScreenDialog dialog)
        {
            _objectChannel.TryWrite(dialog);
            Debug.WriteLine($"Producer: Dialog {dialog.Title} send to channel");
        }

    }
}
