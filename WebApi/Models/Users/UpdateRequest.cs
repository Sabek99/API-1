namespace WebApi.Models.Users;

public class UpdateRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    
    public string Email { get; set; }

    public string PhoneNumber { get; set; }
    public string Bio { get; set; }
    
    public int UniversityLevel { get; set; }
    
    public string PhotoUrl { get; set; }
    
    public string College { get; set; }
    
    public string Major { get; set; }
    
}