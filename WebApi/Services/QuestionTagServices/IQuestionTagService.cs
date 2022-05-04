using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApi.Entities;
using WebApi.Models.QuestionTag;

namespace WebApi.Services.QuestionTagServices;

public interface IQuestionTagService
{
    Task<QuestionTag> GetQuestionTagByTagId(int tagId);
    Task<QuestionTag> CreateQuestionTag(QuestionTag questionTag);
    Task<QuestionTag> CheckIfQuestionTagExists(QuestionTagModel questionTag);
    QuestionTag UpdateQuestionTag(QuestionTag questionTag);
}