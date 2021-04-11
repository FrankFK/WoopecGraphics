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

    /// <summary>
    /// An instance of this class represent the state of the turtle
    /// (heading, position an so on)
    /// </summary>
    internal class TurtleState
    {
        private readonly Screen _screen;

        public TurtleState()
        {
            _screen = Screen.GetDefaultScreen();
            Position = new Vec2D(0, 0);
            Orientation = new Vec2D(1, 0);
            Heading = 0;
        }

        public Vec2D Position { get; set; }

        public Vec2D Orientation { get; private set; }

        public double Heading { get; private set; }

        public void Rotate(double angle)
        {
            var newHeading = (Heading + angle) % 360;
            if (newHeading < 0) newHeading += 360;

            var newOrientation = Orientation.Rotate(angle);

            // TODO: Turtle neu anzeigen
            Orientation = newOrientation;
            Heading = newHeading;
        }

        public void Move(double distance)
        {
            var newPosition = Position + distance * Orientation;

            // TODO: Turtle bewegen
            var lineOnScreen = new LineOnScreen()
            {
                From = Position,
                To = newPosition,
                Color = "red",
                Width = 2,
                AnimationTime = 2,
            };

            _screen.DrawLine(lineOnScreen);

            Position = newPosition;

        }


    }
}
