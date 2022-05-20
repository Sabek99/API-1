namespace WebApi.Entities;

public class Message
{
    public int  Id { get; set; }

    public string MessageBody { get; set; }

    public DateTime CreationTime { get; set; }
    
    public DateTime? UpdateTime { get; set; }
    
    public virtual User User { get; set; }
    public int UserId { get; set; }

    public virtual Group Group { get; set; }
    public int GroupId { get; set; }
}