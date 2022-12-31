using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoldSongLib.Core;

public static class DependencyInjectionExtesions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IData, Data>();
        services.AddHostedService<DataInitializer>();
        
        return services;
    }
}
