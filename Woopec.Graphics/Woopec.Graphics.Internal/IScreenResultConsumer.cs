using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Graphics.InternalBackend
{
    internal interface IScreenResultConsumer
    {
        public Task<string> ReadTextResultAsync();
        public Task<double?> ReadNumberResultAsync();

        public Task<Vec2D> ReadVec2DResultAsync();
    }
}
