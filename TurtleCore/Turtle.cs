// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace TurtleCore
{
    /// <summary>
    /// An instance of this class represents a turtle.
    /// Documentation: see https://docs.python.org/3/library/turtle.html#module-turtle
    /// python Implementation: see https://github.com/python/cpython/blob/master/Lib/turtle.py
    /// </summary>
    public class Turtle
    {
        public Vec2D Position { get; set; }

        private Vec2D _orientation;

        public Turtle()
        {
            Position = new Vec2D(0, 0);
            _orientation = new Vec2D(1, 0);
        }

        public void Forward(double distance)
        {
            Position += distance * _orientation;
        }
    }
}
