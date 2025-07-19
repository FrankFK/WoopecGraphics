using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Graphics.Interface.Dtos;
using Woopec.Graphics.Helpers;
using Woopec.Graphics.Interface.Screen;

namespace Woopec.Graphics.Internal.Backend
{
    internal class LowLevelScreenFactory : ILowLevelScreenFactory
    {
        public ILowLevelScreen CreateLowLevelScreen()
        {
            return new LowLevelScreen(DefaultProducerAndConsumer.GetDefaultScreenObjectProducer(), DefaultProducerAndConsumer.GetDefaultScreenResultConsumer());
        }
    }

    /// <summary>
    /// In certain unit test scenarios, you want to mock producers and consumers. This is made possible by this class.
    /// </summary>
    internal class DefaultProducerAndConsumer
    {
        private static IScreenObjectProducer s_defaultScreenObjectProducer;
        private static IScreenResultConsumer s_defaultScreenResultConsumer;

        public static void InitializeDefaultScreenObjectProducer(IScreenObjectProducer screenObjectProducer)
        {
            s_defaultScreenObjectProducer = screenObjectProducer;
        }

        public static void InitializeDefaultScreenResultConsumer(IScreenResultConsumer screenResultConsumer)
        {
            s_defaultScreenResultConsumer = screenResultConsumer;
        }

        public static IScreenObjectProducer GetDefaultScreenObjectProducer()
        {
            return s_defaultScreenObjectProducer;
        }

        public static IScreenResultConsumer GetDefaultScreenResultConsumer()
        {
            return s_defaultScreenResultConsumer;
        }
    }
}
