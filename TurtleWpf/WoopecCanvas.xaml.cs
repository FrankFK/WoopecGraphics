﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Woopec.Core;
using Woopec.Core.Internal;
using Colors = Woopec.Core.Colors;
using System.Reflection;

namespace Woopec.Wpf
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class WoopecCanvas : UserControl
    {
        private readonly Canvas _canvas;

        private readonly Communication _communication;
        private readonly WpfScreenObjectWriter _screenObjectWriter;
        /*
        private IScreenObjectProducer _actualProducer;
        private IScreenObjectConsumer _actualConsumer;
        */


        /// <summary>
        /// Constructor
        /// </summary>
        public WoopecCanvas()
        {
            InitializeComponent();
            _canvas = new Canvas() { Width = 400, Height = 400 };
            this.Content = _canvas;

            _screenObjectWriter = new WpfScreenObjectWriter(_canvas);
            _screenObjectWriter.OnAnimationIsFinished += WhenWriterIsFinished;

            _communication = new Communication(_screenObjectWriter);
            _communication.StartProgram();

            NextTask();

            /*
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
            */

        }

        /// <summary>
        /// Normal use of woopec:
        /// - The code that draws woopec-objects to the WPF-canvas runs in one thread.
        /// - The code that generates woopec-objects runs in a second thread
        /// - Both threads communicate through a System.Threading.Channels.Channel
        /// 
        /// This setting isn`t easy to debug (see ADR 003 for details)
        /// </summary>
        private void RunInSingleProcess()
        {
            /*
            // Generate a class that writes all objects to the wpf-canvas
            _screenObjectWriter = new WpfScreenObjectWriter(_canvas);
            _screenObjectWriter.OnAnimationIsFinished += WhenWriterIsFinished;

            // the broker transports screen objects from the producer(s) to the consumer. The consumer sends them to the writer.
            var objectBroker = new ScreenObjectBroker(_screenObjectWriter, 10000);
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
            */

            // The consumer runs in this thread. It waits asynchronically for the next object in the channel
            // and sends it to the writer
            NextTask();
        }

        /// <summary>
        /// Easier use of woopec (for debugging)
        /// - We start a new process that draws woopec-objects to the WPF-canvas
        /// - The code that generates woopec-objects runs in this process
        /// - Both threads communicate through an anonymous pipe
        /// 
        /// This setting is easier to debug (see ADR 003 for details)
        /// </summary>
        private void RunWithDrawingInSecondProcess(string startOptionForSecondProcess)
        {
            /*
            // Create a copy of the current executable
            var secondProcess = new Process();
            secondProcess.StartInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;

            // the broker start the secondProcess and creates a pipe from this process to the second process
            var objectBroker = new ScreenObjectBroker(secondProcess, startOptionForSecondProcess);

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

            // This thread can sleep for ever because the woopec-objects are drawn by a second process
            Thread.Sleep(int.MaxValue);
            */
        }

        /// <summary>
        /// The counterpart of <c>RunWithDrawingInSecondProcess</c>:
        /// This code runs in the new process that reads woopec-objects from the pipe and draws them to this canvas.
        /// </summary>
        private void ReadObjectsFromPipeAndDrawThem(string pipeHandle)
        {
            /*
            // Generate a class that writes all objects to the wpf-canvas
            _screenObjectWriter = new WpfScreenObjectWriter(_canvas);
            _screenObjectWriter.OnAnimationIsFinished += WhenWriterIsFinished;

            // the broker transports screen objects from the producer(s) to the consumer. The consumer sends them to the writer.
            var objectBroker = new ScreenObjectBroker(pipeHandle, _screenObjectWriter);
            // the one and only consumer
            _actualConsumer = objectBroker.Consumer;
            */
            // The consumer runs in this thread. It waits asynchronically for the next object in the channel
            // and sends it to the writer
            NextTask();
        }

        /*
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
        */

        private void NextTask()
        {
            var task = _communication.ConsumeNextScreenObjectAsync();
            task.ContinueWith((t) =>
            {
                // Aus https://igorpopov.io/2018/06/16/asynchronous-programming-in-csharp-with-wpf/
                Dispatcher.Invoke(() =>
                {
                    if (t.IsFaulted)
                    {
                        throw new Exception($"Error while handling screen object: {t.Exception.InnerException.Message}");
                    }
                    NextTask();
                });
            });
        }

        private void WhenWriterIsFinished(int groupId, int objectId)
        {
            Debug.WriteLine($"{objectId} is finished");
        }

    }
}
