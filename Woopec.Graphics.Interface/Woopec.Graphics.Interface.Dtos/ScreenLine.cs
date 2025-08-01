﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Graphics.Interface.Dtos
{
    /// <summary>
    /// An instance of this class is a line that can be printed and animated on the screen.
    /// It also can be removed from the screen.
    /// </summary>
    internal class ScreenLine : ScreenObject
    {
        public DtoVec2D Point1 { get; set; }

        public DtoVec2D Point2 { get; set; }

        public DtoColor Color { get; set; }

        public void AnimatePoint2(int milliseconds)
        {
            Animation.Effects.Add(new ScreenAnimationMovement() { AnimatedProperty = ScreenAnimationMovementProperty.Point2, Milliseconds = milliseconds, StartValue = Point1 });
        }

        // TO-DO: Color-Handling
        // public Color Color { get; set; }
    }
}
