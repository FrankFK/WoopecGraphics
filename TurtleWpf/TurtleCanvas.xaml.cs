using System;
using System.Collections.Generic;
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
using TurtleCore;
using TurtleCore.Internal;
using Colors = TurtleCore.Colors;

namespace TurtleWpf
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class TurtleCanvas : UserControl
    {
        private readonly Canvas _canvas;
        private readonly IScreenObjectProducer _actualProducer;
        private readonly IScreenObjectConsumer _actualConsumer;
        private readonly WpfScreenObjectWriter _screenObjectWriter;

        public TurtleCanvas()
        {
            InitializeComponent();
            _canvas = new Canvas() { Width = 400, Height = 400 };
            this.Content = _canvas;

            // generate a mockup-object, that simulation the draw-operations on the screen (in reality this would be a wpf-canvas)
            _screenObjectWriter = new WpfScreenObjectWriter(_canvas);
            _screenObjectWriter.OnAnimationIsFinished += WhenWriterIsFinished;

            // the broker transports screen objects from the producer(s) to the consumer. The consumer sends them to the writer.
            // to-do: check, if an unbound broker is better
            var objectBroker = new ScreenObjectBroker(_screenObjectWriter, 10000);
            // the one and only consumer
            _actualConsumer = objectBroker.Consumer;

            // It is possible to have multiple producers. In this test we only have one
            // This producer runs in a another thread.
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
            /////////////////////////////////////////////////////////////
            // 08.03.2021: First turtle with WPF
            var firstTurtle = new Turtle() { Speed = SpeedLevel.Slowest };

            firstTurtle.Right(45);
            firstTurtle.Forward(50);
            firstTurtle.Left(90);
            firstTurtle.Forward(100);
            firstTurtle.Right(45);
            firstTurtle.Forward(20);

            /////////////////////////////////////////////////////////////
            // 1.5.2021: First version of animation handling
            // - Animations of the same turtle follows one after the other
            // - Animations of different turtles are printed in parallel
            var turtles = new List<Turtle>();
            for (var counter = 0; counter < 10; counter++)
            {
                turtles.Add(new Turtle() { IsDown = false, });
            }

            // Move all turtles to the same position as firstTurtle
            foreach (var turtle in turtles)
            {
                turtle.Right(45);
                turtle.Forward(50);
                turtle.Left(90);
                turtle.Forward(100);
                turtle.Right(45);
                turtle.Forward(20);
            }

            foreach (var t in turtles)
                t.PenDown();
            // The turtles are moving in parallel
            for (var j = 0; j < 40; j++)
            {
                for (var index = 0; index < turtles.Count; index++)
                {
                    var turtle2 = turtles[index];
                    turtle2.Left(1 + index * 0.2);
                    turtle2.Forward(5);

                    if (j == 5) turtle2.PenColor = Colors.Green;    // 04.05.2021: Predefined Colors
                }
            }

            //////////////////////////////////////////////////////////////
            // 5.5.2021: Different Speeds
            turtles[0].Speed = SpeedLevel.Slowest; turtles[0].PenColor = Colors.DarkGreen;
            turtles[1].Speed = SpeedLevel.Slow; turtles[1].PenColor = Colors.DarkRed;
            turtles[2].Speed = SpeedLevel.Normal; turtles[2].PenColor = Colors.DarkOrange;
            turtles[3].Speed = SpeedLevel.Fast; turtles[3].PenColor = Colors.DarkBlue;
            for (var speedIndex = 0; speedIndex <= 3; speedIndex++)
            {
                var speedTurtle = turtles[speedIndex];
                speedTurtle.Right(135 + speedIndex * 5);
                speedTurtle.Forward(200);
                speedTurtle.Right(90);
                speedTurtle.Forward(200);
            }

            /////////////////////////////////////////////////////////////
            // 10.05.2021 Penup and Pendown
            turtles[0].PenUp();
            turtles[0].Forward(50);
            turtles[0].PenDown();
            turtles[0].Forward(50);

            /////////////////////////////////////////////////////////////
            // 10.05.2021 Position
            var newPosition = turtles[0].Position + new Vec2D(-20, 0);
            turtles[0].Position = newPosition;

        }

        private void NextTask()
        {
            var task = _actualConsumer.HandleNextScreenObjectAsync();
            task.ContinueWith((t) =>
            {
                // Aus https://igorpopov.io/2018/06/16/asynchronous-programming-in-csharp-with-wpf/
                Dispatcher.Invoke(() =>
                {
                    NextTask();
                });
            });
        }

        private void WhenWriterIsFinished(int groupId, int objectId)
        {
            Console.WriteLine($"{objectId} is finished");
        }

    }
}
