using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Graphics.InternalDtos
{
    /// <summary>
    /// An instance of this class is a dialog that should be shown on the screen.
    /// </summary>
    internal class ScreenDialog : ScreenObject
    {
        public string Title { get; set; }

        public string Prompt { get; set; }

        public Vec2DValue Position { get; set; }
    }
}
