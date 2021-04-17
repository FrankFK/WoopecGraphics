﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    /// <summary>
    /// An instance of this class is a two-dimensional vector
    /// </summary>
    /// <remarks>
    /// The values of a Vec2D can not be changed. If you want to change a value, You can copy the vector.
    /// <code>
    ///    var vector = new Vec2D(1, 2);
    ///    var vector2 = vector with { YCor = 0 );
    /// </code>
    /// </remarks>
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

        /// <summary>
        /// Return true if this is approximately equal to <paramref name="vector"/>
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="precision">Absolute difference of the x- and y-coordinates is less than this value</param>
        /// <returns></returns>
        public bool IsApproximatelyEqualTo(Vec2D vector, double precision)
        {
            return (Math.Abs(XCor - vector.XCor) <= precision && Math.Abs(YCor - vector.YCor) <= precision);
        }


    }
}