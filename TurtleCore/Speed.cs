using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    public enum SpeedLevel
    {
        Fastest,
        Fast,
        Normal,
        Slow,
        Slowest
    }

    /// <summary>
    /// Nearly the same as in python-turtle
    /// A value in the range 0..10 or a SpeedLevel (see enum SpeedLevel)
    /// Set the turtle's speed to an integer value in the range 0 .. 10.
    /// If input is smaller than 0.5, speed is set to 0.
    /// SpeedLevels are mapped to speedvalues in the following way:
    ///      Fastest :   0
    ///      Fast    :  10
    ///      Normal  :   6
    ///      Slow    :   3
    ///      Slowest :   1
    /// speeds from 1 to 10 enforce increasingly faster animation of
    /// line drawing and turtle turning.
    /// Attention:
    /// speed = 0 : *no* animation takes place.
    /// </summary>
    public record Speed
    {
        public double Value { get; init; }

        public bool NoAnimation { get { return Value == 0.0; } }

        public Speed(double value)
        {
            if (value <= 0.5)
                Value = 0.0;
            else
                Value = value; // In contrast to python: No speed limit
        }

        public static implicit operator Speed(SpeedLevel level)
        {
            return level switch
            {
                SpeedLevel.Fastest => new Speed(0.0),
                SpeedLevel.Fast => new Speed(10.0),
                SpeedLevel.Normal => new Speed(6.0),
                SpeedLevel.Slow => new Speed(3.0),
                SpeedLevel.Slowest => new Speed(1.0),
                _ => new Speed(6.0),// Same as normal
            };
        }

        public static implicit operator Speed(double speed) => new(speed);

        /// <summary>
        /// For the given speed: Time (measured in milliseconds) for the movement between two points
        /// </summary>
        /// <param name="fromPoint"></param>
        /// <param name="toPoint"></param>
        /// <returns></returns>
        public int MillisecondsForMovement(Vec2D fromPoint, Vec2D toPoint)
        {
            Vec2D distanceVector = toPoint - fromPoint;
            double distanceInPixels = distanceVector.AbsoluteValue;
            double speedInPixelsPerMillisecond = Value / 10;  // In case of speed == fast == 10: 1000 pixels per second, 1 pixel per millisecond

            // Python-turtle approximates a move by "hops"
            // Historical formula from python turtle: 
            //          diff = (end - start)
            //          diffsq = (diff[0] * screen.xscale)**2 + (diff[1] * screen.yscale)**2
            //          nhops = 1 + int((diffsq**0.5) / (3 * (1.1**self._speed) * self._speed))
            //
            // Formulated a little differently
            //          nhops = 1 + distanceInPixels / python_speed,  with python_speed = (3 * (1.1**self._speed) * self._speed))
            // 
            // For a distance of 1000 pixels this formula results to:
            //
            //          Speed.Value    python_speed   python_hops
            //
            //          Fast    (10)             78            12
            //          Normal  ( 6)             31            32
            //          Slow    ( 3)             12            83
            //          Slowest ( 1)              3           333
            //
            // I do not know the duration of a python "hop". But I have seen videos where a turtle with slowest speed needed approximately
            // one second for 100 pixels. This is a speed of 0.1 pixel per millisecond
            //
            // This formula yields speed of 0.1 for Value = 1
            speedInPixelsPerMillisecond = Math.Pow(1.1, Value) * Value * 0.1 / 1.1;

            //        speed = distance / time
            //    <=> time  = distance / speed
            int milliseconds = (int)(distanceInPixels / speedInPixelsPerMillisecond);
            if (milliseconds == 0)
                // We want an animation
                milliseconds = 1;
            return milliseconds;
        }
    }
}
