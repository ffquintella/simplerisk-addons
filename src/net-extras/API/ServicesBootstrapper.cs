using API.ClassMapping;
using API.Security;
using API.Tools;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ServerServices;

namespace API;

public class ServicesBootstrapper
{
    public static void RegisterServices(IServiceCollection services, IConfiguration config)
    {
        AddGeneralServices(services);
        RegisterDIClasses(services, config);
    }

    private static void AddGeneralServices(IServiceCollection services)
    {
        // Add services to the container.
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddAutoMapper(typeof(ClientProfile));
        services.AddMemoryCache();
        services.AddMemoryCache(options =>
        {
            // Overall 1024 size (no unit)
            options.SizeLimit = 1024;
        });
    }

    private static void RegisterDIClasses(IServiceCollection services, IConfiguration config)
    {
        
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddHostedService<SelfTest>();
        
        services.AddSingleton<IClientRegistrationService, ClientRegistrationService>();
        services.AddSingleton<IAuthorizationHandler, ValidUserRequirementHandler>();
        services.AddSingleton<IAuthorizationHandler, UserInRoleRequirementHandler>();
        services.AddSingleton<IEnvironmentService, EnvironmentService>();
        services.AddSingleton<IAssessmentsService, AssessmentsService>();
        services.AddSingleton<DALManager>(sp => new DALManager(config));
        
        services.AddTransient<IRiskManagementService, RiskManagementService>();
        services.AddTransient<IUserManagementService, UserManagementService>();
        services.AddTransient<IRoleManagementService, RoleManagementService>();
        services.AddTransient<IPermissionManagementService, PermissionManagementService>();
    }
}