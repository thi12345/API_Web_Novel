using System.Text.Json.Serialization;

namespace WebCrud.Entities;

public class Genre
{
    public int Id { get; set; }
    public required string NameGenre { get; set; }
    [JsonIgnore]
    public List<StoryGenre> StoryGenres { get; } = [];
}
