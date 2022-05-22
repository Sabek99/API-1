using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization;
using WebApi.Entities;
using WebApi.Models.Answers;
using WebApi.Services.AnswerServices;
using WebApi.Services.QuestionServices;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AnswersController : ControllerBase
{
    private readonly IAnswerService _answerService;
    private readonly IQuestionService _questionService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AnswersController(IAnswerService answerService, IQuestionService questionService, IHttpContextAccessor httpContextAccessor)
    {
        _answerService = answerService;
        _questionService = questionService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("{answerId}")]
    public async Task<IActionResult> GetAnswer(int answerId)
    {
        var answer = await _answerService.GetAnswerById(answerId);
        
        if (answer == null)
            return NotFound(new {error = "Answer is not found!",status_code = 404 });
        
        return Ok(await _answerService.GetTheAnswer(answerId));
    }

    [Authorize(Role.Mentor)]
    [HttpPost("{questionId}")]
    public async Task<IActionResult> CreateAnswer(int questionId, BaseAnswerModel model)
    {
        var userObject = (User) _httpContextAccessor.HttpContext?.Items["User"];
        
        var question = await _questionService.CheckIfQuestionExists(questionId);
        
        if (userObject == null)
            return NotFound(new {error = "User is not found!",status_code = 404 });
        
        if (question == null)
            return NotFound(new {error = "Question is not found!",status_code = 404 });

        if (string.IsNullOrWhiteSpace(model.Body))
            return BadRequest(new {error = "Answer body is required!",status_code = 400 });

        var answer = new Answer
        {
            Body = model.Body,
            CreationTime = DateTime.UtcNow,
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
            return NotFound(new {error = "Answer is not found!",status_code = 404 });
        
        if (userObject == null)
            return NotFound(new {error = "User is not found!",status_code = 404 });
        
        if (answer.UserId != userObject.Id)
            return BadRequest(new {error = "you are not allowed!",status_code = 400 });

        if (string.IsNullOrWhiteSpace(model.Body))
            return BadRequest(new {error = "Answer body is required!",status_code = 400 });


        answer.Body = model.Body;
        answer.UpdateTime = DateTime.UtcNow;
        
        _answerService.UpdateAnswer(answer);
        return Ok(await _answerService.GetTheAnswer(answerId));
    }
    
    
    [HttpDelete("{answerId}")]
    public async Task<IActionResult> DeleteAnswer(int answerId)
    {
        var userObject =  (User) _httpContextAccessor.HttpContext?.Items["User"];
        
        if (userObject == null)
            return NotFound(new {error = "User is not found!",status_code = 404 });
        
        var answer = await _answerService.GetAnswerById(answerId);
        
        if (answer == null)
            return NotFound(new {error = "Answer is not found!",status_code = 404 });

        if (answer.UserId != userObject.Id)
            return BadRequest(new {error = "you are not allowed!",status_code = 400 });
        
        _answerService.DeleteAnswer(answer);
        return Ok("Answer is deleted!");
    }
}

