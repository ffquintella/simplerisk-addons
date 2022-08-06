namespace GUIClient.Services;

public interface IAuthenticationService
{
    bool IsAuthenticated { get; set; }
    
}