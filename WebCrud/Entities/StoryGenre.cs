namespace WebCrud.Entities;

public class StoryGenre
{
    public int StoryId { get; set; }
    public int GenreId { get; set; }
    public Story Stories { get; set; } = null!;
    public Genre Genres { get; set; } = null!;
}
