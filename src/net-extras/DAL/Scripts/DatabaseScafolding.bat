
dotnet ef dbcontext scaffold Name=Database:ConnectionString --project DAL.csproj --startup-project DAL.csproj --configuration Debug --framework net6.0  Pomelo.EntityFrameworkCore.MySql --context SRDbContext --context-dir Context --output-dir Entities --schema simplerisk --force 
