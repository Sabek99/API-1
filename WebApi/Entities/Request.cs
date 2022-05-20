using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities;

public class Request
{
    public int Id { get; set; }
    public virtual User User { get; set; }
    public int UserId { get; set; }
    public virtual User Mentor { get; set; }
    public int MentorId { get; set; }
    public string RequestBody { get; set; }
}