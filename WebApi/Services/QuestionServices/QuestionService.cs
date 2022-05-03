using System.Collections;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Questions;

namespace WebApi.Services.QuestionServices;

public class QuestionService : IQuestionService
{

    private readonly DataContext _context;

    public QuestionService(DataContext context)
    {
        _context = context;
    }

    public IQueryable GetAllQuestions()
    {
        var query = from question in _context.Questions
            join questionTag in _context.QuestionTags on question.Id equals questionTag.QuestionId
            join user in _context.AspNetUsers on question.UserId equals user.Id
            select new
            {
                question = new 
                {
                    question_id = question.Id,
                    question_body = question.Body,
                    creation_time = question.CreationTime,
                    update_time = question.UpdateTime,
                    is_banned = question.IsBanned,
                    tags = question.QuestionTags,
                    owner = new
                    {
                        user_id = question.User.Id,
                        user_first_name = question.User.FirstName,
                        user_last_name = question.User.LastName
                    },
                    answers = question.Answers
                }
            };

        return query;
    }

    public IQueryable GetAllQuestionsByUserId(int userId)
    {
        var query = from question in _context.Questions
            join user in _context.AspNetUsers on question.UserId equals user.Id
            select new
            {
                question_id =  question.Id,
                question_body = question.Body,
                creation_time = question.CreationTime,
                update_time = question.UpdateTime,
                is_banned = question.IsBanned,
                tags = question.QuestionTags,
                owner = new
                {
                    user_id = question.User.Id,
                    user_first_name = question.User.FirstName,
                    user_last_name = question.User.LastName
                },
                answers = question.Answers
            };

        return query;
    }

    public IQueryable GetAllQuestionsByTagId(int tagId)
    {
        var query = from question in _context.Questions
            join questionTag in _context.QuestionTags on question.Id equals questionTag.QuestionId
            where questionTag.TagId == tagId
            select new
            {
               question_id = question.Id,
               question_body = question.Body,
               creation_time = question.CreationTime,
               update_time = question.UpdateTime,
               is_banned =  question.IsBanned,
               tags = questionTag.Tag,
               owner = new
                {
                    user_id = question.User.Id,
                    user_first_name = question.User.FirstName,
                    user_last_name = question.User.LastName
                },
                answers = question.Answers
            };
        return query;
    }

    public IQueryable GetQuestionById(int questionId)
    {
        var query = from question in _context.Questions
            join questionTag in _context.QuestionTags on question.Id equals questionTag.QuestionId
            where question.Id == questionId
            select new
            {
                question_id = question.Id,
                question_body = question.Body,
                creation_time = question.CreationTime,
                update_time = question.UpdateTime,
                is_banned = question.IsBanned,
                tags = questionTag.Tag,
                owner = new
                {
                    user_id = question.User.Id,
                    user_first_name = question.User.FirstName,
                    user_last_name = question.User.LastName
                },
                answers = question.Answers
            };
        return query;
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

    public Question UpdateTag(Question question)
    {
        throw new NotImplementedException();
    }

    public Question DeleteTag(Question question)
    {
        throw new NotImplementedException();
    }
}