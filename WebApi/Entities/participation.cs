namespace WebApi.Entities;

public class participation
{
    public User User { get; set; }
    public int UserId { get; set; }

    public Group Group { get; set; }
    public int GroupId { get; set; }
}