using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Woopec.Graphics.Internal.Backend;
using Woopec.Graphics.Internal.Frontend;

namespace Woopec.Graphics.Internal.Communication
{
    /// <summary>
    /// An instance of this class transports data (ScreenObjects and ScreenResults) between the woopec-core-code and the woopec-drawing-code
    /// </summary>
    internal class CommunicationBroker
    {
        public IScreenObjectChannelForWriter ScreenObjectChannelForWriter { get; init; }
        public IScreenObjectChannelForReader ScreenObjectChannelForReader { get; init; }

        public IScreenResultChannelForWriter ScreenResultChannelForWriter { get; init; }
        public IScreenResultChannelForReader ScreenResultChannelForReader{ get; init; }

        public IScreenObjectConsumer ScreenObjectConsumer { get; init; }


        /// <summary>
        /// Broker for the case where producers and consumers are in the same process
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="capacity"></param>
        public CommunicationBroker(IScreenObjectWriter writer, int capacity)
        {
            var channelOptions = new BoundedChannelOptions(capacity) { SingleReader = true, SingleWriter = false };
            var screenObjectChannel = new ScreenObjectChannelWithinProcess(channelOptions);
            ScreenObjectChannelForWriter = screenObjectChannel;
            ScreenObjectChannelForReader = screenObjectChannel;
            var screenObjectConsumer = new ScreenObjectConsumer(writer, ScreenObjectChannelForReader);
            ScreenObjectConsumer = screenObjectConsumer;
            writer.OnAnimationIsFinished += screenObjectConsumer.AnimationOfGroupIsFinished;

            var resultChannelOptions = new BoundedChannelOptions(capacity) { SingleReader = true, SingleWriter = true };
            var screenResultChannel = new ScreenResultChannelWithinProcess(resultChannelOptions);

            ScreenResultChannelForReader = screenResultChannel;
            ScreenResultChannelForWriter = screenResultChannel;

            var resultProducer = new ScreenResultProducer(ScreenResultChannelForWriter);
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

            ScreenObjectChannelForWriter = serverProcessChannelForScreenObjects;
            ScreenObjectChannelForReader = serverProcessChannelForScreenObjects;

            ScreenResultChannelForReader = serverProcessChannelForScreenResults;
            ScreenResultChannelForWriter = serverProcessChannelForScreenResults;

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
            var screenObjectChannel = new ScreenObjectChannelInClientProcess(screenObjectPipeHandle);
            ScreenObjectChannelForReader = screenObjectChannel;
            ScreenObjectChannelForWriter = screenObjectChannel;

            var screenObjectConsumer = new ScreenObjectConsumer(writer, ScreenObjectChannelForReader);
            ScreenObjectConsumer = screenObjectConsumer;
            writer.OnAnimationIsFinished += screenObjectConsumer.AnimationOfGroupIsFinished;

            var screenResultChannel = new ScreenResultChannelInClientProcess(screenResultPipeHandle);

            ScreenResultChannelForWriter = screenResultChannel;
            ScreenResultChannelForReader = screenResultChannel;
            var resultProducer = new ScreenResultProducer(ScreenResultChannelForWriter);
            writer.SetScreenResultProducer(resultProducer);

        }
    }
}
