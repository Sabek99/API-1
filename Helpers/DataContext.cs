namespace WebApi.Helpers;

using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sql server database
        options.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"));
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany(b => b.Answers);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Tag> Tags { get; set; }   
    public DbSet<Question> Questions { get; set; }   
    public DbSet<Answer> Answers { get; set; }   
    
}