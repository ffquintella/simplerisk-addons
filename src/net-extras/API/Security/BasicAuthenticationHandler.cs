using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using DAL;
using DAL.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServerServices;
using static BCrypt.Net.BCrypt;


namespace API.Security;

public class BasicAuthenticationHandler: AuthenticationHandler<AuthenticationSchemeOptions>
{
    private SRDbContext? _dbContext = null;
    private IEnvironmentService _environmentService;
    private ILogger _log;
    
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options, 
        ILoggerFactory logger, 
        UrlEncoder encoder, 
        ISystemClock clock,
        IEnvironmentService environmentService,
        DALManager dalManager) : base(options, logger, encoder, clock)
    {
        _dbContext = dalManager.GetContext();
        _environmentService = environmentService;
        _log = logger.CreateLogger(nameof(BasicAuthenticationHandler));
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var endpoint = Context.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }
        
        var authHeader = Request.Headers["Authorization"].ToString();
        
        // Basic Authentication
        if (authHeader != null && authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
        {
            var token = authHeader.Substring("Basic ".Length).Trim();
            //System.Console.WriteLine(token);
            var credentialstring = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            var credentials = credentialstring.Split(':');
            
            if (credentials[0] != "" && credentials[1] != "")
            {
                var user = _dbContext?.Users?
                    .Where(u => u.Type == "simplerisk" && u.Username == Encoding.UTF8.GetBytes(credentials[0]))
                    .FirstOrDefault();

                if (user != null)
                {
                    // Check the password
                    var valid = Verify(credentials[1], Encoding.UTF8.GetString(user.Password));
                    if (valid)
                    {
                        var claims = new[] { new Claim(ClaimTypes.Name, credentials[0]) };
                        
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
        // JWT Authentication 
        else if (authHeader != null && authHeader.StartsWith("jwt", StringComparison.OrdinalIgnoreCase))
        {
            string? username;
            var token = authHeader.Substring("Jwt ".Length).Trim();
            
            if (ValidateToken(token, out username))
            {
                // based on username to get more information from database 
                // in order to build local identity
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username!)
                    // Add more claims if needed: Roles, ...
                };

                var identity = new ClaimsIdentity(claims, "Jwt");
                var user = new ClaimsPrincipal(identity);
                _log.LogInformation("User {0} authenticated using token", username);
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(user, Scheme.Name)));
                
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
    
    private ClaimsPrincipal? GetPrincipalFromJWT(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return null;

            var symmetricKey = Convert.FromBase64String(_environmentService.ServerSecretToken);

            var validationParameters = new TokenValidationParameters()
            {
                RequireExpirationTime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
            };

            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

            return principal;
        }
        catch (Exception ex)
        {
            _log.LogError("Error extracting credentials from token message: {0}", ex.Message);
            return null;
        }
    }
    
    private bool ValidateToken(string token, out string? username)
    {
        username = null;

        var simplePrinciple = GetPrincipalFromJWT(token);
        if (simplePrinciple == null) return false;
        
        var identity = simplePrinciple.Identity as ClaimsIdentity;

        if (identity == null || !identity.IsAuthenticated)
            return false;

        var usernameClaim = identity.FindFirst(ClaimTypes.Name);
        username = usernameClaim?.Value;

        if (string.IsNullOrEmpty(username))
            return false;

        // Validate to check whether username exists in system
        var usu = username;
        
        var user = _dbContext?.Users?
            .Where(u => u.Type == "simplerisk" && u.Username == Encoding.UTF8.GetBytes(usu))
            .FirstOrDefault();

        if (user == null) return false;


        return true;
    }
}