using System.Collections;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Pagination;
using WebApi.Models.Questions;

namespace WebApi.Services.QuestionServices;

public class QuestionService : IQuestionService
{

    private readonly DataContext _context;

    public QuestionService(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable> GetAllQuestions(PaginationParams @params)
    {
        var questions = await _context.Questions.Select(question => new {
                question_id = question.Id,
                question_title = question.Title,
                question_body = question.Body,
                creation_time = question.CreationTime,
                update_time = question.UpdateTime,
                is_banned = question.IsBanned,
                tags = question.QuestionTags.Select(t => new {t.TagId, t.Tag.Name}),
                owner = new
                {
                    user_id = question.User.Id,
                    user_first_name = question.User.FirstName,
                    user_last_name = question.User.LastName
                },
                answers = question.Answers.Select(a => new
                {
                    answer_id = a.Id,
                    answer_body = a.Body,
                    owner = new
                    {
                        user_id = a.User.Id,
                        user_first_name = a.User.FirstName,
                        user_last_name = a.User.LastName
                    },
                    is_verified = a.IsVerified,
                    is_banned = a.IsBanned
                })
            })
            .Skip((@params.Page - 1) * @params.ItemPerPage)
            .Take(@params.ItemPerPage)
            .ToListAsync();
        return questions;
    }


    public async Task<IEnumerable> GetAllQuestionsByUserId(int userId, PaginationParams @params)
    {
        var questions = await _context.Questions.Select(question => new {
                    question_id = question.Id,
                    question_title = question.Title,
                    question_body = question.Body,
                    creation_time = question.CreationTime,
                    update_time = question.UpdateTime,
                    is_banned = question.IsBanned,
                    tags = question.QuestionTags.Select(t => new {t.TagId, t.Tag.Name}),
                    owner = new
                    {
                        user_id = question.User.Id,
                        user_first_name = question.User.FirstName,
                        user_last_name = question.User.LastName
                    },
                    answers = question.Answers.Select(a => new
                    {
                        answer_id = a.Id,
                        answer_body = a.Body,
                        owner = new
                        {
                            user_id = a.User.Id,
                            user_first_name = a.User.FirstName,
                            user_last_name = a.User.LastName
                        },
                        is_verified = a.IsVerified,
                        is_banned = a.IsBanned
                    })
            })
            .Skip((@params.Page - 1) * @params.ItemPerPage)
            .Take(@params.ItemPerPage)
            .ToListAsync();
        return questions;
    }

    
    public async Task<IEnumerable> GetAllQuestionsByTagId(int tagId,PaginationParams @params)
    {
       var questions = await _context.Questions.Join(_context.QuestionTags, 
               question => question.Id, 
               qt => qt.QuestionId, 
               (question,qt)=>new {
                                               question_id = question.Id,
                                               question_title = question.Title,
                                               question_body = question.Body,
                                               creation_time = question.CreationTime,
                                               update_time = question.UpdateTime,
                                               is_banned = question.IsBanned,
                                               tags = question.QuestionTags.Select(t => new { t.TagId, t.Tag.Name }),
                                               owner = new
                                               {
                                                   user_id = question.User.Id,
                                                   user_first_name = question.User.FirstName,
                                                   user_last_name = question.User.LastName
                                               },
                                               answers = question.Answers.Select(a=> new
                                               {
                                                   answer_id = a.Id,
                                                   answer_body = a.Body,
                                                   owner = new
                                                   {
                                                       user_id = a.User.Id,
                                                       user_first_name = a.User.FirstName,
                                                       user_last_name = a.User.LastName
                                                   },
                                                   is_verified = a.IsVerified,
                                                   is_banned = a.IsBanned
                                               })
                                           })
           .Skip((@params.Page - 1) * @params.ItemPerPage)
           .Take(@params.ItemPerPage)
           .ToListAsync();
       return questions;
    }

    public async Task<IEnumerable> GetQuestionById(int questionId)
    {
        var question = await _context.Questions
            .Where(question => question.Id == questionId)
            .Select(question => new
            {
                question_id = question.Id,
                question_title = question.Title,
                question_body = question.Body,
                creation_time = question.CreationTime,
                update_time = question.UpdateTime,
                is_banned = question.IsBanned,
                tags = question.QuestionTags.Select(t => new {t.TagId, t.Tag.Name}),
                owner = new
                {
                    user_id = question.User.Id,
                    user_first_name = question.User.FirstName,
                    user_last_name = question.User.LastName
                },
                answers = question.Answers.Select(a => new
                {
                    answer_id = a.Id,
                    answer_body = a.Body,
                    owner = new
                    {
                        user_id = a.User.Id,
                        user_first_name = a.User.FirstName,
                        user_last_name = a.User.LastName
                    },
                    is_verified = a.IsVerified,
                    is_banned = a.IsBanned
                })

            })
            .ToListAsync();
        return question;
    }

    public async Task<Question> CheckIfQuestionExists(int questionId)
    {
        return await _context.Questions
            .SingleOrDefaultAsync(q => q.Id == questionId);
    }

    public async Task<Question> CreateQuestion(Question question)
    {
        await _context.AddAsync(question);
        await _context.SaveChangesAsync();
        return question;
    }

    public Question UpdateQuestion(Question question)
    {
        _context.Questions.Update(question);
        _context.SaveChanges();
        return question;
    }

    public Question DeleteQuestion(Question question)
    {
        _context.Questions.Remove(question);
        _context.SaveChanges();
        return question;
    }

    public async Task<int> GetCount()
    {
        return await _context.Questions.CountAsync();
    }

    public async Task<int> GetCountUserId(int id)
    {
        return await _context.Questions.Where(q => q.UserId == id).CountAsync();
    }

    public async Task<int> GetCountTagId(int id)
    {
        return await _context.QuestionTags.Where(qt =>qt.TagId ==id).CountAsync();
    }
}