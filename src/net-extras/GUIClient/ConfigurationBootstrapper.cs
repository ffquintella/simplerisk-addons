using Microsoft.Extensions.Configuration;
using Splat;
using GUIClient.Configuration;

namespace GUIClient;

public static  class ConfigurationBootstrapper
{
    
    public static void RegisterConfiguration(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        var configuration = BuildConfiguration();

        RegisterLoggingConfiguration(services, configuration);
        //RegisterDefaultThemeConfiguration(services, configuration);
        //RegisterThemesNamesConfiguration(services, configuration);
        //RegisterLanguagesConfiguration(services, configuration);

    }
    
    private static IConfiguration BuildConfiguration() =>
        new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
    
    private static void RegisterLoggingConfiguration(IMutableDependencyResolver services,
        IConfiguration configuration)
    {
        var config = new LoggingConfiguration();
        configuration.GetSection("Logging").Bind(config);
        services.RegisterConstant(config);
    }
}