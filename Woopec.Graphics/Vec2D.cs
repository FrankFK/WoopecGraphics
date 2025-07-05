using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Core
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
    public record Vec2D(double X, double Y)
    {
        /// <summary>
        /// The absolute value of the vector (the "length" of the vector)
        /// </summary>
        public double AbsoluteValue => Math.Sqrt(X * X + Y * Y);

        /// <summary>
        /// A string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"X = {X}, Y = {Y}";
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Vec2D operator +(Vec2D vector1, Vec2D vector2) => new(vector1.X + vector2.X, vector1.Y + vector2.Y);
        public static Vec2D operator -(Vec2D vector1, Vec2D vector2) => new(vector1.X - vector2.X, vector1.Y - vector2.Y);
        public static Vec2D operator *(Vec2D vector1, Vec2D vector2) => new(vector1.X * vector2.X, vector1.Y * vector2.Y);
        public static Vec2D operator *(double scalar, Vec2D vector) => new(scalar * vector.X, scalar * vector.Y);
        public static Vec2D operator -(Vec2D vector) => new(-vector.X, -vector.Y);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

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
            return new(X * cosine - Y * sine, X * sine + Y * cosine);
        }

        /// <summary>
        /// Calculate the angle to the given position. This angle is given as heading.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>Heading between 0 and 360. Heading 0 shows to East, 90 to North, and so on.</returns>
        public double HeadingTo(Vec2D position)
        {
            var delta = position - this;
            var x = delta.X;
            var y = delta.Y;

            var angle = Math.Atan2(y, x);

            var heading = 360 * angle / (2 * Math.PI);
            if (heading < 0)
                heading = 360 + heading;
            return heading;
        }

        /// <summary>
        /// Return true if this is approximately equal to <paramref name="vector"/>
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="precision">Absolute difference of the x- and y-coordinates is less than this value</param>
        /// <returns></returns>
        public bool IsApproximatelyEqualTo(Vec2D vector, double precision)
        {
            return (Math.Abs(X - vector.X) <= precision && Math.Abs(Y - vector.Y) <= precision);
        }

        /// <summary>
        /// Instead of <code>Vec2D x = new Vec2D(1, 4)</code> you can write <code>Vec2D x = (1, 4)</code>
        /// </summary>
        /// <param name="tuple"></param>
        public static implicit operator Vec2D((double x, double y) tuple) => new Vec2D(tuple.x, tuple.y);

    }
}
