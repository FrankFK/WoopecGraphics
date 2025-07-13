using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Graphics.InternalDtos
{
    /// <summary>
    /// We do not want to have any dependencies on the Woopec.Graphics objects. Therefore we use a separate type instead of Vec2D.
    /// </summary>
    /// <param name="X"></param>
    /// <param name="Y"></param>
    public record Vec2DValue(double X, double Y);
}
