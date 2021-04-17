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

namespace TurtleWpf
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class TurtleCanvas : UserControl
    {
        private readonly Canvas _canvas;
        private readonly ScreenOutput _turtleScreenOutput;
        private readonly Channel<ScreenObject> _objectChannel;
        private readonly Thread _turtleThread;


        public TurtleCanvas()
        {
            InitializeComponent();
            _canvas = new Canvas() { Width = 400, Height = 400 };
            this.Content = _canvas;

            var channelOptions = new BoundedChannelOptions(1000) { SingleReader = true, SingleWriter = true };
            _objectChannel = Channel.CreateBounded<ScreenObject>(channelOptions);
            _turtleScreenOutput = new ScreenOutput(_canvas, _objectChannel);
            TurtleOutputs.InitializeDefaultScreen(_turtleScreenOutput);

            _turtleThread = new Thread(new ThreadStart(TestProgram));
            _turtleThread.Start();

            var task = _turtleScreenOutput.ReadScreenObjectAsync();

            task.ContinueWith((t) =>
               {
                   // Aus https://igorpopov.io/2018/06/16/asynchronous-programming-in-csharp-with-wpf/
                   Dispatcher.Invoke(() =>
                   {
                       _turtleScreenOutput.DrawScreenObject(t.Result);
                   });
               });
        }

        private void TestProgram()
        {
            var turtle = new Turtle();

            turtle.Right(45);
            turtle.Forward(50);
            turtle.Left(90);
            turtle.Forward(100);
            turtle.Right(45);
            turtle.Forward(20);

        }
    }
}
