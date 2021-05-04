using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    public class Screen
    {
        private readonly IScreenObjectProducer _screenObjectProducer;


        public Screen()
        {
            _screenObjectProducer = TurtleOutputs.GetDefaultScreenObjectProducer();
        }

        public int CreateLine()
        {
            if (_screenObjectProducer == null)
            {
                Console.WriteLine("Producer is null!!!!!");
            }
            return _screenObjectProducer.CreateLine();
        }

        public void DrawLine(ScreenLine line)
        {
            _screenObjectProducer.DrawLine(line);
        }

        private static Screen s_defaultScreen;
        internal static Screen GetDefaultScreen()
        {
            if (s_defaultScreen == null)
                s_defaultScreen = new Screen();
            return s_defaultScreen;
        }
    }
}
