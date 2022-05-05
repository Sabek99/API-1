using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;
using WebApi.Models.Answers;
using WebApi.Services;
using WebApi.Services.AnswerServices;
using WebApi.Services.QuestionServices;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerService _answerService;
        private readonly IQuestionService _questionService;
        private readonly IUserService _userService;

        public AnswersController(IAnswerService answerService, IQuestionService questionService, IUserService userService)
        {
            _answerService = answerService;
            _questionService = questionService;
            _userService = userService;
        }

        [HttpGet("{answerId}")]
        public async Task<IActionResult> GetAnswer(int answerId)
        {
            var answer = await _answerService.GetAnswerById(answerId);
            
            if (answer == null)
                return NotFound("Answer is not found!");
            
            return Ok(answer);
        }

        [HttpPost("{userId} {questionId}")]
        public async Task<IActionResult> CreateAnswer(int userId,int questionId, BaseAnswerModel model)
        {
            var user = _userService.GetById(userId);
            var question = await _questionService.CheckIfQuestionExists(questionId);
            
            if (user == null)
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
                UserId = userId
            };
            
            await _answerService.CreateAnswer(answer);
            return Ok(answer);
        }

        [HttpPut("{answerId}")]
        public async Task<IActionResult> UpdateAnswer(int answerId, BaseAnswerModel model)
        {
            var checkAnswer = await _answerService.GetAnswerById(answerId);

            if (checkAnswer == null)
                return NotFound("answer is not found!");

            if (string.IsNullOrWhiteSpace(model.Body))
                return BadRequest("answer Body is required!");

            var answer = new Answer
            {
                Body = model.Body,
                UpdateTime = DateTime.Now
            };

            return Ok(answer);
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
}
