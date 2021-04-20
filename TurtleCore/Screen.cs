using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    public class Screen
    {
        private readonly IScreenObjectProducer _screenOutput;


        public Screen()
        {
            _screenOutput = TurtleOutputs.GetDefaultScreenOutput();
        }

        public int CreateLine()
        {
            return _screenOutput.CreateLine();
        }

        public void DrawLine(ScreenLine line)
        {
            _screenOutput.DrawLine(line);
        }

        private static Screen _defaultScreen;
        internal static Screen GetDefaultScreen()
        {
            if (_defaultScreen == null)
                _defaultScreen = new Screen();
            return _defaultScreen;
        }
    }
}
