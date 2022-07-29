using Microsoft.AspNetCore.Authorization;

namespace API.Security;

public class ValidSamlUserRequirementHandler : AuthorizationHandler<ValidSamlUserRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidSamlUserRequirement requirement)
    {
        
        
        throw new NotImplementedException();
    }
}