using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FluentResults;
using GoldSongLib.Core.Models;

namespace GoldSongLib.Core;

public interface IData
{
    Task EnsureInitialized();

    ITenantDataClient GetTenantDataClient(Tenant tenant);

    Task<UserModel?> GetUser(string username, CancellationToken cancellationToken);

    Task<Result> AddUser(UserModel user, CancellationToken cancellationToken);
}

public class Data : IData
{
    private readonly BlobServiceClient blobServiceClient;
    private readonly BlobContainerClient globalContainerClient;

    public Data(BlobServiceClient blobServiceClient)
    {
        this.blobServiceClient = blobServiceClient;
        this.globalContainerClient = this.blobServiceClient.GetBlobContainerClient("songsetbuilder");
    }

    public async Task EnsureInitialized()
    {
        await this.globalContainerClient.CreateIfNotExistsAsync();
    }

    public ITenantDataClient GetTenantDataClient(Tenant tenant)
        => new TenantDataClient(
            this.blobServiceClient.GetBlobContainerClient($"songsetbuilder-{tenant.Id.ToLower()}"),
            tenant
        );

    public async Task<UserModel?> GetUser(string username, CancellationToken cancellationToken)
    {
        var blobClient = this.globalContainerClient.GetBlobClient($"users/{username.ToLower()}.json");

        if (!await blobClient.ExistsAsync(cancellationToken))
        {
            return null;
        }

        var contents = (await blobClient.OpenReadAsync(cancellationToken: cancellationToken))!;
        var user = (await Json.DeserializeAsync<UserModel>(contents, cancellationToken))!;

        return user;
    }

    public async Task<Result> AddUser(UserModel user, CancellationToken cancellationToken)
    {
        var blobClient = this.globalContainerClient.GetBlobClient($"users/{user.Username.ToLower()}.json");

        if (await blobClient.ExistsAsync(cancellationToken))
        {
            return Result.Fail(new ConflictError());
        }

        var contents = (await blobClient.OpenWriteAsync(
            overwrite: true, 
            options: new BlobOpenWriteOptions()
            {
                Tags = new Dictionary<string, string>()
                {
                    ["userId"] = user.Id.ToString()
                }
            },
            cancellationToken: cancellationToken
        ))!;

        await Json.SerializeAsync(contents, user, cancellationToken);

        await contents.FlushAsync();
        
        return Result.Ok();
    }
}

