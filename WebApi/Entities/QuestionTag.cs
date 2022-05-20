
namespace WebApi.Entities;

public class QuestionTag
{
    //navigation properties 
    public virtual int QuestionId { get; set; }
    public virtual Question Question { get; set; }

    public virtual int TagId { get; set; }
    public virtual Tag Tag { get; set; }
}