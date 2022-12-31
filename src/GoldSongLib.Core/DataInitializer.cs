using Azure.Storage.Blobs;
using Microsoft.Extensions.Hosting;

namespace GoldSongLib.Core;

public class DataInitializer : BackgroundService
{
    private readonly IData data;

    public DataInitializer(IData data)
    {
        this.data = data;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await this.data.EnsureInitialized();
    }
}
