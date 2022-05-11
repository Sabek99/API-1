using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization;
using WebApi.Entities;
using WebApi.Models.Questions;
using WebApi.Services;
using WebApi.Services.QuestionServices;
using WebApi.Services.QuestionTagServices;
using WebApi.Services.TageServices;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _questionService;
    private readonly IUserService _userService;
    private readonly ITagService _tagService;
    private readonly IQuestionTagService _questionTagService;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public QuestionsController(
        IQuestionService questionService,
        IUserService userService,
        ITagService tagService,
        IQuestionTagService questionTagService,
        IHttpContextAccessor httpContextAccessor
        )
    {
        _questionService = questionService;
        _userService = userService;
        _tagService = tagService;
        _questionTagService = questionTagService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public IActionResult GetAllQuestions()
    {
        var questions = _questionService.GetAllQuestions();
        return Ok(questions);
    }

    [HttpGet("GetAllQuestionsByUser {userId}")]
    public  IActionResult GetAllQuestionsByUserId(int userId)
    {
        var user =  _userService.GetById(userId);
        
        if (user == null)
            return NotFound("user not found!");

        var questions = _questionService.GetAllQuestionsByUserId(userId);
        return Ok(questions);
    }

    [HttpGet("GetAllQuestionsByTag {tagId}")]
    public async Task<IActionResult> GetAllQuestionsByTagId(int tagId)
    {
        var tag = await _tagService.CheckIfTagExists(tagId);
        
        if (tag == null)
            return NotFound("Tag is not found!");

        var questions = _questionService.GetAllQuestionsByTagId(tagId);
        return Ok(questions);
    }

    [HttpGet("GetQuestionById {questionId}")]
    public async Task<IActionResult> GetQuestionById(int questionId)
    {
        var checkQuestion = await _questionService.CheckIfQuestionExists(questionId);
        
        if (checkQuestion == null)
            return NotFound("Question is not found!");

        var question = _questionService.GetQuestionById(questionId);

        return Ok(question);
    }
    
  

    [HttpPost]
    public async Task<IActionResult> CreateQuestion(BaseQuestionModel model)
    {
        var userObject = (User) _httpContextAccessor.HttpContext?.Items["User"];

        if (userObject == null)
            return NotFound("user is not found!");
        
        if (string.IsNullOrWhiteSpace(model.QuestionTitle) || string.IsNullOrWhiteSpace(model.QuestionBody))
            return BadRequest("Title and body are required");

        foreach (var tag in model.TagsId)
        {
            var checkTag = await _tagService.CheckIfTagExists(tag);
            if (checkTag == null)
                return NotFound($"tag with this ID: {tag} is not valid!");
        }
        
        var question = new Question
        {
            Title = model.QuestionTitle,
            Body = model.QuestionBody,
            CreationTime = DateTime.Now,
            UserId = userObject.Id
        };
        
        await _questionService.CreateQuestion(question);
        
        foreach (var tag in model.TagsId)
        {
            var questionTag = new QuestionTag
            {
                QuestionId = question.Id,
                TagId = tag
            };
            await _questionTagService.CreateQuestionTag(questionTag);
        }
        
        return Ok(_questionService.GetQuestionById(question.Id));
    }
    
    //this request is not working correctly yet 
    //updating the tags in questionTags table is the problem 
    [HttpPut("{userId} {questionId}")]
    public async Task<IActionResult> UpdateQuestion(int userId, int questionId, BaseQuestionModel model)
    {
        var user = _userService.GetById(userId);
        var question = await _questionService.CheckIfQuestionExists(questionId);

        if (user == null)
            return NotFound("user is not found!");
        
        if (question == null)
            return NotFound("Question is not found!");
        
        if (string.IsNullOrWhiteSpace(model.QuestionTitle) || string.IsNullOrWhiteSpace(model.QuestionBody))
            return BadRequest("Title and body are required");

        foreach (var tag in model.TagsId)
        {
            var checkTag = await _tagService.CheckIfTagExists(tag);
            if (checkTag == null)
                return NotFound($"tag with this ID: {tag} is not valid!");
        }

        question.Title = model.QuestionTitle;
        question.Body = model.QuestionBody;
        _questionService.UpdateQuestion(question);
        
        
        return Ok(_questionService.GetQuestionById(question.Id));
    }

    [HttpDelete("{questionId}")]
    public async Task<IActionResult> DeleteQuestion(int questionId)
    {
        var question = await _questionService.CheckIfQuestionExists(questionId);
        
        if (question == null)
            return NotFound("Question is not found!");
        
        _questionService.DeleteQuestion(question);
        return Ok("Question has been deleted!");
    }

}
