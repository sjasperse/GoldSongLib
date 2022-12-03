using System.Diagnostics;
using GoldSongLib.Cli;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

if (args.Contains("--debug"))
{
    Console.WriteLine($"Waiting for debugger to attached process {Environment.ProcessId}...");
    while (!Debugger.IsAttached)
    {
        await Task.Delay(500);
    }

    Console.WriteLine("Debugged Attached");
}

var cancellationTokenSource = new CancellationTokenSource();
var cancellationToken = cancellationTokenSource.Token;

await Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddCommands();
        services.AddHostedService<Main>();
    })
    .RunConsoleAsync(c => c.SuppressStatusMessages = true, cancellationToken);
