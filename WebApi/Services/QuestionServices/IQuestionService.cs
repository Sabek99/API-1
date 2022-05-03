using WebApi.Entities;
using WebApi.Models.Questions;

namespace WebApi.Services.QuestionServices;

public interface IQuestionService
{
    IQueryable GetAllQuestions();
    IQueryable GetAllQuestionsByUserId(int userId);
    IQueryable GetAllQuestionsByTagId(int tagId);
    IQueryable GetQuestionById(int questionId);
    Task<Question> CheckIfQuestionExists(int questionId);
    Task<Question> CreateQuestion(Question question);
    Question UpdateTag(Question question);
    Question DeleteTag(Question question);
    
    
}