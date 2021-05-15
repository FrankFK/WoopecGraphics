using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TurtleCore.Internal
{
    internal class ScreenObjectProducer : IScreenObjectProducer
    {
        private int _lineCounter;
        private int _figureCounter;
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
            Debug.WriteLine($"Producer: Line {line.ID} send to channel");
            _objectChannel.Writer.TryWrite(line);
        }


        public int CreateFigure(ShapeBase shape)
        {
            _figureCounter++;
            var figureId = _figureCounter - 1;

            // The consumer creates the figure. The consumer does not show the figure
            var creation = new ScreenFigureCreate(figureId, shape);
            _objectChannel.Writer.TryWrite(creation);

            return figureId;
        }

        public void UpdateFigure(ScreenFigure figure)
        {
            _objectChannel.Writer.TryWrite(figure);
            Debug.WriteLine($"Producer: Update of figure {figure.ID} send to channel");
        }
    }
}
