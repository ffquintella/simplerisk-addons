namespace DAL;

using Microsoft.Extensions.Configuration;

public class DAL
{
    // requires using Microsoft.Extensions.Configuration;
    private readonly IConfiguration Configuration;
    private string ConnectionString;
    
    public DAL(IConfiguration configuration)
    {
        Configuration = configuration;
        ConnectionString = Configuration["Database:ConnectionString"];

    }
}