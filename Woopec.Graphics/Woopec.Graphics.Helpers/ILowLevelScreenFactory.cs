using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Graphics.CommunicatedObjects;

namespace Woopec.Graphics.Helpers
{
    internal interface ILowLevelScreenFactory
    {
        public ILowLevelScreen CreateLowLevelScreen();
    }
}
