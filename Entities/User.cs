using Microsoft.AspNetCore.Identity;

namespace WebApi.Entities;


public class User:IdentityUser
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    //navigation properties 
    public ICollection<Question> Questions { get; set; }
}