namespace Model.Authentication;

public class SAMLRequest
{
    public string RequestToken { get; set; }
    public string Status { get; set; } = "requested";
}