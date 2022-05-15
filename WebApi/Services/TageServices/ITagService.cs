using WebApi.Entities;

namespace WebApi.Services.TageServices;

public interface ITagService
{
    Task<Tag> CheckIfTagExists(int id);
    IQueryable GetAllTags();
    IQueryable GetTagById(int id);
    Task<Tag> CreateTag(Tag tagModel);
    Tag UpdateTag(Tag tag);
    Tag DeleteTag(Tag tag);
    
}