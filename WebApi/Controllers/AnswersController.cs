using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization;
using WebApi.Entities;
using WebApi.Models.Answers;
using WebApi.Services;
using WebApi.Services.AnswerServices;
using WebApi.Services.QuestionServices;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AnswersController : ControllerBase
{
    private readonly IAnswerService _answerService;
    private readonly IQuestionService _questionService;
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AnswersController(IAnswerService answerService, IQuestionService questionService, IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _answerService = answerService;
        _questionService = questionService;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("{answerId}")]
    public async Task<IActionResult> GetAnswer(int answerId)
    {
        var answer = await _answerService.GetAnswerById(answerId);
        
        if (answer == null)
            return NotFound("Answer is not found!");
        
        return Ok(await _answerService.GetTheAnswer(answerId));
    }

    [HttpPost("{questionId}")]
    public async Task<IActionResult> CreateAnswer(int questionId, BaseAnswerModel model)
    {
        var userObject = (User) _httpContextAccessor.HttpContext?.Items["User"];
        
        var question = await _questionService.CheckIfQuestionExists(questionId);
        
        if (userObject == null)
            return NotFound("User is not found!");
        
        if (question == null)
            return NotFound("question is not found!");

        if (string.IsNullOrWhiteSpace(model.Body))
            return BadRequest("Answer body is required");

        var answer = new Answer
        {
            Body = model.Body,
            CreationTime = DateTime.Now,
            IsVerified = true,
            IsBanned = false,
            QuestionId = questionId,
            UserId = userObject.Id
        };

        await _answerService.CreateAnswer(answer);
        return Ok(await _answerService.GetTheAnswer(answer.Id));
    }

    [HttpPut("{answerId}")]
    public async Task<IActionResult> UpdateAnswer(int answerId, BaseAnswerModel model)
    {
        var userObject =  (User) _httpContextAccessor.HttpContext?.Items["User"];

        var answer = await _answerService.GetAnswerById(answerId);

        if (answer == null)
            return NotFound("answer is not found!");
        
        if (userObject == null)
            return NotFound("User is not found!");
        
        if (answer.UserId != userObject.Id)
            return BadRequest("Not Allowed!");

        if (string.IsNullOrWhiteSpace(model.Body))
            return BadRequest("answer Body is required!");


        answer.Body = model.Body;
        answer.UpdateTime = DateTime.Now;
        
        _answerService.UpdateAnswer(answer);
        return Ok(await _answerService.GetTheAnswer(answerId));
    }

    [HttpDelete("{answerId}")]
    public async Task<IActionResult> DeleteAnswer(int answerId)
    {
        var answer = await _answerService.GetAnswerById(answerId);
        
        if (answer == null)
            return NotFound("answer is not found!");

        _answerService.DeleteAnswer(answer);
        return Ok("Answer is deleted!");
    }
}

