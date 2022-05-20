namespace WebApi.Entities;

public class Answer
{
    public int Id { get; set; }
    public string Body { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public bool IsBanned { get; set; }
    public bool IsVerified { get; set; }
    
    //navigation properties 
    public int? QuestionId { get; set; }
    public virtual Question Question { get; set; }
    public virtual User User { get; set; }
    public int UserId { get; set; }
    public virtual ICollection<Vote> Votes { get; set; }
}