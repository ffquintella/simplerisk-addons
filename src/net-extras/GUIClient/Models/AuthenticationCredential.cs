namespace GUIClient.Models;

public class AuthenticationCredential
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string SAMLCookie { get; set; }
    public AuthenticationType AuthenticationType { get; set; }
}