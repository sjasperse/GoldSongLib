using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using FluentResults;
using GoldSongLib.Core.Models;

namespace GoldSongLib.Core;

public interface ITenantDataClient
{
    Task EnsureInitialized();

    Task<IEnumerable<SongModel>> GetSongsAsync(CancellationToken cancellationToken);
    Task<Result> AddSongAsync(SongModel song, CancellationToken cancellationToken);
    Task<Result> UpdateSongAsync(SongModel song, CancellationToken cancellationToken);
    Task<Result> DeleteSongAsync(Guid songId, CancellationToken cancellationToken);

    Task<IEnumerable<WorshipOrder>> GetWorshipOrders(CancellationToken cancellationToken);
    Task<Result> AddWorshipOrderAsync(WorshipOrder song, CancellationToken cancellationToken);
    Task<Result> UpdateWorshipOrderAsync(WorshipOrder song, CancellationToken cancellationToken);
    Task<Result> DeleteWorshipOrderAsync(Guid worshipOrderId, CancellationToken cancellationToken);
}

public class TenantDataClient : ITenantDataClient
{
    private readonly BlobContainerClient tenantContainer;
    private readonly Tenant tenant;

    public TenantDataClient(BlobContainerClient tenantContainer, Tenant tenant)
    {
        this.tenantContainer = tenantContainer;
        this.tenant = tenant;
    }

    public async Task EnsureInitialized()
    {
        await this.tenantContainer.CreateIfNotExistsAsync();
    }

    public async Task<IEnumerable<SongModel>> GetSongsAsync(CancellationToken cancellationToken)
    {
        var blobsIterator = this.tenantContainer.GetBlobsAsync(prefix: "songs/", cancellationToken: cancellationToken);

        var songs = new List<SongModel>();

        await foreach (var blob in blobsIterator)
        {
            var blobClient = this.tenantContainer.GetBlobClient(blob.Name);
            var contents = (await blobClient.OpenReadAsync(cancellationToken: cancellationToken))!;
            var song = (await Json.DeserializeAsync<SongModel>(contents, cancellationToken))!;
            songs.Add(song);
        }

        return songs;
    }

    public async Task<Result> AddSongAsync(SongModel song, CancellationToken cancellationToken)
    {
        var blobClient = this.tenantContainer.GetBlobClient($"songs/{song.Id}.json");

        if (await blobClient.ExistsAsync(cancellationToken))
        {
            return Result.Fail(new ConflictError());
        }

        var contents = (await blobClient.OpenWriteAsync(true, cancellationToken: cancellationToken))!;

        await Json.SerializeAsync(contents, song, cancellationToken);

        await contents.FlushAsync();
        
        return Result.Ok();
    }

    public Task<Result> UpdateSongAsync(SongModel song, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> DeleteSongAsync(Guid songId, CancellationToken cancellationToken)
    {
        var blobClient = this.tenantContainer.GetBlobClient($"songs/{songId}.json");
        if (await blobClient.ExistsAsync(cancellationToken))
        {
            await blobClient.DeleteAsync(cancellationToken: cancellationToken);

            return Result.Ok();
        }
        else
        {
            return Result.Fail(new NotFoundError());
        }
    }

    public Task<IEnumerable<WorshipOrder>> GetWorshipOrders(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result> AddWorshipOrderAsync(WorshipOrder song, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteWorshipOrderAsync(Guid worshipOrderId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateWorshipOrderAsync(WorshipOrder song, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
