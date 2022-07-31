using API.Security;
using DAL;
using DAL.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Saml2.Authentication.Core.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

var configuration =  new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddUserSecrets<Program>()
    .AddJsonFile($"appsettings.json");
var config = configuration.Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSingleton<IAuthorizationHandler, ValidSamlUserRequirementHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, UserInRoleRequirementHandler>();

builder.Services.AddSingleton<DALManager>(sp => new DALManager(config));

// Add Saml2.Authentication.Core
builder.Services.Configure<Saml2Configuration>(builder.Configuration.GetSection("Saml2"));

builder.Services.AddSaml();

builder.Services.AddAuthentication(options =>
    {
        //options.DefaultScheme = "saml2";
        options.DefaultScheme = "BasicAuthentication";
        options.DefaultChallengeScheme = "saml2";
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


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminOnly", policy =>
    {
        policy.RequireAuthenticatedUser()
            .Requirements.Add(new ValidSamlUserRequirement());
        policy.Requirements.Add(new UserInRoleRequirement("Administrator"));
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();