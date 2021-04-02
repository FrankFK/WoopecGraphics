// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    public record Vec2D(double XCor, double YCor)
    {
        public double AbsoluteValue => Math.Sqrt(XCor * XCor + YCor * YCor);

        public static Vec2D operator +(Vec2D vector1, Vec2D vector2) => new(vector1.XCor + vector2.XCor, vector1.YCor + vector2.YCor);
        public static Vec2D operator -(Vec2D vector1, Vec2D vector2) => new(vector1.XCor - vector2.XCor, vector1.YCor - vector2.YCor);
        public static Vec2D operator *(Vec2D vector1, Vec2D vector2) => new(vector1.XCor * vector2.XCor, vector1.YCor * vector2.YCor);
        public static Vec2D operator *(double scalar, Vec2D vector) => new(scalar * vector.XCor, scalar * vector.YCor);
        public static Vec2D operator -(Vec2D vector) => new(-vector.XCor, -vector.YCor);

        /// <summary>
        /// Rotation counterclockwise
        /// </summary>
        /// <param name="angle">measured in degress (360 = complete rotation)</param>
        /// <returns></returns>
        public Vec2D Rotate(double angle)
        {
            var radians = Math.PI * angle / 180;
            var cosine = Math.Cos(radians);
            var sine = Math.Sin(radians);
            return new(XCor * cosine - YCor * sine, XCor * sine + YCor * cosine);
        }


    }
}
