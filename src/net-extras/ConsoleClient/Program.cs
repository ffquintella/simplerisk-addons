// See https://aka.ms/new-console-template for more information

using ConsoleClient.Commands;
using Serilog;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Spectre;
using Spectre.Console;
using Spectre.Console.Cli;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Spectre("{Timestamp:HH:mm:ss} [{Level:u4}] {Message:lj}{NewLine}{Exception}", LogEventLevel.Debug)
    //.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .MinimumLevel.Verbose()
    .CreateLogger();
    
#if DEBUG
var debug = true;
Log.Information("Starting Console Client with debug");
#else
var debug = false;
#endif

var app = new CommandApp();

app.Configure(config =>
{
#if DEBUG
    config.PropagateExceptions();
    config.ValidateExamples();
#endif


    config.AddCommand<SelfTestCommand>("selfTest");

});

return app.Run(args);
