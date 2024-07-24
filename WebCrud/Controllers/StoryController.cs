
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCrud.Data;
using WebCrud.DTOs;
using WebCrud.Entities;

namespace WebCrud.Controllers;
[ApiController]
[Route("[controller]")]
public class StoryController(DataContext context) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Story>>> GetStories()
    {
        var stories = await context.Stories.ToListAsync();

        return stories;

    }

    [HttpPost("AddStory")]
    public async Task<ActionResult<Story>> AddStory(AddStoriesDto addStoriesDto)
    {
        var story = new Story
        {
            NameStory = addStoriesDto.NameStory,
            ImagePath = addStoriesDto.ImagePath,
            Description = addStoriesDto.Description,
            author = addStoriesDto.Author,
            artis = addStoriesDto.Artis
        };
        context.Stories.Add(story);
        await context.SaveChangesAsync();
        var existingGenres = await context.Genres
             .Where(g => addStoriesDto.Genres.Contains(g.NameGenre))
             .ToListAsync();
        var StoriesGenres = existingGenres.Select(g => new StoryGenre
        {
            GenreId = g.Id,
            StoryId = story.Id
        });
        //var newGenres = addStoriesDto.Genres.Except(existingGenres.Select(g => g.NameGenre))
        //    .Select(name => new Genre { NameGenre = name }).ToList();
        //foreach (var genre in newGenres)
        //{
        //    var storygenre = new StoryGenre
        //    {
        //        Genres = genre,
        //        StoryId = story.Id
        //    };

        //    context.Genres.Add(genre);
        //}
        story.NameGenres = addStoriesDto.Genres.ToList();
        context.StoryGenres.AddRange(StoriesGenres);
        await context.SaveChangesAsync();
        return story;
    }
    [HttpGet("GetStoryByGenre")]
    public async Task<ActionResult<IEnumerable<Story>>> GetStoriesByGenre (string genrename)
    {
        var stories = await context.Stories.Where(s =>
        s.StoryGenres.Any(sg => sg.Genres.NameGenre == genrename )).ToListAsync();
        if (stories is null || stories.Count == 0) return NotFound();
        return stories;
    }
    [HttpPut("EditInfoStory")]
    public async Task<ActionResult<Story>> EditStoryInfo (EditStoriesDto editStoriesDto, int StoryId)
    {
        var stories = await context.Stories.FindAsync(StoryId);
        if (stories is null) return NotFound();
        if (editStoriesDto.NameStory != "") stories.NameStory = editStoriesDto.NameStory;
        if (editStoriesDto.ImagePath != "") stories.ImagePath = editStoriesDto.ImagePath;
        if (editStoriesDto.Description != "") stories.Description = editStoriesDto.Description;
        if (editStoriesDto.Artis != "") stories.artis = editStoriesDto.Artis;
        if (editStoriesDto.Author != "") stories.author = editStoriesDto.Author;
        context.Stories.Update(stories);
        await context.SaveChangesAsync();
        return stories;
    }
    [HttpPut("UpdateGenreStory")]
    public async Task<ActionResult<Story>> UpdateGenre(AddGenreInStory addGenreInStory, int StoryId)
    {
        var stories = await context.Stories.FindAsync(StoryId);
        if (stories is null) return NotFound();
        var existingGenres = await context.Genres
        .Where(g => addGenreInStory.Genres.Contains(g.NameGenre))
        .ToListAsync();

        var newGenres = addGenreInStory.Genres
        .Except(existingGenres.Select(g => g.NameGenre))
        .Select(name => new Genre { NameGenre = name })
        .ToList();
        var updateStoryGenre = existingGenres.Concat(newGenres).ToList();
        context.Stories.Update(stories);
        await context.SaveChangesAsync();
        return stories;
    }
    [HttpDelete("DeleteStory")]
    public async Task<ActionResult<Story>> DeleteStory(int StoryId)
    {
        var stories = await context.Stories.FindAsync(StoryId);
        if (stories is null) return NotFound();
        context.Stories.Remove(stories);
        await context.SaveChangesAsync();
        return Ok("Sucess");
    }
}
