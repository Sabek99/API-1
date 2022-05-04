using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services.AnswerServices;

public class AnswerService : IAnswerService
{

    private readonly DataContext _context;

    public AnswerService(DataContext context)
    {
        _context = context;
    }


    public IQueryable GetAnswerById(int answerId)
    {
        var query = from answer in _context.Answers
            select new
            {
                Answer_body = answer.Body,
                //question = answer.Question,
                answered_by = answer.User.FirstName + ", " + answer.User.LastName,
                answer.CreationTime,
                answer.UpdateTime
            };
        return query;
    }

    public async Task<Answer> CreateAnswer(Answer answer)
    {
        await _context.Answers.AddAsync(answer);
        await _context.SaveChangesAsync();
        return answer;
    }

    public async Task<Answer> CheckIfAnswerExists(int answerId)
    {
        return await _context.Answers
            .SingleOrDefaultAsync(a => a.Id == answerId);
    }

    public Answer UpdateAnswer(Answer answer)
    {
         _context.Answers.Update(answer);
         _context.SaveChanges();
         return answer;
    }

    public Answer DeleteAnswer(Answer answer)
    {
        _context.Answers.Remove(answer);
        _context.SaveChanges();
        return answer;
    }
}