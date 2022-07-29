using Microsoft.AspNetCore.Authorization;

namespace API.Security;

public class ValidSamlUserRequirement: IAuthorizationRequirement
{
    public ValidSamlUserRequirement()
    {
    }
}