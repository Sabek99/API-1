using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebApi.Helpers;

using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

public class DataContext : IdentityDbContext <IdentityUser<int>,IdentityRole<int>,int>

{
    
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
        
    }
    public DbSet<User> AspNetUsers { get; set; }
    public DbSet<Tag> Tags { get; set; }   
    public DbSet<Question> Questions { get; set; }   
    public DbSet<Answer> Answers { get; set; }
    public DbSet<QuestionTag> QuestionTags { get; set; }
   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
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
        
        //User Answer one to many relation
        modelBuilder.Entity<Answer>()
            .HasOne(u => u.User)
            .WithMany(a => a.Answers);
        
        //User Tag one to many relation 
        modelBuilder.Entity<Tag>()
            .HasOne(u => u.User)
            .WithMany(a => a.Tags);

    }

   
    
}