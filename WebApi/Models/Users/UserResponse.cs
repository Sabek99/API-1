namespace WebApi.Models.Users;

public class UserResponse 
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public string Bio { get; set; }
    
    public int UniversityLevel { get; set; }
    
    public string PhotoUrl { get; set; }
    
    public string College { get; set; }
    
    public string Major { get; set; }
    
    public DateTime Birthdate { get; set; }
    
    public string CreationTimestamp { get; set; }
    public string UpdateTimestamp { get; set; }
   
}