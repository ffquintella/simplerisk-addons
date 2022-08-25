using GUIClient.Configuration;
using GUIClient.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Splat;
using ILogger = Serilog.ILogger;

namespace GUIClient;

public class GeneralServicesBootstrapper
{
    
    public static void RegisterServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton<ILocalizationService>(() => new LocalizationService(resolver.GetService<ILoggerFactory>()));
        
        services.RegisterLazySingleton<IRegistrationService>(() => 
            new RegistrationService(resolver.GetService<ILoggerFactory>(), 
                resolver.GetService<IMutableConfigurationService>(),
                resolver.GetService<IRestService>()
                ));
        
        services.RegisterLazySingleton<IAuthenticationService>(() => new AuthenticationService(
            resolver.GetService<ILoggerFactory>(),
            resolver.GetService<IRegistrationService>(),
            resolver.GetService<IRestService>(),
            resolver.GetService<IMutableConfigurationService>()
            ));

        services.RegisterLazySingleton<IClientService>(() => new ClientService(
            resolver.GetService<ILogger>(),
            resolver.GetService<IRestService>()
        ));
        
        services.RegisterLazySingleton<IRestService>(() => new RestService(
            resolver.GetService<ILoggerFactory>(),
            resolver.GetService<ServerConfiguration>()
        ));
        
    }

}