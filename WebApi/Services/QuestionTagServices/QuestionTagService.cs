using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.QuestionTag;

namespace WebApi.Services.QuestionTagServices;

public class QuestionTagService : IQuestionTagService
{
    private readonly DataContext _context;

    public QuestionTagService(DataContext context)
    {
        _context = context;
    }


    public async Task<QuestionTag> GetQuestionTagByTagId(int tagId)
    {
        return await _context.QuestionTags
            .SingleOrDefaultAsync(qt => qt.TagId == tagId);
    }

    public async Task<QuestionTag> CreateQuestionTag(QuestionTag questionTag)
    {
        await _context.QuestionTags.AddAsync(questionTag);
        await _context.SaveChangesAsync();
        return questionTag;
    }

    public async Task<QuestionTag> CheckIfQuestionTagExists(QuestionTagModel questionTag)
    {
        return await _context.QuestionTags
            .SingleOrDefaultAsync(qt => (qt.QuestionId == questionTag.QuestionId) && (qt.TagId == questionTag.TagId));
    }

    public QuestionTag UpdateQuestionTag(QuestionTag questionTag)
    {
        _context.Update(questionTag);
        _context.SaveChanges();
        return questionTag;
    }
}