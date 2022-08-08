// See https://aka.ms/new-console-template for more information

using ConsoleClient.Commands;
using DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Spectre;
using ServerServices;
using Spectre.Cli.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Spectre("{Timestamp:HH:mm:ss} [{Level:u4}] {Message:lj}{NewLine}{Exception}", LogEventLevel.Debug)
    //.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .MinimumLevel.Verbose()
    .CreateLogger();

var configuration =  new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddUserSecrets<Program>()
    .AddJsonFile($"appsettings.json");
var config = configuration.Build();


#if DEBUG
Log.Information("Starting Console Client with debug");
#endif

var services = new ServiceCollection();
// add extra services to the container here
services.AddSingleton<IConfiguration>(config);
services.AddScoped<IClientRegistrationService, ClientRegistrationService>();
services.AddSingleton<DALManager>();

var registrar = new DependencyInjectionRegistrar(services);
var app = new CommandApp(registrar);

app.Configure(config =>
{
#if DEBUG
    config.PropagateExceptions();
    config.ValidateExamples();
#endif


    config.AddCommand<SelfTestCommand>("selfTest");
    config.AddCommand<RegistrationCommand>("registration");

});

return app.Run(args);
