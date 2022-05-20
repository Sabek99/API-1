namespace WebApi.Entities;

public class Vote
{
    public int? UserId {set; get;}
    public User User { set; get; }
    
    public virtual int AnswerId {set; get;}
    public virtual Answer Answer {set; get;}
    public Count Count { get; set; } 

}

public enum Count
{
    Down = -1,
    Up=1
}