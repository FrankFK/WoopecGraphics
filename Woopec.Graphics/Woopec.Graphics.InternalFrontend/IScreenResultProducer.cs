using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Graphics.InternalDtos;

namespace Woopec.Graphics.InternalFrontend
{
    /// <summary>
    /// Hand screen results (e.g. the answer in a text input dialog windos) over to the code that is waiting for it.
    /// </summary>
    internal interface IScreenResultProducer
    {
        public void SendText(string text);
        public void SendNumber(double? number);

        public void SendVec2D(Vec2DValue value);
    }
}
