using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Tags;

namespace WebApi.Services.TageServices;

public class TagService : ITagService
{
    private readonly DataContext _context;

    public TagService(DataContext context)
    {
        _context = context;
    }
    public async Task<Tag> CheckIfTagExists(int id) 
    {
        return await _context.Tags
            .SingleOrDefaultAsync(t => t.Id == id); 
    }

    public IQueryable GetAllTags()
    {
        var query = from tag in _context.Tags
            select new
            {
                tag_id = tag.Id,
                tag_name = tag.Name,
                tag_description = tag.Description,
                number_of_questions = tag.QuestionTags.Count
            };
        return query;
    }

    public IQueryable GetTagById(int id)
    {
        var query = from tag in _context.Tags
                .Where(t => t.Id == id)
            select new
            {
                tag_id = tag.Id,
                tag_name = tag.Name,
                tag_description = tag.Description,
                number_of_questions = tag.QuestionTags.Count
            };
        return query;
    }
    
    public async Task<Tag> CreateTag(Tag tag)
    {
        await _context.AddAsync(tag);
        await _context.SaveChangesAsync();
        return tag;
    }

    public Tag UpdateTag(Tag tag)
    {
        _context.Update(tag);
        _context.SaveChanges();
        return tag;
    }

    public Tag DeleteTag(Tag tag)
    {
        _context.Tags.Remove(tag);
        _context.SaveChanges();
        return tag;
    }

   
}