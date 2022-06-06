using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Core.Internal
{
    /// <summary>
    /// Hand screen results (e.g. the answer in a text input dialog windos) over to the code that is waiting for it.
    /// </summary>
    internal interface IScreenResultProducer
    {
        public void SendText(string text);
    }
}
