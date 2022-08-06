using Microsoft.Extensions.Logging;

namespace GUIClient.Services;

public class AuthenticationService: IAuthenticationService
{
    public bool IsAuthenticated { get; set; } = false;

    private ILogger<AuthenticationService> _logger;
    public AuthenticationService(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<AuthenticationService>();
    }
}