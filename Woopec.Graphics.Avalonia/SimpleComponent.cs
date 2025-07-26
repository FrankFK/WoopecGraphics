using Avalonia.Controls;
using Avalonia.Markup.Declarative;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

//prevent from trimming [injected] services by native aot compilation
[method: DynamicDependency(DynamicallyAccessedMemberTypes.PublicProperties, typeof(SimpleComponent))]
public class SimpleComponent(SampleDataService dataService) : ComponentBase //constructor dependency injection sample
{
    // You can also use Service injection into Property with DI container as follows:
    [Inject] public SampleDataService? DataService { get; set; }

    //Styles
    protected override StyleGroup? BuildStyles() =>
    [
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
    ];

    //Markup
    protected override object Build() =>
        new Grid().Cols("150, *")
            .BindClass(() => Bounds.Width < 400, "narrow")
            .Children(
                new StackPanel()
                    .Name("SideBar")
                    .Background(Brushes.CadetBlue)
                    .Children(
                        new TextBlock()
                            .Name("title")
                            .Margin(top: 16, left: 16) //partial margin defined with named arguments
                            .Text("Sidebar")
                    ),

                new StackPanel().Col(1)
                    .VerticalAlignment(VerticalAlignment.Center)
                    .HorizontalAlignment(HorizontalAlignment.Center)
                    .Children(
                        new TextBlock().Ref(out _textBlock1)
                            .Text("Hello world"),
                        new TextBlock().Ref(out _textBlock2)
                            .Text("TextBlock2"),
                        new TextBlock()
                            .Text(() => $"Counter: {(Counter == 0 ? "zero" : Counter)}"), //expression binding with dynamic string result
                        new NumericUpDown()
                            .Value(() => Counter, onChanged: v => Counter = v), //two-way binding sample
                        new Button()
                            .HorizontalAlignment(HorizontalAlignment.Center)
                            .Content("Click me")
                            .OnClick(OnButtonClick) //direct event callback
                    )
            );

    //Code
    private TextBlock _textBlock1 = null!;

    private TextBlock _textBlock2 = null!;

    private decimal? Counter { get; set; } = 0;

    private void OnButtonClick(RoutedEventArgs e)
    {
        _textBlock1.Text = dataService?.GetData() ?? "Data service is `null`";
        StateHasChanged();
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        //force recalculation on window width to check if it's Narrow state now
        StateHasChanged();
        base.OnSizeChanged(e);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        // Frank 24.06.2023
        // - This method is called when the UserControl is Loaded.
        // - It is important that the NextTask loop does not start until everything is rendered. Only then are the values for _canvas.ActualWidth and _canvas.ActualHeight
        //   are set. And this is important for the calculation in CanvasHelpers.ConvertToCanvasPoint()
        DispatchNextTask();

        // var width = MainCanvas.Width;
        // Debug.WriteLine($"Width: {width}");
        Debug.WriteLine($"Hello?");
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
            await Task.Delay(5000);
            dataService.SetData(DateTime.Now.ToString());
            StateHasChanged(); // <-- Das ändert nichts
            _textBlock2.Text = DateTime.Now.ToString(); // <-- Das ändert etwas!
            Debug.WriteLine($"Consumer: returned");
            return;
        }
    }

    private void ShowErrorMessage(string? message)
    {

        // Create the window object
        // To do: Da muss noch eine TextBox rein....
        Window sampleWindow =
            new Window
            {
                Title = message,
                Width = 200,
                Height = 200
            };


        // open the modal (dialog) window
        //
        // unklar, wo man das Window herbekommt, vielleicht so:
        // var x = Avalonia.Application.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        // sampleWindow.ShowDialog(x.MainWindow);
        // var window = this.GetVisualRoot();
        if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (desktop.MainWindow != null)
                sampleWindow.ShowDialog(desktop.MainWindow);
        }

    }



}