using System.Collections;
using WebApi.Entities;
using WebApi.Models.Pagination;
using WebApi.Models.Questions;

namespace WebApi.Services.QuestionServices;

public interface IQuestionService
{
    Task<IEnumerable> GetAllQuestions(PaginationParams @params);
    Task<IEnumerable> GetAllQuestionsByUserId(int userId,PaginationParams @params);
    Task<IEnumerable> GetAllQuestionsByTagId(int tagId,PaginationParams @params);
    Task<IEnumerable> GetQuestionById(int questionId);
    Task<Question> CheckIfQuestionExists(int questionId);
    Task<Question> CreateQuestion(Question question);
    Question UpdateQuestion(Question question);
    Question DeleteQuestion(Question question);
    Task<int> GetCount();
    Task<int> GetCountUserId(int id);
    Task<int> GetCountTagId(int id);
    


}