using System.Reflection.Metadata.Ecma335;

namespace WebApi.Entities;

public class QuestionTag
{
    public int Id { get; set; }
    
    //navigation properties 
    
    public int QuestionId { get; set; }
    public Question Question { get; set; }

    public int TagId { get; set; }
    public Tag Tag { get; set; }
}