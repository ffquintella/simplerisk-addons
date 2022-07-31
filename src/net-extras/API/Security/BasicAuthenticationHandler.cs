using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using DAL;
using DAL.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using static BCrypt.Net.BCrypt;


namespace API.Security;

public class BasicAuthenticationHandler: AuthenticationHandler<AuthenticationSchemeOptions>
{
    private SRDbContext _dbContext = null;
    
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options, 
        ILoggerFactory logger, 
        UrlEncoder encoder, 
        ISystemClock clock,
        DALManager dalManager) : base(options, logger, encoder, clock)
    {
        _dbContext = dalManager.GetContext();
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authHeader = Request.Headers["Authorization"].ToString();
        if (authHeader != null && authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
        {
            var token = authHeader.Substring("Basic ".Length).Trim();
            //System.Console.WriteLine(token);
            var credentialstring = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            var credentials = credentialstring.Split(':');
            
            if (credentials[0] != "" && credentials[1] != "")
            {
                var user = _dbContext.Users
                    .Where(u => u.Type == "simplerisk" && u.Username == Encoding.UTF8.GetBytes(credentials[0]))
                    .FirstOrDefault();

                if (user != null)
                {
                    // Check the password

                    //var hashedPassword = HashPassword(credentials[1], workFactor: 15);

                    var valid = Verify(credentials[1], Encoding.UTF8.GetString(user.Password));
                    
                    //BCrypt.HashPassword("RwiKnN>9xg3*C)1AZl.)y8f_:GCz,vt3T]PI", workFactor: cost);

                    if (valid)
                    {
                        var claims = new[] { new Claim("name", credentials[0]) };
                        
                        if (user.Admin)
                        {
                            claims = claims.Concat(new[] {new Claim(ClaimTypes.Role, "Admin")}).ToArray();
                        }
                        

                        var identity = new ClaimsIdentity(claims, "Basic");
                        var claimsPrincipal = new ClaimsPrincipal(identity);
                        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
                    }
                }
            }

            Response.StatusCode = 401;
            Response.Headers.Add("WWW-Authenticate", "Basic realm=\"simplerisk-netextras.net\"");
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
        }
        else
        {
            Response.StatusCode = 401;
            Response.Headers.Add("WWW-Authenticate", "Basic realm=\"simplerisk-netextras.net\"");
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
        }
    }
}