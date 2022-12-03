using System.CommandLine;
using Microsoft.Extensions.Hosting;

namespace GoldSongLib.Cli;

public class Main : BackgroundService
{
    private readonly IHost host;
    private readonly IEnumerable<Command> commands;

    public Main(
        IHost host,
        IEnumerable<System.CommandLine.Command> commands
    )
    {
        this.host = host;
        this.commands = commands;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var args = System.Environment.GetCommandLineArgs().Skip(1).ToArray();

        var rootCommand = new RootCommand();
        rootCommand.AddGlobalOption(new Option<bool>("--debug"));
        rootCommand.TreatUnmatchedTokensAsErrors = true;

        foreach (var command in commands)
            rootCommand.AddCommand(command);

        var exitCode = await rootCommand.InvokeAsync(args);

        Environment.ExitCode = exitCode;

        await host.StopAsync(stoppingToken);
    }
}
