using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using GoldSongLib.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoldSongLib.Api.Controllers;

[Route("api/songs")]
public class SongsController : ControllerBase
{
    private readonly BlobContainerClient songsContainer;
    private readonly BlobServiceClient blobServiceClient;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public SongsController(BlobServiceClient blobServiceClient, JsonSerializerOptions jsonSerializerOptions)
    {
        this.songsContainer = blobServiceClient.GetBlobContainerClient("songs");
        this.blobServiceClient = blobServiceClient;
        this.jsonSerializerOptions = jsonSerializerOptions;
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<SongModel>>> GetSongs(CancellationToken cancellationToken)
    {
        await this.songsContainer.CreateIfNotExistsAsync();
        var blobsIterator = this.songsContainer.GetBlobsAsync(cancellationToken: cancellationToken);

        var songs = new List<SongModel>();

        await foreach (var blob in blobsIterator)
        {
            var blobClient = this.songsContainer.GetBlobClient(blob.Name);
            var contents = (await blobClient.OpenReadAsync(cancellationToken: cancellationToken))!;
            var song = (await JsonSerializer.DeserializeAsync<SongModel>(contents, jsonSerializerOptions, cancellationToken))!;
            songs.Add(song);
        }

        return this.Ok(songs);
    }

    [HttpPost()]
    public async Task<ActionResult> AddSong([FromBody]SongModel song, CancellationToken cancellationToken)
    {
        await this.songsContainer.CreateIfNotExistsAsync();

        var blobClient = this.songsContainer.GetBlobClient($"{song.Id}.json");
        var contents = (await blobClient.OpenWriteAsync(true, cancellationToken: cancellationToken))!;

        await JsonSerializer.SerializeAsync(contents, song, jsonSerializerOptions, cancellationToken);

        await contents.FlushAsync();

        return this.Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> AddSong([FromRoute]Guid id, CancellationToken cancellationToken)
    {
        await this.songsContainer.CreateIfNotExistsAsync();

        var blobClient = this.songsContainer.GetBlobClient($"{id}.json");
        if (await blobClient.ExistsAsync(cancellationToken))
        {
            await blobClient.DeleteAsync(cancellationToken: cancellationToken);
        }
        else
        {
            return this.NotFound();
        }

        return this.Ok();
    }
}
