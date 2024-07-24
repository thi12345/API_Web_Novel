using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCrud.Data;
using WebCrud.Entities;

namespace WebCrud.Controllers;
[ApiController]
[Route("[controller]")]
public class GenreController(DataContext context) : BaseApiController
    {
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Genre>>> GetGenre()
    {
        var genre = await context.Genres.ToListAsync();
        return genre;
    }
    [HttpGet("id")]
    public async Task<ActionResult<Genre>> GetGenreById(int id)
    {
        var genre = await context.Genres.FirstOrDefaultAsync(m => m.Id == id);
        if (genre is null) return NotFound();
        return genre;
    }
    
    [HttpPost("AddGenre")]
    public async Task<ActionResult<Genre>> AddGenre(string addGenre)
    {
        if (await GenreExists(addGenre)) return BadRequest("Genre is rexist");
        var genre = new Genre
        {
            NameGenre = addGenre.ToLower()
        };

        context.Genres.Add(genre);
        await context.SaveChangesAsync();
        return genre;
    }
    [HttpPost("AddGenreList")]
    public async Task<ActionResult<IEnumerable<Genre>>> AddGenreList(List<string> addGenreList)
    {
        var existingGenres = await context.Genres
            .Where(g => addGenreList.Contains(g.NameGenre.ToLower()))
            .Select(g => g.NameGenre.ToLower())
            .ToListAsync();

        var newGenres = addGenreList
            .Select(name => name.ToLower())
            .Except(existingGenres)
            .Select(name => new Genre { NameGenre = name })
            .ToList();
        if (newGenres.Count == 0) return BadRequest("All genres already exist");

        context.Genres.AddRange(newGenres);
        await context.SaveChangesAsync();
        return newGenres;
    }
    //[HttpDelete("DeleteGenre")]
    //public async Task<ActionResult<Genre>> DeleteGenre(string GenreDelete)
    //{
    //    var genre = await context.Genres.FindAsync(GenreDelete);
    //    if (genre is null) return NotFound(genre);
    //    context.Genres.Remove(genre);
    //    await context.SaveChangesAsync();
    //    return genre;
    //}
    private async Task<bool> GenreExists(string addGenre)
    {
        return await context.Genres.AnyAsync(x
            => x.NameGenre.ToLower() == addGenre.ToLower());
    }
}

