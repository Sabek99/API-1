using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MessagePack;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Entities;


public class User:IdentityUser<int>
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
    
    public string CreationTimestamp { get; set; }
    public string UpdateTimestamp { get; set; }
    
    //navigation properties 
    public virtual ICollection<Question> Questions { get; set; }
    public virtual ICollection<Answer> Answers { get; set; }
    public virtual ICollection<Tag> Tags { get; set; }
    public virtual ICollection<Interest> Interests { get; set; }
    public virtual ICollection<Vote> Votes { get; set; }
    public virtual ICollection<Request> Requests { get; set; }
    [NotMapped]
    public virtual ICollection<Review> Reviews { get; set; }
}