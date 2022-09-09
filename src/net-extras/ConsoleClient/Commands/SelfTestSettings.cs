using Spectre.Console.Cli;

namespace ConsoleClient.Commands;

public class SelfTestSettings: CommandSettings
{
    [CommandOption("--debug")]
    public bool? Debug { get; set; }
}