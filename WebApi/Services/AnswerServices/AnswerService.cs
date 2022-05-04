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


    public async Task<Answer>GetAnswerById(int answerId)
    {
        return await _context.Answers
            .SingleOrDefaultAsync(a =>a.Id == answerId);
    }

    public async Task<Answer> CreateAnswer(Answer answer)
    {
        await _context.Answers.AddAsync(answer);
        await _context.SaveChangesAsync();
        return answer;
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