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

    public async Task<IEnumerable<Tag>> GetAllTags()
    {
        return await _context.Tags
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<Tag> GetTagById(int id)
    {
        return await _context.Tags
            .SingleOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Tag> CreateTag(Tag tag)
    {
        await _context.AddAsync(tag);
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