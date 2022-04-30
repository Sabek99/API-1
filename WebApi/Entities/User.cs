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
    
    //navigation properties 
    public ICollection<Question> Questions { get; set; }
    public ICollection<Answer> Answers { get; set; }
    public ICollection<Tag> Tags { get; set; }
    
}