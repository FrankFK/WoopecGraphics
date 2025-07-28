using System.Diagnostics.CodeAnalysis;
using Avalonia.Themes.Fluent;
using Microsoft.Extensions.DependencyInjection;

namespace Woopec.Graphics.Avalonia
{
    public class Frontend
    {
        public static void Initialize(string[] args)
        {
            var config = new Internal.Frontend.Configuration();
            var services = new ServiceCollection();
            services.AddSingleton<WoopecGraphicsComponentDataService>();

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
                .Title(config.Title)
                .Width(config.Width)
                .Height(config.Height)
                .Content(new MainView());

#if DEBUG
            lifetime.MainWindow.AttachDevTools();
#endif

            lifetime.Start(args);
        }
    }

    // This is a factory class that create controls and injects services into them
    public class ControlFactory(IServiceProvider serviceProvider) : IComponentControlFactory
    {
        public TControl CreateControlInstance<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TControl>()
            where TControl : Control => ActivatorUtilities.CreateInstance<TControl>(serviceProvider);
    }

}
