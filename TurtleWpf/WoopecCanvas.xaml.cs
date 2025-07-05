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
using Woopec.Graphics;
using Woopec.Graphics.Internal;
using Colors = Woopec.Graphics.Colors;
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
            _canvas = new Canvas();
            // Frank 24.06.2023
            // - The _canvas is created without values for Width and Heigth, because the _canvas should adapt to the size of the UserControl WoopecCanvas
            this.Content = _canvas;

            _screenObjectWriter = new WpfScreenObjectWriter(_canvas);
            _screenObjectWriter.OnAnimationIsFinished += WhenWriterIsFinished;

            _communication = new Communication(_screenObjectWriter);
            _communication.StartProgram();
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            // Frank 24.06.2023
            // - This method is called when the UserControl is Loaded.
            // - It is important that the NextTask loop does not start until everything is rendered. Only then are the values for _canvas.ActualWidth and _canvas.ActualHeight
            //   are set. And this is important for the calculation in CanvasHelpers.ConvertToCanvasPoint()
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
