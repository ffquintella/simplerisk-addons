// See https://aka.ms/new-console-template for more information

using System.Net.Mime;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Query.ExpressionTranslators.Internal;
using DAL;

Console.WriteLine("This is a test application!");


var configuration =  new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddUserSecrets<Program>()
    .AddJsonFile($"appsettings.json");
            
var config = configuration.Build();

var connectionString = config["Database:ConnectionString"];

Console.WriteLine("ConnectionString:" + connectionString);

var dalManager = new DALManager(config);

var dbContext = dalManager.GetContext();

Environment.Exit(-1);