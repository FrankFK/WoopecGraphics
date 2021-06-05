using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Core
{
    /// <summary>
    /// An instance of this class represents the screen to which screen objects (lines, shapes, ...) are drawn
    /// </summary>
    public class Screen : IScreen
    {
        private static Screen _defaultScreen;

        private readonly IScreenObjectProducer _screenObjectProducer;

        private readonly Dictionary<string, ShapeBase> _shapes;

        private Screen(IScreenObjectProducer producer)
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

        #endregion

        /// <summary>
        /// Create a Screen-Instance which draws to a default-Screen
        /// </summary>
        /// <returns></returns>
        internal static Screen GetDefaultScreen()
        {
            if (_defaultScreen == null)
            {
                _defaultScreen = new Screen(TurtleOutputs.GetDefaultScreenObjectProducer());
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
