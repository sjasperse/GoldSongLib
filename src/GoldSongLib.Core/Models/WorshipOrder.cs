namespace GoldSongLib.Core.Models;

public class WorshipOrder
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public IEnumerable<WorshipOrderSongSet> SongSets { get; set; } = Enumerable.Empty<WorshipOrderSongSet>();
    public IEnumerable<string> Tags { get; set; } = Enumerable.Empty<string>();
}

public class WorshipOrderSongSet
{
    public WorshipOrderSongSet(string title)
    {
        Title = title;
    }

    public string Title { get; set; }
    public IEnumerable<WorshipOrderSong> Songs { get; set; } = Enumerable.Empty<WorshipOrderSong>();
}

public class WorshipOrderSong
{
    public Guid SongId { get; set; }
}
