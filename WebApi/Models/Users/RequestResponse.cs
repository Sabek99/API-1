using WebApi.Entities;

namespace WebApi.Models.Users;

public class RequestResponse
{
    public int Id { get; set; }
    public string request_body { get; set; }
    public int Status { get; set; }
    public int MentorId { get; set; }
    
}