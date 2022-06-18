using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Core.Internal
{
    /// <summary>
    /// An instance of this class represents the screen to which screen objects (lines, shapes, ...) are drawn
    /// </summary>
    /// <remarks>
    /// This interface only is used internally.The users of the woopec library are using the Screen class
    /// </remarks>
    internal interface ILowLevelScreen
    {
        /// <summary>
        /// Return the GroupId of the last animation that is drawn at the screen
        /// </summary>
        public int LastIssuedAnimatonGroupID { get; set; }

        public int CreateLine();

        public void DrawLine(ScreenLine line);

        public int CreateFigure();

        public void UpdateFigure(ScreenFigure figure);

        /// <summary>
        /// Open a TextInput dialog on screen and return the text, that the user has entered
        /// </summary>
        /// <param name="title"></param>
        /// <param name="prompt"></param>
        /// <returns></returns>
        public Task<string> TextInputAsync(string title, string prompt);

        /// <summary>
        /// Open a NumberInput dialog on screen and return the number, that the user has entered
        /// </summary>
        /// <param name="dialog">Informations about the dialog and the expected value</param>
        /// <returns></returns>
        public Task<double?> NumberInputAsync(ScreenNumberDialog dialog);
    }
}
