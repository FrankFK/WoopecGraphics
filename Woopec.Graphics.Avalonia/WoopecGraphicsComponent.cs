using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Declarative;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using Avalonia.Threading;

namespace Woopec.Graphics.Avalonia
{
    //prevent from trimming [injected] services by native aot compilation
    [method: DynamicDependency(DynamicallyAccessedMemberTypes.PublicProperties, typeof(WoopecGraphicsComponent))]
    public class WoopecGraphicsComponent(WoopecGraphicsComponentDataService dataService) : ComponentBase //constructor dependency injection sample
    {
        // You can also use Service injection into Property with DI container as follows:
        [Inject] public WoopecGraphicsComponentDataService? DataService { get; set; }

        //Styles
        protected override StyleGroup? BuildStyles() =>
        [
            /*
            new Style<Button>(x => x.Class(":pointerover").Descendant())
            .Background(Brushes.LightBlue),

        new Style<Button>()
            .Margin(6)
            .Background(Brushes.DarkSalmon),

        new Style<TextBlock>(s => s.OfType<StackPanel>().Name("SideBar").Descendant())
            .FontSize(16)
            .Foreground(Brushes.White),

        new StyleGroup(x => x.Class("narrow").Descendant())
        {
            new Style<StackPanel>(s => s.Name("SideBar"))
                .IsVisible(false)
        }
            */
        ];

        //Markup
        protected override object Build()
        {
            var markup = new Grid().Cols("*")
                .Children(
                    new Grid().Col(0).Rows("*")
                        .Children(
                            // new TextBlock().Col(1).Row(0).Text("Hallo")),
                            new Canvas().Ref(out _canvas).Col(0).Row(0).Background(new SolidColorBrush(Colors.SandyBrown))
                        )
                );
            _canvas.LayoutUpdated += OnCanvasLayoutUpdatedHandler;
            return markup;
        }

        //Code
        private bool _dispatcherIsStarted = false;
        private Canvas _canvas = null;

        private void OnCanvasLayoutUpdatedHandler(object? sender, System.EventArgs e)
        {
            // Frank 24.06.2023
            // - This method is called when the UserControl is Loaded.
            // - It is important that the NextTask loop does not start until everything is rendered. Only then are the values for _canvas.ActualWidth and _canvas.ActualHeight
            //   are set. And this is important for the calculation in CanvasHelpers.ConvertToCanvasPoint()

            var width = _canvas.Bounds.Width;
            Debug.WriteLine($"Width: {width}");
            if (!_dispatcherIsStarted)
            {
                _dispatcherIsStarted = true;
                DispatchNextTask();
            }
        }

        private void DispatchNextTask()
        {
            Dispatcher.UIThread.Post(DoNextTaskAndDispatchOvernextTask);   // see: https://docs.avaloniaui.net/docs/guides/development-guides/accessing-the-ui-thread
        }

        private void DoNextTaskAndDispatchOvernextTask()
        {
            var task = HandleNextScreenObjectAsync();
            // var task = _woopecCoreCommunication.ConsumeNextScreenObjectAsync();

            task.ContinueWith((t) =>
            {
                if (t.IsFaulted)
                {
                    // Exception werfen hilft nicht. Hier müsste eine Meldung auf dem Bildschirm angezeigt werden. Allerdings hat Avalonia aktuell keine MessageBox, dann wär's einfach
                    // throw new Exception($"Error while handling screen object: {t.Exception?.InnerException?.Message}");
                    var message = t.Exception?.InnerException?.Message;
                    Dispatcher.UIThread.Post(() => { ShowErrorMessage(message); });
                    return;
                }
                DispatchNextTask();
            }
            );
        }

        private async Task HandleNextScreenObjectAsync()
        {
            while (true)
            {
                Debug.WriteLine($"Consumer: Read async started ");
                await Task.Delay(2000);
                // _textBlock2.Text = DateTime.Now.ToString() + "w:" + _canvas.Bounds.Width.ToString(); // <-- Das ändert etwas!
                throw new NotImplementedException("Hallo");
                Debug.WriteLine($"Consumer: returned");
                return;
            }
        }

        private void ShowErrorMessage(string? message)
        {
            var errorWindow = new ErrorWindow(message);
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (desktop.MainWindow != null)
                    errorWindow.ShowDialog(desktop.MainWindow);
            }

        }
    }
}
