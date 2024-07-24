using Microsoft.EntityFrameworkCore;

using WebCrud.Entities;


namespace WebCrud.Data;
public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Story> Stories { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<StoryGenre> StoryGenres { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StoryGenre>()
                .HasKey(sg => new { sg.StoryId, sg.GenreId });

            modelBuilder.Entity<StoryGenre>()
                .HasOne(sg => sg.Stories)
                .WithMany(s => s.StoryGenres)
                .HasForeignKey(sg => sg.StoryId);

            modelBuilder.Entity<StoryGenre>()
                .HasOne(sg => sg.Genres)
                .WithMany(g => g.StoryGenres)
                .HasForeignKey(sg => sg.GenreId);

    }
}

