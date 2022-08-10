using DAL.Entities;
using Serilog;
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
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (settings == null) throw new ArgumentNullException(nameof(settings));
        
        switch (settings.Operation)
        {
            case "list":
                ExecuteList(context, settings);
                return 0;
            case "approve":
                ExecuteApprove(context, settings);
                return 0;
            case "reject":
                ExecuteReject(context, settings);
                return 0;
            case "delete":
                return 0;
            default:
                AnsiConsole.MarkupLine("[red]*** Invalid operation selected ***[/]");
                AnsiConsole.MarkupLine("[white] valid options are: list, approve, reject or delete [/]");
                return -1;
        }
    }
    public void ExecuteApprove(CommandContext context, RegistrationSettings settings)
    {
        
        var registrations = _registrationService.GetRequested();

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


        var regReq = _registrationService.GetRegistrationById(id);
        if (regReq == null)
        {
            AnsiConsole.MarkupLine("[Red]*** Invalid ID ***[/]");
            return;
        }

        regReq.Status = "approved";
        
        _registrationService.Save(regReq);

        var loggedUser = Environment.UserName;
        Log.Information("Registration: {0} approved by {1}", id, loggedUser);

    }
    
    public void ExecuteReject(CommandContext context, RegistrationSettings settings)
    {
        
        var registrations = _registrationService.GetRequested();

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
            id = AnsiConsole.Ask<int>("Choose the id you want to [red]reject[/]?");
        }
        else
        {
            id = (int)settings.Id;
        }


        var regReq = _registrationService.GetRegistrationById(id);
        if (regReq == null)
        {
            AnsiConsole.MarkupLine("[Red]*** Invalid ID ***[/]");
            return;
        }

        regReq.Status = "rejected";
        
        _registrationService.Save(regReq);

        var loggedUser = Environment.UserName;
        Log.Information("Registration: {0} rejected by {1}", id, loggedUser);

    }

    public void ExecuteList(CommandContext context, RegistrationSettings settings)
    {
        List<AddonsClientRegistration> registrations;
        if(settings.All != null && settings.All == true) registrations = _registrationService.GetAll();
        else registrations = _registrationService.GetRequested();

        AnsiConsole.MarkupLine("[blue]**********************[/]");
        AnsiConsole.MarkupLine("[bold]Loading registrations requests[/]");
        AnsiConsole.MarkupLine("[blue]----------------------[/]");
        
        foreach (var registration in registrations)
        {
            string color;
            switch (registration.Status)
            {   
                case "requested":
                    color = "blue";
                    break;
                case "accepted":
                    color = "green";
                    break;
                case "rejected":
                    color = "red";
                    break;
                default:
                    color = "white";
                    break;
            }
            AnsiConsole.MarkupLine("{0}. {1} - {2} : [{4}]{3}[/] ", registration.Id, registration.RegistrationDate, registration.ExternalId, registration.Status, color);
        }
        
        AnsiConsole.MarkupLine("[white]======================[/]");
    }
}