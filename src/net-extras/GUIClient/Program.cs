using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.Globalization;
using Avalonia.DesignerSupport.Remote.HtmlTransport;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using Avalonia.Controls;
using Splat;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace GUIClient
{
    class Program
    {
        private const int TimeoutSeconds = 3;
        
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
        
            var mutex = new Mutex(false, typeof(Program).FullName);

            try
            {
                if (!mutex.WaitOne(TimeSpan.FromSeconds(TimeoutSeconds), true))
                {
                    return;
                }

                SubscribeToDomainUnhandledEvents();
                RegisterDependencies();
                //RunBackgroundTasks();

                BuildAvaloniaApp()
                    .StartWithClassicDesktopLifetime(args, ShutdownMode.OnMainWindowClose);
            }
            finally
            {
                mutex.ReleaseMutex();
            }
            
            /*
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);

            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddLocalization(options =>
                    {
                        options.ResourcesPath = "Resources";
                    });
                    
                    services.Configure<RequestLocalizationOptions>(options =>
                    {
                        var supportedCultures = new[]
                        {
                            new CultureInfo("en-US"),
                            new CultureInfo("pt-BR")
                        };

                        options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                        options.SupportedCultures = supportedCultures;
                        options.SupportedUICultures = supportedCultures;
                    });
                }).Build();
           
            IServiceProvider services = host.Services;
            
            ILogger logger =
                services.GetRequiredService<ILoggerFactory>()
                    .CreateLogger("GUIClient");
            */
            
            //host.Run();
        } 

        private static void SubscribeToDomainUnhandledEvents() =>
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var logger = Locator.Current.GetService<ILogger>();
                var ex = (Exception) args.ExceptionObject;

                logger.LogCritical($"Unhandled application error: {ex}");
            };
        
        private static void RegisterDependencies() =>
            Bootstrapper.Register(Locator.CurrentMutable, Locator.Current);
        
        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
    }
}