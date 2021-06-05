using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Core
{
    /// <summary>
    /// I have made a few attempts using this record for coordinates.
    /// If the coordinates in Vec2D were these C#-records the usage would be fine
    /// (the actual unit-tests for Vec2D would work without any code-changes). But the performance
    /// would slow down with factor 2. 
    /// Therefore this record is not used at the moment.
    /// </summary>
    internal record Coordinate(double Value)
    {
        public static implicit operator double(Coordinate c) => c.Value;
        public static implicit operator Coordinate(double value) => new(value);
    }
}
