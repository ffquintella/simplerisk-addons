namespace Model.Authentication;

public class AuthenticatedUserInfo
{
    public string? UserName
    {
        get;
        set;
    }
    public string? UserAccount  
    {
        get;
        set;
    }
    
    public string? UserEmail
    {
        get;
        set;
    }
    
    public List<string>? UserRoles
    {
        get;
        set;
    }
}