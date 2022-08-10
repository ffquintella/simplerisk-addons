using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ConsoleClient.Commands;

public class RegistrationSettings: CommandSettings
{
    [Description("One of the operations to execute. Valid values are: list, approve, reject, delete.")]
    [CommandArgument(0, "<operation>")]
    public string Operation { get; set; } = "";
    
    [CommandOption("-i|--id")]
    public int? Id { get; set; }
    
    [CommandOption("--all")]
    public bool? All { get; set; } 
    
    /*public override ValidationResult Validate()
    {
        switch (Operation)
        {
            case "list":
                return ValidationResult.Success();
            case "approve":
                return ValidationResult.Success();
            case "reject":
                return ValidationResult.Success();
            case "delete":
                return ValidationResult.Success();
            default:
                return  ValidationResult.Error("Invalid operation.");
        }
 
    }*/
}