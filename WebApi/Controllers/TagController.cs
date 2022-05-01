using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;
using WebApi.Models.Tags;
using WebApi.Services.TageServices;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var tags =  _tagService.GetAllTags();

            if (tags == null)
                return NotFound("There are no tags");
            
            return Ok(tags);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTagById(int id)
        {
            var checkTag =await _tagService.CheckIfTagExists(id);
            
            if (checkTag == null)
                return NotFound("Tag is not found");
            
            var tag = _tagService.GetTagById(id);
            return Ok(tag);
        }

        [HttpPost("CreateTag {userId}")]
        public async Task<IActionResult> CreateTag(int userId,BaseTagRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.Name)) return BadRequest("tag name is required!");
            var tag =  new Tag
            {
                Name = model.Name,
                Description = model.Description,
                UserId = userId
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _tagService.CheckIfTagExists(id);
            if (tag == null)
                return NotFound("Tag is not found");

            _tagService.DeleteTag(tag);

            return Ok("Tag has been deleted");
        }
    }
}
