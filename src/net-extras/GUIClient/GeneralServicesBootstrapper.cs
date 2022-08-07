using GUIClient.Configuration;
using GUIClient.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Splat;

namespace GUIClient;

public class GeneralServicesBootstrapper
{
    
    public static void RegisterServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton<ILocalizationService>(() => new LocalizationService(resolver.GetService<ILoggerFactory>()));
        services.RegisterLazySingleton<IRegistrationService>(() => 
            new RegistrationService(resolver.GetService<ILoggerFactory>(), 
                resolver.GetService<IMutableConfigurationService>()));
        services.RegisterLazySingleton<IAuthenticationService>(() => new AuthenticationService(
            resolver.GetService<ILoggerFactory>(),
            resolver.GetService<IRegistrationService>()));

        services.RegisterLazySingleton<IRestService>(() => new RestService(
            resolver.GetService<ILoggerFactory>(),
            resolver.GetService<IAuthenticationService>(),
            resolver.GetService<IMutableConfigurationService>(),
            resolver.GetService<ServerConfiguration>()
            ));
        
    }

}