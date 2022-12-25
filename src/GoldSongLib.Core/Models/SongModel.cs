using System.ComponentModel.DataAnnotations;

namespace GoldSongLib.Core.Models;

public class SongModel
{
    public SongModel(Guid id, string name, IEnumerable<string> tags)
    {
        Id = id;
        Name = name;
        Tags = tags ?? Enumerable.Empty<string>();
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<string> Tags { get; set; }
}
