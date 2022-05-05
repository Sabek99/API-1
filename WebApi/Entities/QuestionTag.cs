using System.Reflection.Metadata.Ecma335;

namespace WebApi.Entities;

public class QuestionTag
{
    public int Id { get; set; }
    
    //navigation properties 
    
    public virtual int QuestionId { get; set; }
    public virtual Question Question { get; set; }

    public virtual int TagId { get; set; }
    public virtual Tag Tag { get; set; }
}