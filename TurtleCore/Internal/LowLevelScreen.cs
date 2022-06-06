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
    /// This class only is used internally.The users of the woopec library are using the Screen class.
    /// </remarks>
    internal class LowLevelScreen : ILowLevelScreen
    {
        private static LowLevelScreen _defaultScreen;

        private readonly IScreenObjectProducer _screenObjectProducer;

        private readonly Dictionary<string, ShapeBase> _shapes;

        private LowLevelScreen(IScreenObjectProducer producer)
        {
            _screenObjectProducer = producer ?? throw new ArgumentNullException("producer");
            _shapes = new();
            LastIssuedAnimatonGroupID = ScreenObject.NoGroupId;
        }


        #region Methods of IScreen
        ///<inheritdoc/>
        public int LastIssuedAnimatonGroupID { get; set; }

        ///<inheritdoc/>
        public int CreateLine()
        {
            return _screenObjectProducer.CreateLine();
        }

        ///<inheritdoc/>
        public void DrawLine(ScreenLine line)
        {
            UpdateLastIssuedAnimationGroupID(line);
            _screenObjectProducer.DrawLine(line);
        }

        ///<inheritdoc/>
        public int CreateFigure()
        {
            return _screenObjectProducer.CreateFigure();
        }

        ///<inheritdoc/>
        public void UpdateFigure(ScreenFigure figure)
        {
            UpdateLastIssuedAnimationGroupID(figure);
            _screenObjectProducer.UpdateFigure(figure);
        }

        ///<inheritdoc/>
        public void RegisterShape(string name, ShapeBase shape)
        {
            _shapes.Add(name, shape);
        }

        ///<inheritdoc/>
        public void AddShape(string name, ShapeBase shape) => RegisterShape(name, shape);

        public ShapeBase GetShape(string shapeName)
        {
            return _shapes.GetValueOrDefault(shapeName);
        }


        ///<inheritdoc/>
        public List<string> GetShapes()
        {
            return _shapes.Select(s => s.Key).ToList();
        }

        ///<inheritdoc/>
        public string TextInput(string title, string prompt)
        {
            var dialog = new ScreenDialog() { Title = title, Prompt = prompt };
            // If we do not wait for another animation this dialog is shown immediately. In most cases the programmer expects
            // that all previously created animation are drawn before the dialog is shown
            // Therefore:
            dialog.WaitForAnimationsOfGroupID = LastIssuedAnimatonGroupID;
            _screenObjectProducer.ShowDialog(dialog);
            return "Hier fehlt noch das Warten auf den Dialog";
        }


        #endregion

        /// <summary>
        /// Create a LowLevelScreen-Instance which draws to a default-Screen
        /// </summary>
        /// <returns></returns>
        internal static LowLevelScreen GetDefaultScreen()
        {
            if (_defaultScreen == null)
            {
                _defaultScreen = new LowLevelScreen(TurtleOutputs.GetDefaultScreenObjectProducer());
            }
            return _defaultScreen;
        }

        /// <summary>
        /// Needed for tests
        /// </summary>
        internal static void ResetDefaultScreen()
        {
            _defaultScreen = null;
        }

        private void UpdateLastIssuedAnimationGroupID(ScreenObject screenObject)
        {
            if (screenObject.HasAnimations)
            {
                LastIssuedAnimatonGroupID = screenObject.GroupID;
            }
        }
    }
}
