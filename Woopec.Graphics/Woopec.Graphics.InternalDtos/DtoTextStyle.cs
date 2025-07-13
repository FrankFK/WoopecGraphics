using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Graphics.InternalDtos
{
    /// <summary>
    /// Horizontal alignment of a text
    /// </summary>
    internal enum DtoTextAlignmentType
    {
        /// <summary>
        /// Left aligned text
        /// </summary>
        Left,

        /// <summary>
        /// Right aligned text
        /// </summary>
        Right,

        /// <summary>
        /// Centered text 
        /// </summary>
        Center
    }

    /// <summary>
    /// The style of a font
    /// </summary>
    internal enum DtoFontStyleType
    {
        /// <summary>
        /// Normal font
        /// </summary>
        Normal,

        /// <summary>
        /// Italic font
        /// </summary>
        Italic
    }

    /// <summary>
    /// Weight of a font
    /// </summary>
    internal enum DtoFontWeightType
    {
        // There are more in https://learn.microsoft.com/en-us/dotnet/api/system.windows.fontweights?view=windowsdesktop-7.0
        // I've only selected a few of them

        /// <summary>
        /// Thin weight (100)
        /// </summary>
        Thin = 100,

        /// <summary>
        /// Light weight (300)
        /// </summary>
        Light = 300,

        /// <summary>
        /// Normal weight (400)
        /// </summary>
        Normal = 400,

        /// <summary>
        /// Bold weight (700);
        /// </summary>
        Bold = 700
    }

    /// <summary>
    /// An instance of this class describes properties of a text 
    /// </summary>
    internal class DtoTextStyle
    {
        /// <summary>
        /// For example "Arial" or "Times New Roman". 
        /// </summary>
        public string FontFamilyName { get; set; }

        /// <summary>
        /// For example 12. Default is 8
        /// </summary>
        public double FontSize { get; set; } = 8;

        /// <summary>
        /// For example Normal or Italic. Default is Normal
        /// </summary>
        public DtoFontStyleType FontStyle { get; set; }

        /// <summary>
        /// For example Thin, Normal or Bold. Default is Normal
        /// </summary>
        public DtoFontWeightType FontWeight { get; set; }

        /// <summary>
        /// The color of the text background (default is null whichs means that the background is not colored)
        /// </summary>
        public ColorValue BackgroundColor { get; set; }

        /// <summary>
        /// The color of the text (default is black)
        /// </summary>
        public ColorValue ForegroundColor { get; set; }
    }
}
