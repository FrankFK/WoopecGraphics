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
        private static Screen _defaultScreen;

        internal Screen(ILowLevelScreen lowLevelScreen)
        {
            _lowLevelScreen = lowLevelScreen;
        }


        /// <summary>
        /// The default-Screen
        /// </summary>
        /// <returns></returns>
        public static Screen Default
        {
            get
            {
                if (_defaultScreen == null)
                {
                    _defaultScreen = new Screen(LowLevelDefaultScreen.Get());
                }
                return _defaultScreen;
            }
        }


        /// <summary>
        /// Change the default screen so that Turtle, Figure and Line can be used in unit tests.
        /// </summary>
        public static void SwitchToUnitTestDefaultScreen()
        {
            LowLevelDefaultScreen.SetUnitTestScreen();
            _defaultScreen = new Screen(LowLevelDefaultScreen.Get());
        }

        /// <summary>
        /// After usage of SwitchToUnitTestDefaultScreen: Change the default screen back to its normal behaviour.
        /// </summary>
        public static void SwitchToNormalDefaultScreen()
        {
            LowLevelDefaultScreen.Reset();
            _defaultScreen = null; // do NOT directly try to create a new Screen, because this would try to generate a normal screen. And we are in a unit test scenario here, where we have no normal screen.
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
        /// <param name="position">(optional). Approximate position of the lower left corner of the dialog window</param>
        /// <returns>The string input. If the dialog is canceled, return null</returns>
        public string TextInput(string title, string prompt, Vec2D position)
        {
            var task = _lowLevelScreen.TextInputAsync(title, prompt, position);

            // async/await is too complex for C# beginners. Therfore we wait for the input:
            return task.Result;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string TextInput(string title, string prompt) => TextInput(title, prompt, null);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Pop up a dialog window for input of an integer value. 
        /// The number input must be in the range minValue .. maxValue if these are given. If not, a hint is issued and the dialog remains open for correction.
        /// <example>
        /// <code>
        /// var turtle = new Turtle(); <br/>
        /// var answer = turtle.Screen.NumInput("Poker", "Your stakes:", 1000, 10, 10000; <br/>
        /// if (answer != null)  <br/>
        /// {  <br/>
        ///     int stakes = answer.GetValueOrDefault();  <br/>
        ///     // Handling the stakes <br/>
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
        /// <param name="defaultValue">(optional). Default value</param>
        /// <param name="minValue">(optional). Minimal value</param>
        /// <param name="maxValue">(optional). Maximal value</param>
        /// <param name="position">(optional). Approximate position of the lower left corner of the dialog window</param>
        /// <returns>Return the number input. If the dialog is canceled, return null.</returns>
        public int? NumInput(string title, string prompt, int? defaultValue, int? minValue, int? maxValue, Vec2D position)
        {
            var dialog = new ScreenNumberDialog() { Title = title, Prompt = prompt, DefaultValue = defaultValue, MinValue = minValue, MaxValue = maxValue, Position = position, ReturnType = ScreenNumberDialog.NumberType.Integer };

            var task = _lowLevelScreen.NumberInputAsync(dialog);

            // async/await is too complex for C# beginners. Therfore we wait for the input:
            return (int?)task.Result;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public int? NumInput(string title, string prompt) => NumInput(title, prompt, null, null, null, null);
        public int? NumInput(string title, string prompt, int defaultValue) => NumInput(title, prompt, defaultValue, null, null, null);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Pop up a dialog window for input of an double value. 
        /// The number input must be in the range minValue .. maxValue if these are given. If not, a hint is issued and the dialog remains open for correction.
        /// <example>
        /// <code>
        /// var turtle = new Turtle(); <br/>
        /// var answer = turtle.Screen.DoubleInput("Poker", "Your stakes:", 00.50, 0.10, 2.50); <br/>
        /// if (answer != null)  <br/>
        /// {  <br/>
        ///     double stakes = answer.GetValueOrDefault();  <br/>
        ///     // Handling the stakes <br/>
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
        /// <param name="defaultValue">(optional). Default value</param>
        /// <param name="minValue">(optional). Minimal value</param>
        /// <param name="maxValue">(optional). Maximal value</param>
        /// <param name="position">(optional). Approximate position of the lower left corner of the dialog window</param>
        /// <returns>Return the number input. If the dialog is canceled, return null.</returns>
        public double? DoubleInput(string title, string prompt, double? defaultValue, double? minValue, double? maxValue, Vec2D position)
        {
            var dialog = new ScreenNumberDialog() { Title = title, Prompt = prompt, DefaultValue = defaultValue, MinValue = minValue, MaxValue = maxValue, Position = position, ReturnType = ScreenNumberDialog.NumberType.Double };

            var task = _lowLevelScreen.NumberInputAsync(dialog);

            // async/await is too complex for C# beginners. Therfore we wait for the input:
            return task.Result;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public double? DoubleInput(string title, string prompt) => DoubleInput(title, prompt, null, null, null, null);
        public double? DoubleInput(string title, string prompt, double defaultValue) => DoubleInput(title, prompt, defaultValue, null, null, null);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Write text at the given position (which sets the upper left corner of the text) according to alignment and with the given text style.
        /// If returnLowerRightCorner is true, the coordinate of the lower right corner of the text is returned
        /// By default, returnUpperRightCorner  is False.
        /// </summary>
        /// <param name="text">Text. Multiple lines can be separated by \n </param>
        /// <param name="position">upper left corner </param>
        /// <param name="textStyle">Definition of the text style</param>
        /// <param name="alignment">Horizontal alignment of the text (relevant if text has multiple lines)</param>
        /// <param name="returnLowerRightCorner">Upper right corner of the text</param>
        /// <returns></returns>
        public Vec2D WriteText(string text, Vec2D position, TextStyle textStyle, TextAlignmentType alignment, bool returnLowerRightCorner)
        {
            var textBlock = new ScreenTextBlock() { Text = text, Position = position, TextStyle = textStyle, Alignment = alignment, ReturnLowerRightCorner = returnLowerRightCorner };

            if (returnLowerRightCorner)
            {
                var task = _lowLevelScreen.ShowTextBlockWithReturnCoordinateAsync(textBlock);
                return task.Result;
            }
            else
            {
                _lowLevelScreen.ShowTextBlock(textBlock);
                return null;
            }
        }
        /// <summary>
        /// Shut the turtlegraphics window
        /// <example>
        /// <code>
        /// var turtle = new Turtle(); <br/>
        /// // ... <br/>
        /// if (userWantsToExit) <br/>
        /// {  <br/>
        ///     turtle.Screen.Bye();  <br/>
        /// }  <br/>
        /// </code>
        /// </example>
        /// </summary>
        public void Bye()
        {
            // There may be better ways to shut the window. But that`s enough for the start (KISS)
            System.Environment.Exit(0);
        }

    }
}
