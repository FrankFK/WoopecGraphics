using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Graphics.InternalDtos
{

    /// <summary>
    /// An instance of this class is a text that should be written to the the screen.
    /// </summary>
    internal class ScreenTextBlock : ScreenObject
    {
        public Vec2DValue Position { get; set; }

        public string Text { get; set; }

        public DtoTextStyle TextStyle { get; set; }

        public DtoTextAlignmentType Alignment { get; set; }

        public bool ReturnLowerRightCorner { get; set; }
    }
}
