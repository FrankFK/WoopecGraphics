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
        }

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
