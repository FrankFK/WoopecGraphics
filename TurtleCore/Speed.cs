using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Core
{
    /// <summary>
    /// Specifies the turtle's speed as a value in the range 0 .. 10.
    /// If input is smaller than 0.5, speed is set to 0.
    /// speeds from 1 to 10 enforce increasingly faster animation of
    /// line drawing and turtle turning.
    /// Examples
    /// <example>
    /// <code>
    /// turtle.Speed = Speeds.Slow; <br/>
    /// turtle.Speed = 5; <br/>
    /// </code>
    /// </example>
    /// </summary>
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
            var speedInPixelsPerMillisecond = Math.Pow(1.1, Value) * Value * 0.1 / 1.1;

            //        speed = distance / time
            //    <=> time  = distance / speed
            int milliseconds = (int)(distanceInPixels / speedInPixelsPerMillisecond);
            if (milliseconds == 0)
                // We want an animation
                milliseconds = 1;
            return milliseconds;
        }

        /// <summary>
        /// For the given Speed:  Time (measured in milliseconds) for the movement between two heading-angles
        /// </summary>
        /// <param name="oldHeading">Old heading. Specified in degrees.</param>
        /// <param name="newHeading">New heading. Specified in degrees.</param>
        /// <returns></returns>
        public int MillisecondsForRotation(double oldHeading, double newHeading)
        {
            double distanceInDegrees = Math.Abs(oldHeading - newHeading);

            // Python-turtle approximates rotation by drawing multiple steps with delta-angles
            // Historical formula from python turtle: 
            //      anglevel = 3.0 * self._speed
            //      steps = 1 + int(abs(angle) / anglevel)
            //
            // I do not know the duration of a python "step". But on my machine a full rotation with slowest speed (Speed.Value == 1) has a duration of 2.3 seconds
            var speedInDegreesPerMillisecond = Value * 360 / (2.3 * 1000);

            int milliseconds = (int)(distanceInDegrees / speedInDegreesPerMillisecond);
            if (milliseconds == 0)
                // We want an animation
                milliseconds = 1;
            return milliseconds;
        }
    }

    /// <summary>
    /// Predefined Speeds with following speedvalues:
    ///      Fastest :   0 (*no* animation takes place.)<br></br>
    ///      Fast    :  10<br></br>
    ///      Normal  :   6<br></br>
    ///      Slow    :   3<br></br>
    ///      Slowest :   1<br></br>
    /// </summary>
    public class Speeds
    {
        public static Speed Fastest { get { return new Speed(0.0); } }
        public static Speed Fast { get { return new Speed(10.0); } }
        public static Speed Normal { get { return new Speed(6.0); } }
        public static Speed Slow { get { return new Speed(3.0); } }
        public static Speed Slowest { get { return new Speed(1.0); } }
    }

}
