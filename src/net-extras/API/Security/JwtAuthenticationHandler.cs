using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using DAL;
using DAL.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ServerServices;
using ILogger = Serilog.ILogger;


namespace API.Security;

public class JwtAuthenticationHandler: AuthenticationHandler<JwtBearerOptions>
{
    private SRDbContext? _dbContext = null;
    private IEnvironmentService _environmentService;
    private ILogger _log;
    private JwtBearerOptions _options;
    
    public JwtAuthenticationHandler(
        IOptionsMonitor<JwtBearerOptions> options, 
        ILoggerFactory logger, 
        UrlEncoder encoder, 
        ISystemClock clock,
        IEnvironmentService environmentService,
        DALManager dalManager) : base(options, logger, encoder, clock)
    {
        _options = options.CurrentValue;
        _dbContext = dalManager.GetContext();
        _environmentService = environmentService;
        _log = Log.Logger;
    }
    
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var endpoint = Context.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }
        
        var authHeader = Request.Headers["Authorization"].ToString();
        
        // JWT Authentication 
        if (authHeader != null && authHeader.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
        {
            if (_options.RequireHttpsMetadata)
            {
                if (!Request.IsHttps)
                {
                    Response.StatusCode = 401;
                    Response.Headers.Add("WWW-Authenticate", "Basic realm=\"sr-netextras.net\"");
                    return Task.FromResult(AuthenticateResult.Fail("Https is required"));                    
                }
            }
            
            string? username;
            var token = authHeader.Substring("Bearer ".Length).Trim();
            
            if (ValidateToken(token, out username))
            {
                // based on username to get more information from database 
                // in order to build local identity
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username!)
                    // Add more claims if needed: Roles, ...
                };

                var identity = new ClaimsIdentity(claims, "Bearer");
                var user = new ClaimsPrincipal(identity);
                _log.Information("User {0} authenticated using token", username);
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(user, Scheme.Name)));
                
            }
            
            Response.StatusCode = 401;
            Response.Headers.Add("WWW-Authenticate", "Basic realm=\"sr-netextras.net\"");
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

            /*var validationParameters = new TokenValidationParameters()
            {
                RequireExpirationTime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
            };*/

            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, _options.TokenValidationParameters, out securityToken);

            return principal;
        }
        catch (Exception ex)
        {
            _log.Error("Error extracting credentials from token message: {0}", ex.Message);
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
            .Where(u => u.Type == "simplerisk" && u.Enabled == true && u.Lockout == 0 && u.Username == Encoding.UTF8.GetBytes(usu))
            .FirstOrDefault();

        if (user == null) return false;


        return true;
    }
}