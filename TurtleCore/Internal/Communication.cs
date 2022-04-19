using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Woopec.Core.Internal
{
    internal class Communication
    {
        private IScreenObjectProducer _actualProducer;
        private IScreenObjectConsumer _actualConsumer;
        private readonly IScreenObjectWriter _actualWriter;
        private ScreenObjectBroker _actualScreenObjectBroker;

        public Communication(IScreenObjectWriter writer)
        {
            _actualWriter = writer;
        }

        /// <summary>
        /// - Create a producer and start the turtle/woopec program in it.
        /// - Create a consumer which reads the generated objects and renders it
        /// - Create a communication channel between producer and consumer
        /// </summary>
        public void StartProgram()
        {
            var readFromPipeOption = "--read_from_pipe";
            string[] arguments = Environment.GetCommandLineArgs();
            string pipeHandle = null;
            if (arguments.Length > 2 && arguments[1] == readFromPipeOption && arguments[2] != null)
            {
                pipeHandle = arguments[2];

            }

            if (pipeHandle != null)
            {
                // This process reads the woopec-objects from a pipe. Another process puts them to this pipe. (Useful setting for debugging)
                ReadObjectsFromPipeAndDrawThem(pipeHandle);
            }
            else
            {
                if (Debugger.IsAttached)
                {
                    // Debugging is easier, if this process only produces the woopec-objects and another process draws them
                    RunWithDrawingInSecondProcess(readFromPipeOption);
                }
                else
                {
                    // Normal use (best performance, but difficult to debug)
                    RunInSingleProcess();
                }
            }
        }

        /// <summary>
        /// When used in WPF: Consume the next object.
        /// </summary>
        /// <returns></returns>
        public Task ConsumeNextScreenObjectAsync() => _actualConsumer.HandleNextScreenObjectAsync();

        /// <summary>
        /// An experiment: Initalization routine for a woopec program which runs as an console program and uses a second
        /// process as renderer (the same concept as in <c>RunWithDrawingInSecondProcess</c>)
        /// </summary>
        public void InitForConsoleProgram()
        {
            // Create a process that renders to WPF
            // This process has to be installed somehow. At the moment it isn't.
            var secondProcess = new Process();
            secondProcess.StartInfo.FileName = @"C:\Users\frank\source\repos\simple-graphics-for-csharp-beginners\UsingTurtleCanvas\bin\Debug\net6.0-windows\UsingTurtleCanvas.exe";

            // the broker start the secondProcess and creates a pipe from this process to the second process
            _actualScreenObjectBroker = new ScreenObjectBroker(secondProcess, "--read_from_pipe");

            // It is possible to have multiple producers. In this case we ony have one.
            // This producer runs in another thread.
            _actualProducer = new ScreenObjectProducer(_actualScreenObjectBroker.ObjectChannel);
            TurtleOutputs.InitializeDefaultScreenObjectProducer(_actualProducer);
        }


        /// <summary>
        /// Most performant use of woopec
        /// - We start a new thread that that generates woopec-objects (producer)
        /// - This thread (consumer) reads this objects by calling ConsumeNextScreenObjectAsync() and draws them to the WPF-canvas
        /// - Both threads communicate through a channel
        /// </summary>
        private void RunInSingleProcess()
        {
            // the broker transports screen objects from the producer(s) to the consumer. The consumer sends them to the writer.
            var objectBroker = new ScreenObjectBroker(_actualWriter, 10000);
            // the one and only consumer
            _actualConsumer = objectBroker.Consumer;

            // It is possible to have multiple producers. In this case we ony have one.
            // This producer runs in another thread.
            _actualProducer = new ScreenObjectProducer(objectBroker.ObjectChannel);
            TurtleOutputs.InitializeDefaultScreenObjectProducer(_actualProducer);
            var producerThread = new Thread(
                        new ThreadStart(() =>
                        {
                            StartWoopecProgramm();
                        }
                    )
                );
            producerThread.Start();
        }

        /// <summary>
        /// Easier use of woopec (for debugging)
        /// - We start a new process that draws woopec-objects to the WPF-canvas
        /// - The code that generates woopec-objects runs in this process
        /// - Both processes communicate through an anonymous pipe
        /// 
        /// This setting is easier to debug (see ADR 003 for details)
        /// </summary>
        private void RunWithDrawingInSecondProcess(string startOptionForSecondProcess)
        {
            // Create a copy of the current executable
            var secondProcess = new Process();
            secondProcess.StartInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;

            // the broker start the secondProcess and creates a pipe from this process to the second process
            _actualScreenObjectBroker = new ScreenObjectBroker(secondProcess, startOptionForSecondProcess);

            // It is possible to have multiple producers. In this case we ony have one.
            // This producer runs in another thread.
            _actualProducer = new ScreenObjectProducer(_actualScreenObjectBroker.ObjectChannel);
            TurtleOutputs.InitializeDefaultScreenObjectProducer(_actualProducer);
            var producerThread = new Thread(
                        new ThreadStart(() =>
                        {
                            StartWoopecProgramm();
                        }
                    )
                );
            producerThread.Start();

            // This thread can sleep for ever because the woopec-objects are drawn by a second process
            Thread.Sleep(int.MaxValue);
        }

        /// <summary>
        /// The counterpart of <c>RunWithDrawingInSecondProcess</c>:
        /// This code runs in the new process that reads woopec-objects from the pipe and draws them to this canvas.
        /// </summary>
        private void ReadObjectsFromPipeAndDrawThem(string pipeHandle)
        {
            // the broker transports screen objects from the producer(s) to the consumer. The consumer sends them to the writer.
            var objectBroker = new ScreenObjectBroker(pipeHandle, _actualWriter);
            // the one and only consumer
            _actualConsumer = objectBroker.Consumer;
        }


        private static void StartWoopecProgramm()
        {
            var foundTurtleCode = WoopecCodeFinder.Find();

            if (foundTurtleCode != null)
            {
                foundTurtleCode.Invoke();
            }
            else
            {
                var firstTurtle = new Turtle() { Speed = Speeds.Slowest, Shape = Shapes.Turtle, Color = Colors.DarkRed };
            }

            return;
        }

    }
}
