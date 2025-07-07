using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Graphics.LowLevelScreen;
using Woopec.Graphics.InternalBackend;

namespace Woopec.Graphics.Factories
{
    internal class LowLevelDefaultScreen
    {
        private static ILowLevelScreen _defaultScreen;

        /// <summary>
        /// Create a LowLevelScreen-Instance which draws to a default-Screen
        /// </summary>
        /// <returns></returns>
        internal static ILowLevelScreen Get()
        {
            if (_defaultScreen == null)
            {
                _defaultScreen = new InternalBackend.LowLevelScreen(DefaultProducerAndConsumer.GetDefaultScreenObjectProducer(), DefaultProducerAndConsumer.GetDefaultScreenResultConsumer());
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
