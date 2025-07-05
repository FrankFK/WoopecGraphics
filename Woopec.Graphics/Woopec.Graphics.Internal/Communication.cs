using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Woopec.Graphics.Internal
{
    internal class Communication
    {
        private IScreenObjectProducer _actualScreenObjectProducer;
        private IScreenObjectConsumer _actualScreenObjectConsumer;
        private readonly IScreenObjectWriter _actualScreenObjectWriter;
        private CommunicationBroker _actualBroker;

        public Communication(IScreenObjectWriter writer)
        {
            _actualScreenObjectWriter = writer;
        }

        public void StartProgram()
        {
            StartProgram(true);
        }

        /// <summary>
        /// - Create a producer and start the turtle/woopec program in it.
        /// - Create a consumer which reads the generated objects and renders it
        /// - Create a communication channel between producer and consumer
        /// </summary>
        /// <param name="inDebugModeStartRendererProcess">If a debugger ist attached and this value is true, the renderer is started in a second process (that makes debugging much easier for programming beginners)</param>
        public void StartProgram(bool inDebugModeStartRendererProcess)
        {
            var usePipesOption = "--use_pipes";
            string[] arguments = Environment.GetCommandLineArgs();
            string screenObjectPipeHandle = null;
            string screenResultPipeHandle = null;
            if (arguments.Length > 3 && arguments[1] == usePipesOption && arguments[2] != null && arguments[3] != null)
            {
                screenObjectPipeHandle = arguments[2];
                screenResultPipeHandle = arguments[3];

            }

            if (screenObjectPipeHandle != null && screenResultPipeHandle != null)
            {
                // This process reads the woopec-objects from a pipe. Another process puts them to this pipe. (Useful setting for debugging)
                ReadObjectsFromPipeAndDrawThem(screenObjectPipeHandle, screenResultPipeHandle);
            }
            else
            {
                if (Debugger.IsAttached && inDebugModeStartRendererProcess)
                {
                    // Debugging is easier, if this process only produces the woopec-objects and another process draws them
                    RunWithDrawingInSecondProcess(usePipesOption);
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
        public Task ConsumeNextScreenObjectAsync() => _actualScreenObjectConsumer.HandleNextScreenObjectAsync();

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
            _actualBroker = new CommunicationBroker(secondProcess, "--use_pipes");

            // It is possible to have multiple producers. In this case we ony have one.
            // This producer runs in another thread.
            _actualScreenObjectProducer = new ScreenObjectProducer(_actualBroker.ScreenObjectChannel);
            TurtleInputsAndOutputs.InitializeDefaultScreenObjectProducer(_actualScreenObjectProducer);

            var resultConsumer = new ScreenResultConsumer(_actualBroker.ScreenResultChannel);
            TurtleInputsAndOutputs.InitializeDefaultScreenResultConsumer(resultConsumer);

        }


        /// <summary>
        /// Most performant use of woopec
        /// - We start a new thread that that generates woopec-objects (producer)
        /// - This thread (consumer) reads this objects by calling ConsumeNextScreenObjectAsync() and draws them to the WPF-canvas
        /// - Both threads communicate through a channel
        /// </summary>
        private void RunInSingleProcess()
        {
            const int capacity = 10000;
            // the screen object broker transports screen objects from the producer(s) to the consumer. The consumer sends them to the writer.
            var communicationBroker = new CommunicationBroker(_actualScreenObjectWriter, capacity);
            // the one and only consumer
            _actualScreenObjectConsumer = communicationBroker.ScreenObjectConsumer;

            // It is possible to have multiple screen object producers. In this case we only have one.
            // This producer runs in another thread.
            _actualScreenObjectProducer = new ScreenObjectProducer(communicationBroker.ScreenObjectChannel);
            TurtleInputsAndOutputs.InitializeDefaultScreenObjectProducer(_actualScreenObjectProducer);

            // the screen result broker transports screen results (e.g. answer in a text input dialog windos) from the screen thread to the screen object producing thread
            var resultConsumer = new ScreenResultConsumer(communicationBroker.ScreenResultChannel);
            TurtleInputsAndOutputs.InitializeDefaultScreenResultConsumer(resultConsumer);


            var screenObjectProducingThread = new Thread(
                        new ThreadStart(() =>
                        {
                            StartWoopecProgramm();
                        }
                    )
                );
            screenObjectProducingThread.Start();
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
            _actualBroker = new CommunicationBroker(secondProcess, startOptionForSecondProcess);

            // It is possible to have multiple producers. In this case we ony have one.
            // This producer runs in another thread.
            _actualScreenObjectProducer = new ScreenObjectProducer(_actualBroker.ScreenObjectChannel);
            TurtleInputsAndOutputs.InitializeDefaultScreenObjectProducer(_actualScreenObjectProducer);

            var resultConsumer = new ScreenResultConsumer(_actualBroker.ScreenResultChannel);
            TurtleInputsAndOutputs.InitializeDefaultScreenResultConsumer(resultConsumer);

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
        private void ReadObjectsFromPipeAndDrawThem(string screenObjectPipeHandle, string screenResultPipeHandle)
        {
            // the broker transports screen objects from the producer(s) to the consumer. The consumer sends them to the writer.
            var objectBroker = new CommunicationBroker(screenObjectPipeHandle, screenResultPipeHandle, _actualScreenObjectWriter);
            // the one and only consumer
            _actualScreenObjectConsumer = objectBroker.ScreenObjectConsumer;
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
