using WebCrud.Entities;

namespace WebCrud.DTOs;


public record struct AddStoriesDto(string NameStory, string ImagePath,
    string Description, string Author, string Artis, List<string> Genres);
public class EditStoriesDto
{
    public string NameStory { get; set; } = "";
    public string ImagePath { get; set; } = "";
    public string Description { get; set; } = "";
    public string Author { get; set; } = "";
    public string Artis { get; set; } = "";
}
public class AddGenreInStory
{
    public required List<string> Genres { get; set;}
}