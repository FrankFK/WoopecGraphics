﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Core
{

    /// <summary>
    /// An instance of this class is a text that should be written to the the screen.
    /// </summary>
    internal class ScreenTextBlock : ScreenObject
    {
        public Vec2D Position { get; set; }

        public string Text { get; set; }

        public TextStyle TextStyle { get; set; }

        public TextAlignmentType Alignment { get; set; }

        public bool ReturnLowerRightCorner { get; set; }
    }
}
