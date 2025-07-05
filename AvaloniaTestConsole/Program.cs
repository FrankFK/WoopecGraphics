using Avalonia;
using Avalonia.ReactiveUI;
using Woopec.Core;

namespace AvaloniaTestConsole
{
    internal class Program
    {
        /*
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
        */
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace()
                .UseReactiveUI();

        public static void WoopecMain()
        {
            var firstTurtle = new Turtle() { Speed = Speeds.Slowest, IsVisible = false, IsDown = true };

            firstTurtle.Forward(200);
            firstTurtle.Left(90);
            firstTurtle.Forward(200);
        }
    }
}
