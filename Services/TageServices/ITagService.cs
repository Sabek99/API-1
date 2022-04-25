using WebApi.Entities;
using WebApi.Models.Tags;

namespace WebApi.Services.TageServices;

public interface ITagService
{
    Task<IEnumerable<Tag>> GetAllTags();
    Task<Tag> GetTagById(int id);
    Task<Tag> CreateTag(Tag tag);
    Tag UpdateTag(Tag tag);
    Tag DeleteTag(Tag tag);
}