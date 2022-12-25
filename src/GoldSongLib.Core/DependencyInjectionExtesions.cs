using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoldSongLib.Core;

public static class DependencyInjectionExtesions
{
    public static ServiceCollection AddCore(this ServiceCollection services, IConfiguration config)
    {
        return services;
    }
}
