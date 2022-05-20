using System.Reflection.Metadata.Ecma335;

namespace WebApi.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    //navigation properties 
    public ICollection<QuestionTag> QuestionTags { get; set; }
    public virtual ICollection<Interest> Interests { get; set; }
   
}