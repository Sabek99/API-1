using WebApi.Entities;
using WebApi.Models.Tags;

namespace WebApi.Services.TageServices;

public interface ITagService
{
    IQueryable GetAllTags();
    IQueryable GetTagById(int id);
    Task<Tag> CreateTag(Tag tag);
    Tag UpdateTag(Tag tag);
    Tag DeleteTag(Tag tag);
    Task<Tag> CheckIfTagExists(int id);
}