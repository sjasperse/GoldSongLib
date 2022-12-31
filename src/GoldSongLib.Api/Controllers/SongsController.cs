using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using GoldSongLib.Core;
using GoldSongLib.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoldSongLib.Api.Controllers;

[Route("api/songs")]
public class SongsController : ControllerBase
{
    private readonly ITenantDataClient tenantDataClient;

    public SongsController(ITenantDataClient tenantDataClient)
    {
        this.tenantDataClient = tenantDataClient;
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<SongModel>>> GetSongs(CancellationToken cancellationToken)
    {
        var songs = await tenantDataClient.GetSongsAsync(cancellationToken);

        return this.Ok(songs);
    }

    [HttpPost()]
    public async Task<ActionResult> AddSong([FromBody]SongModel song, CancellationToken cancellationToken)
    {
        var result = await tenantDataClient.AddSongAsync(song, cancellationToken);

        if (result.HasError<ConflictError>()) return this.Conflict(result.Errors);

        return this.Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSong([FromRoute]Guid id, CancellationToken cancellationToken)
    {
        var result = await tenantDataClient.DeleteSongAsync(id, cancellationToken);

        if (result.HasError<NotFoundError>()) return this.NotFound(result.Errors);
        if (result.HasError<ConflictError>()) return this.Conflict(result.Errors);

        return this.Ok();
    }
}
