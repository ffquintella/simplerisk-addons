using System.Runtime.CompilerServices;
using DAL;
using Serilog;

namespace API.Tools;

public  class SelfTest: IHostedService
{
    private DALManager _dalManager;
    
    public SelfTest(DALManager dalManager)
    {
        _dalManager = dalManager;
    }
    
    public  void ExecuteAllTests()
    {
        this.ExecuteDBTests();
    }

    public  void ExecuteDBTests()
    {
        var context = _dalManager.GetContext();

        if (!context.Database.CanConnect())
        {
            Log.Error("Error in self test: database connection failed");
        }

    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        
        ExecuteAllTests();
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Log.Information("Application Stopped");
        
        return Task.CompletedTask;
    }
}