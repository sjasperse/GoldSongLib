using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;

namespace GoldSongLib.Cli;

public static class Extensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        // services.AddSingleton<Command, ImportWorshipOrdersCommand>();

        return services;
    }

    public static Option<T> Configure<T>(this Option<T> option, Action<Option<T>> configure)
    {
        configure(option);

        return option;
    }
    public static Option<T> AddOptionFluent<T>(this Command command, Option<T> option)
    {
        command.AddOption(option);
        return option;
    }
}
