using System.Threading;
using Serilog;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ConsoleClient.Commands;

public class SelfTestCommand : Command<SelfTestSettings>
{
    public override int Execute(CommandContext context, SelfTestSettings settings)
    {

        AnsiConsole.Status()
            .Start("[underline blue]SelfTesting... [/]", ctx =>
            {
                Log.Information("SelfTest started");
                var debugStatus = "disabled";
                if (settings.Debug == true) debugStatus = "enabled";
                Log.Information("SelfTest Debug is {0}", debugStatus);

                // Simulate some work
                AnsiConsole.MarkupLine("Doing some work...");
                Thread.Sleep(1000);

                // Update the status and spinner
                ctx.Status("Thinking some more");
                ctx.Spinner(Spinner.Known.Star);
                ctx.SpinnerStyle(Style.Parse("green"));

                // Simulate some work
                AnsiConsole.MarkupLine("Doing some more work...");
                Thread.Sleep(2000);
            });


            

        return 0;
    }
}