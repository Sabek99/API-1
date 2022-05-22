using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities;

public class Request
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public User Student { get; set; }
    public int StudentId { get; set; }
    public User Mentor { get; set; }
    public int MentorId { get; set; }
    public string RequestBody { get; set; }

    public Status Status { get; set; } = Status.OnHold;
}

public enum Status
{
    OnHold,
    Accepted,
    Rejected
}