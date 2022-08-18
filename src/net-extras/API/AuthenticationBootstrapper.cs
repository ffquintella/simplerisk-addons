using API.Security;
using Microsoft.AspNetCore.Authentication;
using Saml2.Authentication.Core.Configuration;

namespace API;

public static class AuthenticationBootstrapper
{
    public static void RegisterAuthentication(IServiceCollection services, IConfiguration config)
    {
        // Add Saml2.Authentication.Core
        services.Configure<Saml2Configuration>(config.GetSection("Saml2"));
        services.AddSaml();
        
        services.AddAuthentication(options =>
            {
                //options.DefaultScheme = "saml2";
                options.DefaultScheme = "headerSelector";
                options.DefaultChallengeScheme = "headerSelector";
            })
            .AddPolicyScheme("headerSelector", "this will select SAML or Basic Authentication", options =>
            {
                options.ForwardDefaultSelector = (context) =>
                {
                    if (context.Request.Headers.ContainsKey("Authorization"))
                    {
                        return "BasicAuthentication";
                    }
                    else if(config["Saml2:Enabled"] == "True")
                    {
                        return "saml2";
                    }
                    else
                    {
                        return "BasicAuthentication";
                    }
        
                };
                
            })
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null)
            .AddCookie("saml2.cookies", options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            })
            .AddSaml("saml2", "saml2", options =>
            {
                options.SignInScheme = "saml2.cookies";
                options.IdentityProviderName = "stubidp.sustainsys";
            });
        
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdminOnly", policy =>
            {
                policy.RequireAuthenticatedUser()
                    .Requirements.Add(new ValidUserRequirement());
                policy.Requirements.Add(new UserInRoleRequirement("Administrator"));
            });
        });
    }
}