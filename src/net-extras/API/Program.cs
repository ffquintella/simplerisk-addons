using System.Net;
using API;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Serilog;

#if DEBUG
var configuration =  new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddUserSecrets<Program>()
    .AddJsonFile($"appsettings.json");
#else 
var configuration =  new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.json");
#endif

var config = configuration.Build();

var builder = WebApplication.CreateBuilder(args);

int httpsPort = Int32.Parse(config["https:port"]);
string certificateFile = config["https:certificate:file"];
string certificatePassword = config["https:certificate:password"];

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Listen(IPAddress.Any, 5443, listenOptions =>
    {
        listenOptions.UseHttps(certificateFile, certificatePassword);
    } );
});

Bootstrapper.Register(builder.Services, config);

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

app.Lifetime.ApplicationStarted.Register(() =>
    {
        Log.Information("Application started");
    }
);

app.Run();