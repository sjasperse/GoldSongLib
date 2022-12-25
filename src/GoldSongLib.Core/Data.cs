using GoldSongLib.Core.Models;

namespace GoldSongLib.Core;

public interface IData
{
    Task<IEnumerable<SongModel>> GetSongsAsync(CancellationToken cancellationToken);
    Task AddSongAsync(SongModel song, CancellationToken cancellationToken);
    Task UpdateSongAsync(SongModel song, CancellationToken cancellationToken);
    Task DeleteSongAsync(Guid songId, CancellationToken cancellationToken);

    Task<IEnumerable<WorshipOrder>> GetWorshipOrders(CancellationToken cancellationToken);
    Task AddWorshipOrderAsync(WorshipOrder song, CancellationToken cancellationToken);
    Task UpdateWorshipOrderAsync(WorshipOrder song, CancellationToken cancellationToken);
    Task DeleteWorshipOrderAsync(Guid worshipOrderId, CancellationToken cancellationToken);
}

public class Data
{
    
}
