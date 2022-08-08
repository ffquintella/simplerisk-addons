using ServerServices;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ConsoleClient.Commands;

public class RegistrationCommand: Command<RegistrationSettings>
{
    IClientRegistrationService _registrationService;
    public RegistrationCommand(IClientRegistrationService clientRegistrationService)
    {
        _registrationService = clientRegistrationService;
        
    }
    public override int Execute(CommandContext context, RegistrationSettings settings)
    {
        var registrations = _registrationService.GetAll();

        AnsiConsole.MarkupLine("[blue]**********************[/]");
        AnsiConsole.MarkupLine("[bold]Loading registrations[/]");
        AnsiConsole.MarkupLine("[blue]----------------------[/]");
        
        foreach (var registration in registrations)
        {
            AnsiConsole.WriteLine("{0}. {1} - {2} : {3} ", registration.Id, registration.RegistrationDate, registration.ExternalId, registration.Status);
        }
        
        AnsiConsole.MarkupLine("[white]======================[/]");
        
        return 0;
    }
}