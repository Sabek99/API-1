using WebApi.Entities;
using WebApi.Models.Tags;

namespace WebApi.Services.TageServices;

public interface ITagService
{
    Task<Tag> CheckIfTagExists(int id);
    IQueryable GetAllTags();
    IQueryable GetTagById(int id);
    IQueryable GetSpecificTags(IEnumerable<int> tagsId);
    Task<Tag> CreateTag(Tag tagModel);
    Tag UpdateTag(Tag tag);
    Tag DeleteTag(Tag tag);
    
}