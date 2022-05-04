using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;
using WebApi.Models.Questions;
using WebApi.Models.QuestionTag;
using WebApi.Services;
using WebApi.Services.QuestionServices;
using WebApi.Services.QuestionTagServices;
using WebApi.Services.TageServices;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IUserService _userService;
        private readonly ITagService _tagService;
        private readonly IQuestionTagService _questionTagService;

        public QuestionsController(IQuestionService questionService, IUserService userService, ITagService tagService, IQuestionTagService questionTagService)
        {
            _questionService = questionService;
            _userService = userService;
            _tagService = tagService;
            _questionTagService = questionTagService;
        }

        [HttpGet]
        public IActionResult GetAllQuestions()
        {
            var questions = _questionService.GetAllQuestions();
            return Ok(questions);
        }

        [HttpGet("GetAllQuestionsByUser {userId}")]
        public async Task<IActionResult> GetAllQuestionsByUserId(int userId)
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

        [HttpPost("{userId}")]
        public async Task<IActionResult> CreateQuestion(int userId,BaseQuestionModel model)
        {
            var user = _userService.GetById(userId);

            if (user == null)
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
                UserId = user.Id
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

            foreach (var tag in model.TagsId)
            {
                var questionTag = new 
                
            }
            
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
}
