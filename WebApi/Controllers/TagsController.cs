using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization;
using WebApi.Entities;
using WebApi.Models.Tags;
using WebApi.Services;
using WebApi.Services.TageServices;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly IUserService _userService;

        public TagsController(ITagService tagService, IUserService userService)
        {
            _tagService = tagService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var tags =  _tagService.GetAllTags();

            if (tags == null)
                return NotFound("There are no tags");
            
            return Ok(tags);
        }

        [HttpGet("{TagId}")]
        public async Task<IActionResult> GetTagById(int tagId)
        {
            var checkTag =await _tagService.CheckIfTagExists(tagId);
            
            if (checkTag == null)
                return NotFound("Tag is not found");
            
            var tag = _tagService.GetTagById(tagId);
            return Ok(tag);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> CreateTag(int userId,BaseTagRequest model)
        {
            var user = _userService.GetById(userId);
            if (string.IsNullOrWhiteSpace(model.Name)) return BadRequest("tag name is required!");
            var tag =  new Tag
            {
                Name = model.Name,
                Description = model.Description,
                UserId = user.Id
            };

            await _tagService.CreateTag(tag);
            return Ok(_tagService.GetTagById(tag.Id));
        }

        [HttpPut("{tagId}")]
        public async Task<IActionResult> UpdateTag(int tagId, BaseTagRequest model)
        {
           var tag = await _tagService.CheckIfTagExists(tagId);

           if (tag == null)
               return NotFound("Tag is not found");
           
           tag.Name = model.Name;
           tag.Description = model.Description;
           
           if(string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Description))
               return BadRequest("tag name is required!"); 
                   
           _tagService.UpdateTag(tag);
           return Ok(_tagService.GetTagById(tag.Id));
        }

        [HttpDelete("{tagId}")]
        public async Task<IActionResult> DeleteTag(int tagId)
        {
            var tag = await _tagService.CheckIfTagExists(tagId);
            if (tag == null)
                return NotFound("Tag is not found");

            _tagService.DeleteTag(tag);

            return Ok("Tag has been deleted");
        }
    }
}
