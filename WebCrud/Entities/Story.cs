using System.Text.Json.Serialization;

namespace WebCrud.Entities;

public class Story
{
    public int Id { get; set; }
    public required string NameStory { get; set; }
    public string? ImagePath { get; set; }
    public required string Description { get; set; }
    public string? author { get; set; }
    public string? artis { get; set; }
    public List<string> NameGenres { get; set; } = new List<string>();
    [JsonIgnore]
    public List<StoryGenre> StoryGenres { get; set; } = [];

}
