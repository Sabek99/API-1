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
        //Question & answer one to many relation 
        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany(b => b.Answers);
        
        
        //Question and Tag many to many relation 
        modelBuilder.Entity<QuestionTag>()
            .HasOne(q => q.Tag)
            .WithMany(qt => qt.QuestionTags)
            .HasForeignKey(q => q.QuestionId);
        
        modelBuilder.Entity<QuestionTag>()
            .HasOne(t => t.Tag)
            .WithMany(qt => qt.QuestionTags)
            .HasForeignKey(q => q.TagId);
        
        //User Question one to many relation
        modelBuilder.Entity<Question>()
            .HasOne(u => u.User)
            .WithMany(q => q.Questions);

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Tag> Tags { get; set; }   
    public DbSet<Question> Questions { get; set; }   
    public DbSet<Answer> Answers { get; set; }
    public DbSet<QuestionTag> QuestionTags { get; set; }
    
}