using System;
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

namespace Woopec.Wpf
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class WoopecCanvas : UserControl
    {
        private readonly Canvas _canvas;
        private readonly IScreenObjectProducer _actualProducer;
        private readonly IScreenObjectConsumer _actualConsumer;
        private readonly WpfScreenObjectWriter _screenObjectWriter;

        public WoopecCanvas()
        {
            InitializeComponent();
            _canvas = new Canvas() { Width = 400, Height = 400 };
            this.Content = _canvas;

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
                            TestProgram();
                        }
                    )
                );
            producerThread.Start();

            // The consumer runs in this thread. It waits asynchronically for the next object in the channel
            // and sends it to the writer
            NextTask();
        }


        private static void TestProgram()
        {
            var foundTurtleCode = TurtleCodeFinder.Find();

            if (foundTurtleCode != null)
            {
                foundTurtleCode.Invoke();
            }
            else
            {
                var firstTurtle = new Turtle() { Speed = SpeedLevel.Slowest, Shape = Shapes.Turtle, Color = Colors.DarkGreen };
                firstTurtle.Right(45);
                firstTurtle.Forward(50);
                firstTurtle.Left(90);
                firstTurtle.Forward(100);
                firstTurtle.Right(45);
                firstTurtle.Forward(20);
            }

            return;
        }

        private void NextTask()
        {
            var task = _actualConsumer.HandleNextScreenObjectAsync();
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
