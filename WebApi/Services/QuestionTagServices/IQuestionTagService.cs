using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApi.Entities;

namespace WebApi.Services.QuestionTagServices;

public interface IQuestionTagService
{
    Task<EntityEntry<QuestionTag>> CreateQuestionTag(QuestionTag questionTag);
}