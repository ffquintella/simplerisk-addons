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
            resolver.GetService<IRegistrationService>(),
            resolver.GetService<IRestService>(),
            resolver.GetService<IMutableConfigurationService>(),
            resolver.GetService<IEnvironmentService>()
            ));

        services.RegisterLazySingleton<IClientService>(() => new ClientService(
            resolver.GetService<IRestService>()
        ));
        
        services.RegisterLazySingleton<IStatisticsService>(() => new StatisticsService(
            resolver.GetService<IRestService>()
        ));
        
        services.RegisterLazySingleton<IAssessmentsService>(() => new AssessmentsService(resolver.GetService<IRestService>()));

        services.RegisterLazySingleton<IRestService>(() => new RestService(
            resolver.GetService<ILoggerFactory>(),
            resolver.GetService<ServerConfiguration>(),
            resolver.GetService<IEnvironmentService>()
        ));
        
    }

}