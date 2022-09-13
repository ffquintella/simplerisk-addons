using API;
using Serilog;

#if DEBUG
var configuration =  new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddUserSecrets<Program>()
    .AddJsonFile($"appsettings.json");
#else 
var configuration =  new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddUserSecrets<Program>()
    .AddJsonFile($"appsettings.json");
#endif

var config = configuration.Build();

var builder = WebApplication.CreateBuilder(args);

Bootstrapper.Register(builder.Services, config);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(() =>
    {
        Log.Information("Application started");
    }
);

app.Run();