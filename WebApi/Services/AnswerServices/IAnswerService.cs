using WebApi.Entities;

namespace WebApi.Services.AnswerServices;

public interface IAnswerService
{
    IQueryable GetAnswerById(int answerId);
    Task<Answer> CreateAnswer(Answer answer);
    Task<Answer> CheckIfAnswerExists(int answerId);
    Answer UpdateAnswer(Answer answer);
    Answer DeleteAnswer(Answer answer);

}