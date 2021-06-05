using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Core
{
    /// <summary>
    /// Generate screen objects and hand them over to a consumer
    /// </summary>
    internal interface IScreenObjectProducer
    {
        public int CreateLine();

        public void DrawLine(ScreenLine line);

        public int CreateFigure();

        public void UpdateFigure(ScreenFigure figure);
    }
}
