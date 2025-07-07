using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Graphics.CommunicatedObjects;

namespace Woopec.Graphics.Helpers
{
    internal class LowLevelDefaultScreen
    {
        private static ILowLevelScreen _defaultScreen;
        private static ILowLevelScreenFactory _factory;

        internal static void Init(ILowLevelScreenFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Create a LowLevelScreen-Instance which draws to a default-Screen
        /// </summary>
        /// <returns></returns>
        internal static ILowLevelScreen Get()
        {
            if (_defaultScreen == null)
            {
                if (_factory == null)
                {
                    // This situation occurs if Figure, Pen or Turtle are used in unit tests without any preperation for that.
                    var recommendedClass = nameof(Screen);
                    var recommendedMethod = nameof(Screen.SwitchToUnitTestDefaultScreen);
                    throw new ArgumentException($"Woopec: The arguments for LowLevelScreen are invalid. If you need a screen for unit tests, it is best to call method {recommendedMethod} of class {recommendedClass} at the beginning.");
                }
                _defaultScreen = _factory.CreateLowLevelScreen();
            }
            return _defaultScreen;
        }

        /// <summary>
        /// Set a LowLevelScreen which can be used for unit tests
        /// </summary>
        internal static void SetUnitTestScreen()
        {
            _defaultScreen = new LowLevelScreenForUnitTests();
        }


        /// <summary>
        /// Needed for tests
        /// </summary>
        internal static void Reset()
        {
            _defaultScreen = null;
        }
    }
}
