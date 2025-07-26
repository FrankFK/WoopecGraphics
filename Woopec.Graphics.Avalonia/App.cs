using Avalonia.Themes.Fluent;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

internal class Program
{
    private static void Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddSingleton<SampleDataService>();

        var lifetime = new ClassicDesktopStyleApplicationLifetime { Args = args, ShutdownMode = ShutdownMode.OnLastWindowClose };

        var serviceProvider = services.BuildServiceProvider();

        AppBuilder.Configure<Application>()
            .UsePlatformDetect()
            .AfterSetup(b => b.Instance?.Styles.Add(new FluentTheme()))
            .UseServiceProvider(serviceProvider)
            .UseComponentControlFactory(new ControlFactory(serviceProvider))
            // uncomment the line below to enable rider ht reload workaround
            //.UseRiderHotReload()
            .SetupWithLifetime(lifetime);

        lifetime.MainWindow = new Window()
            .Title("Avalonia MVU Template")
            .Width(800)
            .Height(600)
            .Content(new MainView());

#if DEBUG
        lifetime.MainWindow.AttachDevTools();
#endif

        lifetime.Start(args);
    }
}

#if DEBUG

#endif

public class SampleDataService
{
    public string GetData() { return _value; }

    public void SetData(string value) { _value = value; }

    private string _value = "this text is from sample data service";
}

// This is a factory class that create controls and injects services into them
public class ControlFactory(IServiceProvider serviceProvider) : IComponentControlFactory
{
    public TControl CreateControlInstance<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TControl>() 
        where TControl : Control => ActivatorUtilities.CreateInstance<TControl>(serviceProvider);
}