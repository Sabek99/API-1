using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization;
using WebApi.Entities;
using WebApi.Models.Pagination;
using WebApi.Models.Questions;
using WebApi.Services;
using WebApi.Services.QuestionServices;
using WebApi.Services.QuestionTagServices;
using WebApi.Services.TageServices;

[Authorize]
[Route("api/[controller]/[action]")]
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
    //if there is no question return no questions yet!
    public async Task<IActionResult> GetAllQuestions([FromQuery]PaginationParams @params)
    {
        var count = await _questionService.GetCount();
        var paginationMetaData =  new PaginationMetaData( @params.Page,count, @params.ItemPerPage);
        Response.Headers.Add("X-pagination",JsonSerializer.Serialize(paginationMetaData));

        if (count == 0)
            return NoContent();
        
        var questions =  await _questionService.GetAllQuestions(@params);
        return Ok(questions);
    }

    [HttpGet("GetAllQuestionsByUser/{userId}")]
    public  async Task<IActionResult> GetAllQuestionsByUserId(int userId, [FromQuery]PaginationParams @params)
    {
        var count = await _questionService.GetCountUserId(userId);
        var user = _userService.GetById(userId);
        
        if (user == null)
            return NotFound("No user was found!");

        var paginationMetaData =  new PaginationMetaData( @params.Page, count, @params.ItemPerPage);
        Response.Headers.Add("X-pagination",JsonSerializer.Serialize(paginationMetaData));

        if (count == 0)
            return NoContent();
        
        var questions = await _questionService.GetAllQuestionsByUserId(userId, @params);
        return Ok(questions);
    }

    [HttpGet("GetAllQuestionsByTag/{tagId}")]
    public async Task<IActionResult> GetAllQuestionsByTagId(int tagId, [FromQuery]PaginationParams @params)
    {
        var count = await _questionService.GetCountTagId(tagId);
        var tag = await _tagService.CheckIfTagExists(tagId);
        
        if (tag == null)
            return NotFound("Tag is not found!");
        
        var paginationMetaData =  new PaginationMetaData( @params.Page, count, @params.ItemPerPage);
        Response.Headers.Add("X-pagination",JsonSerializer.Serialize(paginationMetaData));
        
        if (count == 0)
            return NoContent();

        var questions = await _questionService.GetAllQuestionsByTagId(tagId,@params);
        return Ok(questions);
    }

    [HttpGet("GetQuestionById/{questionId}")]
    public async Task<IActionResult> GetQuestionById(int questionId)
    {
        var checkQuestion = await _questionService.CheckIfQuestionExists(questionId);
        
        if (checkQuestion == null)
            return NotFound("Question is not found!");

        var question =  await _questionService.GetQuestionById(questionId);

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
            CreationTime = DateTime.UtcNow,
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
        
        return Ok(await _questionService.GetQuestionById(question.Id));
    }
    
    [HttpPut("{questionId}")]
    public async Task<IActionResult> UpdateQuestion( int questionId, UpdateQuestionModel model)
    {
        var userObject = (User) _httpContextAccessor.HttpContext?.Items["User"];
        var question = await _questionService.CheckIfQuestionExists(questionId);

        if (userObject == null)
            return NotFound("user is not found!");
        
        if (question == null)
            return NotFound("Question is not found!");

        if (question.UserId != userObject.Id)
            return BadRequest("Not Allowed!");
            
        
        if (string.IsNullOrWhiteSpace(model.QuestionTitle) || string.IsNullOrWhiteSpace(model.QuestionBody))
            return BadRequest("Title and body are required");
        

        question.Title = model.QuestionTitle;
        question.Body = model.QuestionBody;
        question.UpdateTime = DateTime.UtcNow;
        _questionService.UpdateQuestion(question);
        
        
        return Ok(await _questionService.GetQuestionById(question.Id));
    }

    [HttpDelete("{questionId}")]
    public async Task<IActionResult> DeleteQuestion(int questionId)
    {
        
        var userObject = (User) _httpContextAccessor.HttpContext?.Items["User"];
        
        if (userObject == null)
            return NotFound("user is not found!");
        
        var question = await _questionService.CheckIfQuestionExists(questionId);
        
        if (question == null)
            return NotFound("Question is not found!");

        if (question.UserId != userObject.Id)
            return BadRequest("Not allowed!");
        
        _questionService.DeleteQuestion(question);
        return Ok("Question has been deleted!");
    }

}

