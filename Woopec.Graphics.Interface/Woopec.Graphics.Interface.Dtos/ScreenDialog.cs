﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Graphics.Interface.Dtos
{
    /// <summary>
    /// An instance of this class is a dialog that should be shown on the screen.
    /// </summary>
    internal class ScreenDialog : ScreenObject
    {
        public string Title { get; set; }

        public string Prompt { get; set; }

        public DtoVec2D Position { get; set; }
    }
}
