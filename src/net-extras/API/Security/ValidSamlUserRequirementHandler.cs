using DAL;
using DAL.Context;
using Microsoft.AspNetCore.Authorization;

namespace API.Security;

public class ValidSamlUserRequirementHandler: AuthorizationHandler<ValidSamlUserRequirement>
{
    
    private SRDbContext _dbContext = null;

    public ValidSamlUserRequirementHandler(DALManager dalManager)
    {
        _dbContext = dalManager.GetContext();
    }
    
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidSamlUserRequirement requirement)
    {
        var userClaimPrincipal = context.User;

        var userName = userClaimPrincipal.Identities.FirstOrDefault().Name;
        
        
        
        throw new NotImplementedException();
    }
}