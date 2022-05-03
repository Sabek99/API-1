using WebApi.Entities;

namespace WebApi.Models.Questions;

public class BaseQuestionModel
{
    public string QuestionTitle { get; set; }
    public string QuestionBody { get; set; }
    public IEnumerable<int> TagsId { get; set; }
}