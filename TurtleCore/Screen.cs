using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    /// <summary>
    /// An instance of this class represents the screen to which screen objects (lines, shapes, ...) are drawn
    /// </summary>
    public class Screen
    {
        /// <summary>
        /// Return the GroupId of the last animation that is drawn at the screen
        /// </summary>
        public int LastIssuedAnimatonGroupID { get; set; }

        public int CreateLine()
        {
            if (_screenObjectProducer == null)
            {
                Console.WriteLine("Producer is null!!!!!");
            }
            return _screenObjectProducer.CreateLine();
        }

        public void DrawLine(ScreenLine line)
        {
            UpdateLastIssuedAnimationGroupID(line);
            _screenObjectProducer.DrawLine(line);
        }

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

        private static Screen _defaultScreen;

        private readonly IScreenObjectProducer _screenObjectProducer;

        private Screen(IScreenObjectProducer producer)
        {
            if (producer == null)
                throw new ArgumentNullException("producer");

            _screenObjectProducer = producer;
            LastIssuedAnimatonGroupID = ScreenAnimation.NoGroupId;
        }


        private void UpdateLastIssuedAnimationGroupID(ScreenObject screenObject)
        {
            if (screenObject.HasAnimation)
            {
                LastIssuedAnimatonGroupID = screenObject.Animation.GroupID;
            }
        }
    }
}
