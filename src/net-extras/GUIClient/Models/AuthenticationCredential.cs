namespace GUIClient.Models;

public class AuthenticationCredential
{
    private string? userName;
    private string? password;
    private string? samlCookie;
    private AuthenticationType authenticationType;

    public string? UserName
    {
        get => userName;
        set => userName = value;
    }

    public string? Password
    {
        get => password;
        set => password = value;
    }

    public string? SAMLCookie
    {
        get => samlCookie;
        set => samlCookie = value;
    }

    public AuthenticationType AuthenticationType
    {
        get => authenticationType;
        set => authenticationType = value;
    }
}