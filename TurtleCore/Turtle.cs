﻿// Licensed to the .NET Foundation under one or more agreements.
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
        private readonly TurtleState _turtleState;

        public Vec2D Position { get { return _turtleState.Position; } set { _turtleState.Position = value; } }

        /// <summary>
        /// Orientation of the turtle.
        /// Here are some common directions in degrees:
        ///  standard - mode:          logo-mode:
        /// -------------------|--------------------
        ///    0 - east                0 - north
        ///   90 - north              90 - east
        ///  180 - west              180 - south
        ///  270 - south             270 - west
        /// </summary>
        public double Heading
        {
            get { return _turtleState.Heading; }
            set
            {
                _turtleState.Rotate(value - Heading);
            }
        }


        public Turtle()
        {
            _turtleState = new TurtleState();
        }

        public void Forward(double distance)
        {
            _turtleState.Move(distance);
        }

        public void Backward(double distance)
        {
            _turtleState.Move(-distance);
        }

        public void Left(double angle)
        {
            _turtleState.Rotate(angle);
        }

        public void Right(double angle)
        {
            _turtleState.Rotate(-angle);
        }


    }
}
