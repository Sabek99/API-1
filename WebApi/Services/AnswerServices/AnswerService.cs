using System.Collections;
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


    public async Task<IEnumerable> GetTheAnswer(int answerId)
    {
        var answer = await _context.Answers
            .Where(a => a.Id == answerId)
            .Select(answer => new
            {
                answer_id = answer.Id,
                answer_body = answer.Body,
                creation_time = answer.CreationTime,
                update_time = answer.UpdateTime,
                is_banned = answer.IsBanned,
                is_verfied = answer.IsVerified,
                question = new
                {
                    question_id = answer.Question.Id,
                    question_title = answer.Question.Title,
                    question_body = answer.Question.Body,
                    user_id = answer.Question.UserId,
                },

                owner = new
                {
                    user_id = answer.UserId,
                    user_first_name = answer.User.FirstName,
                    user_last_name = answer.User.LastName
                }

            })
            .ToListAsync();
        return answer;
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