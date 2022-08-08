using ServerServices;
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
        
        return 0;
    }
}