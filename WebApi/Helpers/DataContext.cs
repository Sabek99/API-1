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
    public DbSet<Interest> Interests { get; set; }
    public DbSet<Vote> Votes { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<participation> Participations { get; set; }
    public DbSet<Review> Reviews { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        //Question & answer one to many relation 
        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany(b => b.Answers);
        
        //Question Tag many to many relation
        modelBuilder.Entity<QuestionTag>()
            .HasKey(qt => new {qt.QuestionId, qt.TagId});
        
        //User Question one to many relation
        modelBuilder.Entity<Question>()
            .HasOne(u => u.User)
            .WithMany(q => q.Questions);
        
        //User Answer one to many relation
        modelBuilder.Entity<Answer>()
            .HasOne(u => u.User)
            .WithMany(a => a.Answers);
        
        //Has Interest relation
        modelBuilder.Entity<Interest>().HasKey(i => new {i.UserId, i.TagId});
        
        // Vote relation 
        modelBuilder.Entity<Vote>().HasKey(v => new {v.UserId, v.AnswerId});
        
        // Request relation 
        modelBuilder.Entity<Request>()
            .HasKey(ru => new { ru.StudentId, ru.MentorId});
        
        modelBuilder.Entity<Request>()
            .HasOne(r => r.Student)
            .WithMany(u => u.Mentor)
            .HasForeignKey(r => r.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Request>()
            .HasOne(r => r.Mentor)
            .WithMany(u => u.Student)
            .HasForeignKey(r => r.MentorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // User Group relation
        modelBuilder.Entity<participation>()
            .HasKey(p => new {p.UserId, p.GroupId});
        
        // Review relation 
        modelBuilder.Entity<Review>()
            .HasKey(ru => new { ru.RevieweeId, ru.ReviewerId});
        
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Reviewee)
            .WithMany(u => u.Reviewer)
            .HasForeignKey(r => r.RevieweeId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Reviewer)
            .WithMany(u => u.Reviewee)
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
    
    
}