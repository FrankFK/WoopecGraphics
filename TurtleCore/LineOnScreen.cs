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
    /// An instance of this class is a line that must be drawn to the screen
    /// </summary>
    public class LineOnScreen
    {
        public Vec2D From { get; set; }

        public Vec2D To { get; set; }

        public string Color { get; set; }

        public double Width { get; set; }

        /// <summary>
        /// true if the line should be on top of all other existig objects
        /// </summary>
        public bool OnTop { get; set; }

        /// <summary>
        /// Time (in seconds) for the animation of the line-drawing (0 no animation)
        /// </summary>
        public double AnimationTime { get; set; }

    }
}
