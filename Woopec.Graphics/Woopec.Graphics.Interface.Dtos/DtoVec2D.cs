using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Graphics.Interface.Dtos
{
    /// <summary>
    /// We do not want to have any dependencies on the Woopec.Graphics objects. Therefore we use a separate type instead of Vec2D.
    /// </summary>
    /// <param name="X"></param>
    /// <param name="Y"></param>
    internal record DtoVec2D(double X, double Y);
}
