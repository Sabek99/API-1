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
    public ICollection<Answer> Answers { get; set; }
    public ICollection<QuestionTag> QuestionTags { get; set; }
    public User User { get; set; }
    
}