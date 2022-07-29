// See https://aka.ms/new-console-template for more information

using System.Net.Mime;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Query.ExpressionTranslators.Internal;

Console.WriteLine("This is a test application!");


var configuration =  new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddUserSecrets<Program>()
    .AddJsonFile($"appsettings.json");
            
var config = configuration.Build();

var connectionString = config["Database:ConnectionString"];

Console.WriteLine("ConnectionString:" + connectionString);

Environment.Exit(-1);