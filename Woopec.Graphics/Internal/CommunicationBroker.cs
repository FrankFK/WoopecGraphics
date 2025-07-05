using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Woopec.Core.Internal
{
    /// <summary>
    /// An instance of this class transports data (ScreenObjects and ScreenResults) between the woopec-core-code and the woopec-drawing-code
    /// </summary>
    internal class CommunicationBroker
    {
        public IScreenObjectChannel ScreenObjectChannel { get; init; }
        public IScreenResultChannel ScreenResultChannel { get; init; }

        public IScreenObjectConsumer ScreenObjectConsumer { get; init; }


        /// <summary>
        /// Broker for the case where producers and consumers are in the same process
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="capacity"></param>
        public CommunicationBroker(IScreenObjectWriter writer, int capacity)
        {
            var channelOptions = new BoundedChannelOptions(capacity) { SingleReader = true, SingleWriter = false };
            ScreenObjectChannel = new ScreenObjectChannelWithinProcess(channelOptions);
            var screenObjectConsumer = new ScreenObjectConsumer(writer, ScreenObjectChannel);
            ScreenObjectConsumer = screenObjectConsumer;
            writer.OnAnimationIsFinished += screenObjectConsumer.AnimationOfGroupIsFinished;

            var resultChannelOptions = new BoundedChannelOptions(capacity) { SingleReader = true, SingleWriter = true };
            ScreenResultChannel = new ScreenResultChannelWithinProcess(resultChannelOptions);

            var resultProducer = new ScreenResultProducer(ScreenResultChannel);
            writer.SetScreenResultProducer(resultProducer);
        }

        /// <summary>
        /// Broker for the case where the producer is in this process and the writer should be started in a second process
        /// </summary>
        /// <param name="writerProcess">Process for the writer, the broker has to start it</param>
        /// <param name="startOptionForSecondProcess">Name of option for the process</param>
        public CommunicationBroker(Process writerProcess, string startOptionForSecondProcess)
        {
            var serverProcessChannelForScreenObjects = new ScreenObjectChannelInServerProcess();
            var serverProcessChannelForScreenResults = new ScreenResultChannelInServerProcess();

            writerProcess.StartInfo.ArgumentList.Add(startOptionForSecondProcess);
            writerProcess.StartInfo.ArgumentList.Add(serverProcessChannelForScreenObjects.Handle);
            writerProcess.StartInfo.ArgumentList.Add(serverProcessChannelForScreenResults.Handle);
            writerProcess.Start();
            serverProcessChannelForScreenObjects.DisposeHandle();
            serverProcessChannelForScreenResults.DisposeHandle();

            ScreenObjectChannel = serverProcessChannelForScreenObjects;
            ScreenResultChannel = serverProcessChannelForScreenResults;

            // The consumer is in the other process
            ScreenObjectConsumer = null;
        }

        /// <summary>
        /// Broker for the case where the producer is in another thread and the consumer is in this thread
        /// </summary>
        /// <param name="screenObjectPipeHandle"></param>
        /// <param name="screenResultPipeHandle"></param>
        /// <param name="writer"></param>
        public CommunicationBroker(string screenObjectPipeHandle, string screenResultPipeHandle, IScreenObjectWriter writer)
        {
            ScreenObjectChannel = new ScreenObjectChannelInClientProcess(screenObjectPipeHandle);

            var screenObjectConsumer = new ScreenObjectConsumer(writer, ScreenObjectChannel);
            ScreenObjectConsumer = screenObjectConsumer;
            writer.OnAnimationIsFinished += screenObjectConsumer.AnimationOfGroupIsFinished;

            var screenResultChannel = new ScreenResultChannelInClientProcess(screenResultPipeHandle);
            var resultProducer = new ScreenResultProducer(screenResultChannel);
            writer.SetScreenResultProducer(resultProducer);

        }
    }
}
