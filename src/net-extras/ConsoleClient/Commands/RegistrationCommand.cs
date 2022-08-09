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
        switch (settings.Operation)
        {
            case "list":
                ExecuteList(context);
                return 0;
            case "approve":
                ExecuteApprove(context, settings);
                return 0;
            case "reject":
                return 0;
            case "delete":
                return 0;
            default:
                AnsiConsole.MarkupLine("[red]*** Invalid operation selected ***[/]");
                AnsiConsole.MarkupLine("[white] valid options are: list, approve, reject or delete [/]");
                return -1;
        }

        return 0;
    }

    public void ExecuteApprove(CommandContext context, RegistrationSettings settings)
    {
        
        var registrations = _registrationService.GetAll();

        AnsiConsole.MarkupLine("[blue]**********************[/]");
        AnsiConsole.MarkupLine("[bold]Loading registrations requests[/]");
        AnsiConsole.MarkupLine("[blue]----------------------[/]");
        
        foreach (var registration in registrations)
        {
            AnsiConsole.WriteLine("{0}. {1} - {2} : {3} ", registration.Id, registration.RegistrationDate, registration.ExternalId, registration.Status);
        }
        
        AnsiConsole.MarkupLine("[white]======================[/]");
        
        int id;
        if (settings.Id == null)
        {
            id = AnsiConsole.Ask<int>("Choose the id you want to [green]approve[/]?");
        }
        else
        {
            id = (int)settings.Id;
        }
    }

    public void ExecuteList(CommandContext context)
    {
        var registrations = _registrationService.GetAll();

        AnsiConsole.MarkupLine("[blue]**********************[/]");
        AnsiConsole.MarkupLine("[bold]Loading registrations requests[/]");
        AnsiConsole.MarkupLine("[blue]----------------------[/]");
        
        foreach (var registration in registrations)
        {
            AnsiConsole.WriteLine("{0}. {1} - {2} : {3} ", registration.Id, registration.RegistrationDate, registration.ExternalId, registration.Status);
        }
        
        AnsiConsole.MarkupLine("[white]======================[/]");
    }
}