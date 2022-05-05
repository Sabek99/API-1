using WebApi.Entities;

namespace WebApi.Services.AnswerServices;

public interface IAnswerService
{
    Task<Answer> GetAnswerById(int answerId);
    Task<Answer> CreateAnswer(Answer answer);
    Answer UpdateAnswer(Answer answer);
    Answer DeleteAnswer(Answer answer);

}