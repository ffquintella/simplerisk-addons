using Spectre.Console.Cli;

namespace ConsoleClient.Commands;

public class RegistrationCommand: Command<RegistrationSettings>
{
    public override int Execute(CommandContext context, RegistrationSettings settings)
    {
        return 0;
    }
}