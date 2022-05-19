using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization;
using WebApi.Entities;
using WebApi.Models.Tags;
using WebApi.Services;
using WebApi.Services.TageServices;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TagsController : ControllerBase
{
    private readonly ITagService _tagService;
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public TagsController(ITagService tagService, IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _tagService = tagService;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public  IActionResult GetAllTags()
    {
        var tags =  _tagService.GetAllTags();

        if (tags == null)
            return NotFound("There are no tags");
        
        return Ok(tags);
    }

    [HttpGet("{tagId}")]
    public async Task<IActionResult> GetTagById(int tagId)
    {
        var checkTag =await _tagService.CheckIfTagExists(tagId);
        
        if (checkTag == null)
            return NotFound("Tag is not found");
        
        var tag = _tagService.GetTagById(tagId);
        return Ok(tag);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTag(BaseTagModel model)
    {
        var userObject = (User) _httpContextAccessor.HttpContext?.Items["User"];
        
        if (userObject == null)
            return NotFound("User is not found!");
        
        if (string.IsNullOrWhiteSpace(model.Name)) return BadRequest("tag name is required!");
        var tag =  new Tag
        {
            Name = model.Name,
            Description = model.Description,
            User = userObject,
            UserId = userObject.Id
        };

        await _tagService.CreateTag(tag);
        return Ok(_tagService.GetTagById(tag.Id));
    }

    [HttpPut("{tagId}")]
    public async Task<IActionResult> UpdateTag(int tagId, BaseTagModel model)
    {
        var userObject = (User) _httpContextAccessor.HttpContext?.Items["User"];
        
        if (userObject == null)
            return NotFound("User is not found!");
        
        var tag = await _tagService.CheckIfTagExists(tagId);

        if (tag == null)
            return NotFound("Tag is not found");

        if (tag.UserId != userObject.Id)
            return BadRequest("Not allowed!");
        
        tag.Name = model.Name;
        tag.Description = model.Description;
           
        if(string.IsNullOrWhiteSpace(model.Name))
            return BadRequest("tag name is required!"); 
                   
        _tagService.UpdateTag(tag);
        return Ok(_tagService.GetTagById(tag.Id));
    }

    [HttpDelete("{tagId}")]
    public async Task<IActionResult> DeleteTag(int tagId)
    {
        var userObject = (User) _httpContextAccessor.HttpContext?.Items["User"];
        
        if (userObject == null)
            return NotFound("User is not found!");
        
        var tag = await _tagService.CheckIfTagExists(tagId);
        
        if (tag == null)
            return NotFound("Tag is not found");

        if (tag.UserId != userObject.Id)
            return BadRequest("Not allowed!");

        _tagService.DeleteTag(tag);

        return Ok("Tag has been deleted");
    }
}

