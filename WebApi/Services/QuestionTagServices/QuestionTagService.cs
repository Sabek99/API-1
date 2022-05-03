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


    public async Task<EntityEntry<QuestionTag>> CreateQuestionTag(QuestionTag questionTag)
    {
        return await _context.QuestionTags.AddAsync(questionTag);
    }
}