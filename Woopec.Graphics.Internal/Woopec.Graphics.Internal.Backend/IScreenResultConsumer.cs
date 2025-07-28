using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Graphics.Interface.Dtos;

namespace Woopec.Graphics.Internal.Backend
{
    internal interface IScreenResultConsumer
    {
        public Task<string> ReadTextResultAsync();
        public Task<double?> ReadNumberResultAsync();

        public Task<DtoVec2D> ReadVec2DResultAsync();
    }
}
