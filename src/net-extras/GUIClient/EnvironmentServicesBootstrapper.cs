using ClientServices.Services;
using Splat;

namespace ClientServices;

public class EnvironmentServicesBootstrapper
{
    public static void RegisterEnvironmentServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        RegisterCommonServices(services);
        RegisterPlatformSpecificServices(services, resolver);
    }

    private static void RegisterCommonServices(IMutableDependencyResolver services)
    {
        services.RegisterLazySingleton<IEnvironmentService>(() => new EnvironmentService());
        services.Register<IPlatformService>(() => new PlatformService());
    }

    private static void RegisterPlatformSpecificServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {

    }

    private static void RegisterWindowsServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {

    }
}