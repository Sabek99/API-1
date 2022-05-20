namespace WebApi.Entities;

public class Group
{
    public int Id { get; set; }
    
    public string GroupName { get; set; }
    
    public DateTime CreationTime { get; set; }
    
    public DateTime? UpdateTime { get; set; }

    public string JitsiRoomId { get; set; }

    public ICollection<Message> Messages { get; set; }
    public ICollection<participation> Participated { get; set; }
}