using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Core
{
    /// <summary>
    /// An instance of this class is a dialog that should be shown on the screen. This dialog is used for number-input
    /// </summary>
    internal class ScreenNumberDialog : ScreenObject
    {
        internal enum NumberType
        {
            Integer = 0,
            Double = 1,
        }

        public string Title { get; set; }

        public string Prompt { get; set; }

        public double? DefaultValue { get; set; }

        /// <summary>
        /// If MinValue and MaxValue are not null, the user-input is checked if it is in range MinValue ... MaxValue. 
        /// If not, a hint is issued and the dialog remains open for correction.
        /// </summary>
        public double? MinValue { get; set; }
        public double? MaxValue { get; set; }

        /// <summary>
        /// If ReturnType is Integer, the user-input is checked if it is an integer.
        /// If ReturnType is Double, the user-input is checked if it is an double.
        /// If not, a hint is issued and the dialog remains open for correction.
        /// </summary>
        public NumberType ReturnType { get; set; }

    }
}
