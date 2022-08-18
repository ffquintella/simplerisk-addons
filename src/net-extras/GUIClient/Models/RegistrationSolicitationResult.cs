namespace GUIClient.Models;

public class RegistrationSolicitationResult
{
    private RequestResult result;

    public RequestResult Result
    {
        get => result;
        set => result = value;
    }

    public string? RequestID { get; set; }
}