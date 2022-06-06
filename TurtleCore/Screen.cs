using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Core.Internal;

namespace Woopec.Core
{
    /// <summary>
    /// An instance of this class represents the screen to which screen objects (lines, shapes, ...) are drawn
    /// </summary>
    public class Screen
    {
        private readonly ILowLevelScreen _lowLevelScreen;

        internal Screen(ILowLevelScreen lowLevelScreen)
        {
            _lowLevelScreen = lowLevelScreen;
        }

        /// <summary>
        /// Pop up a dialog window for input of a string.
        /// <example>
        /// <code>
        /// var turtle = new Turtle(); <br/>
        /// var answer = turtle.Screen.TextInput("NIM", "Name of first player:"); <br/>
        /// if (answer != null)  <br/>
        /// {  <br/>
        ///     // Handling the answer. <br/>
        /// }  <br/>
        /// else  <br/>
        /// {  <br/>
        ///     // Code for the case, that the user has canceled the dialog. <br/>
        /// }  <br/>
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="title">Title of the dialog window</param>
        /// <param name="prompt">A text mostly describing what information to input</param>
        /// <returns>The string input. If the dialog is canceled, return null</returns>
        public string TextInput(string title, string prompt)
        {
            // async/await is too complex for C# beginners. Therfore we wait for the input:
            var task = _lowLevelScreen.TextInputAsync(title, prompt);
            return task.Result;
        }

    }
}
