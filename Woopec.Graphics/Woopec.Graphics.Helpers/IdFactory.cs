using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Woopec.Graphics.Helpers
{
    internal class IdFactory
    {
        private static int s_totalCounter;
        public static int CreateNewId()
        {
            return Interlocked.Increment(ref s_totalCounter);
        }
    }
}
