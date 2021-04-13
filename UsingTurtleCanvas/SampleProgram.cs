using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurtleCore;

namespace UsingTurtleCanvas
{
    class SampleProgram
    {
        public static void TurtleMain()
        {
            var turtle = new Turtle();

            turtle.Right(45);
            turtle.Forward(50);
            turtle.Left(90);
            turtle.Forward(100);
        }
    }
}
