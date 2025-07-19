using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Graphics.Interface.Dtos;

namespace Woopec.Graphics.Interface.Screen
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
        /// <param name="position">(optional). Approximate position of the lower left corner of the dialog window</param>
        /// <returns></returns>
        public Task<string> TextInputAsync(string title, string prompt, DtoVec2D position);

        /// <summary>
        /// Open a NumberInput dialog on screen and return the number, that the user has entered
        /// </summary>
        /// <param name="dialog">Informations about the dialog and the expected value</param>
        /// <returns></returns>
        public Task<double?> NumberInputAsync(ScreenNumberDialog dialog);

        /// <summary>
        /// Show a text on screen
        /// </summary>
        /// <param name="textBlock">All information about the text</param>
        public void ShowTextBlock(ScreenTextBlock textBlock);

        /// <summary>
        /// Show a text on screen and return the upper right coordinate of the text
        /// </summary>
        /// <param name="textBlock"></param>
        /// <returns></returns>
        public Task<DtoVec2D> ShowTextBlockWithReturnCoordinateAsync(ScreenTextBlock textBlock);

    }
}
