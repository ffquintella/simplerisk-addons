using GUIClient.Services;
using Microsoft.Extensions.Logging;
using Splat;

namespace GUIClient;

public class GeneralServicesBootstrapper
{
    
    public static void RegisterServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton<ILocalizationService>(() => new LocalizationService(resolver.GetService<ILoggerFactory>()));
        services.RegisterLazySingleton<IRegistrationService>(() => 
            new RegistrationService(GetService<ILoggerFactory>(), 
                GetService<IMutableConfigurationService>()));
        services.RegisterLazySingleton<IAuthenticationService>(() => new AuthenticationService(
            GetService<ILoggerFactory>(),
            GetService<IRegistrationService>()));

    }
    
    private static T GetService<T>() => Locator.Current.GetService<T>();

}