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
    /// An instance of this class transports ScreenObjects from (one or many) producers to (one) consumer
    /// </summary>
    internal class ScreenObjectBroker
    {
        public IChannel ObjectChannel { get; init; }

        public IScreenObjectConsumer Consumer { get; init; }


        /// <summary>
        /// Broker for the case where producers and consumers are in the same process
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="capacity"></param>
        public ScreenObjectBroker(IScreenObjectWriter writer, int capacity)
        {
            var channelOptions = new BoundedChannelOptions(capacity) { SingleReader = true, SingleWriter = false };
            ObjectChannel = new ChannelBetweenThreads(channelOptions);
            var screenObjectConsumer = new ScreenObjectConsumer(writer, ObjectChannel);
            Consumer = screenObjectConsumer;
            writer.OnAnimationIsFinished += screenObjectConsumer.AnimationOfGroupIsFinished;
        }

        /// <summary>
        /// Broker for the case where the producer is in this process and the writer should be started in a second process
        /// </summary>
        /// <param name="writerProcess">Process for the writer, the broker has to start it</param>
        /// <param name="startOptionForSecondProcess">Name of option for the process</param>
        public ScreenObjectBroker(Process writerProcess, string startOptionForSecondProcess)
        {
            var serverProcessChannel = new ProcessChannelInServer();
            writerProcess.StartInfo.ArgumentList.Add(startOptionForSecondProcess);
            writerProcess.StartInfo.ArgumentList.Add(serverProcessChannel.Handle);
            writerProcess.Start();
            serverProcessChannel.DisposeHandle();

            ObjectChannel = serverProcessChannel;

            // The consumer is in the other process
            Consumer = null;
        }

        /// <summary>
        /// Broker for the case where the producer is in another thread and the consumer is in this thread
        /// </summary>
        /// <param name="pipeHandle"></param>
        /// <param name="writer"></param>
        public ScreenObjectBroker(string pipeHandle, IScreenObjectWriter writer)
        {
            ObjectChannel = new ProcessChannelInClient(pipeHandle);

            var screenObjectConsumer = new ScreenObjectConsumer(writer, ObjectChannel);
            Consumer = screenObjectConsumer;
            writer.OnAnimationIsFinished += screenObjectConsumer.AnimationOfGroupIsFinished;
        }
    }
}
