using System.Reflection.Metadata.Ecma335;

namespace WebApi.Entities;

public class Question
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public bool IsBanned { get; set; }
   
    //navigation properties 
    public virtual ICollection<Answer> Answers { get; set; }
    public virtual ICollection<QuestionTag> QuestionTags { get; set; }
    public virtual User User { get; set; }
    public int UserId { get; set; }
    
}