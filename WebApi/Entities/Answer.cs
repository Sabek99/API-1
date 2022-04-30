namespace WebApi.Entities;

public class Answer
{
    public int Id { get; set; }
    public string Body { get; set; }
    public int UpVote { get; set; }
    public int DownVote { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime UpdateTime { get; set; }
    public bool IsBanned { get; set; }
    public bool IsVerified { get; set; }
    
    //navigation properties 
    public int QuestionId { get; set; }
    public Question Question { get; set; }
    public User User { get; set; }
}