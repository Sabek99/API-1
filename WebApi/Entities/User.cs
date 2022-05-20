using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MessagePack;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Entities;


public class User : IdentityUser<int>
{
    public string Token { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public string Bio { get; set; }
    
    public int UniversityLevel { get; set; }
    
    public string PhotoUrl { get; set; }
    
    public string College { get; set; }
    
    public string Major { get; set; }
    
    public DateTime Birthdate { get; set; }
    
    public DateTime CreationTime { get; set; }
    public DateTime UpdateTime { get; set; }

    public Role Role { get; set; }
    
    //navigation properties
    public virtual ICollection<Question> Questions { get; set; }
    public virtual ICollection<Answer> Answers { get; set; }
    public virtual ICollection<Tag> Tags { get; set; }
    public virtual ICollection<Interest> Interests { get; set; }
    public virtual ICollection<Vote> Votes { get; set; }
    public ICollection<Request> Student { get; set; } = new Collection<Request>();
    public ICollection<Request> Mentor { get; set; } = new Collection<Request>();
   
    public ICollection<Review> Reviewer { get; set; } = new Collection<Review>();
    public ICollection<Review> Reviewee { get; set; } = new Collection<Review>();
}
public enum Role
{
    Student,
    Mentor
}