using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Graphics.Interface.Dtos;

namespace Woopec.Graphics.Interface.Screen
{
    internal interface ILowLevelScreenFactory
    {
        public ILowLevelScreen CreateLowLevelScreen();
    }
}
