namespace WebApi.Entities;

public class Vote
{
    public virtual int UserId {set; get;}
    public  virtual  User User { set; get; }
    
    public virtual int AnswerId {set; get;}
    public virtual Answer Answer {set; get;}
    public VoteType VoteType { get; set; } 

}

public enum VoteType
{
    Down = -1,
    Up=1
}