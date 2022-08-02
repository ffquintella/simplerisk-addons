using GUIClient.Services;
using Microsoft.Extensions.Logging;
using Splat;

namespace GUIClient;

public class GeneralServicesBootstrapper
{
    public static void RegisterServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton<ILocalizationService>(() => new LocalizationService(resolver.GetService<ILoggerFactory>()));
    }
}