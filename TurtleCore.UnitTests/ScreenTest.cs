using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Woopec.Core.Internal;

namespace Woopec.Core.UnitTests
{
    [TestClass]
    public class ScreenTest
    {

        private class TurtleScreenProducerMockup : IScreenObjectProducer
        {
            public List<ScreenLine> DrawnLines = new();

            public int CreateLine()
            {
                return 1;
            }
            public void DrawLine(ScreenLine line)
            {
                DrawnLines.Add(line);
            }

            public int CreateFigure()
            {
                throw new NotImplementedException();
            }

            public void UpdateFigure(ScreenFigure figure)
            {
                throw new NotImplementedException();
            }

            public void ShowDialog(ScreenDialog dialog)
            {
                throw new NotImplementedException();
            }

            public void ShowNumberDialog(ScreenNumberDialog dialog)
            {
                throw new NotImplementedException();
            }
        }

        private class TurtleScreenResultConsumerMockup : IScreenResultConsumer
        {
            public Task<double?> ReadNumberResultAsync()
            {
                throw new NotImplementedException();
            }

            public Task<string> ReadTextResultAsync()
            {
                return Task.FromResult("Don't care");
            }
        }

        private static TurtleScreenProducerMockup _producerMockup;
        private static TurtleScreenResultConsumerMockup _resultConsumerMockup;

        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            _producerMockup = new TurtleScreenProducerMockup();
            _resultConsumerMockup = new TurtleScreenResultConsumerMockup();
            TurtleInputsAndOutputs.InitializeDefaultScreenObjectProducer(_producerMockup);
            TurtleInputsAndOutputs.InitializeDefaultScreenResultConsumer(_resultConsumerMockup);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            LowLevelScreen.ResetDefaultScreen();
        }

        [TestMethod]
        public void Test()
        {
            // No tests for screen at the moment
        }

    }
}
