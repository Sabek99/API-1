using WebApi.Entities;

namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Authorization;
using Models.Users;
using Services;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;
    private IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UsersController(IUserService userService, IMapper mapper,IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

 
    [HttpPost("authenticate")]
    public IActionResult Authenticate(AuthenticateRequest model)
    {
        var response = _userService.Authenticate(model);
        return Ok(response);
    }
    
    [HttpPost("SignOut")]
    public IActionResult SignOut()
    {
        _userService.SignOut();
        return Ok(new { message = "Signed out successfully, hope to see you soon!" });
    }

    
  
    [HttpPost("register")]
    public IActionResult Register(RegisterRequest model)
    {
        var response = _userService.Register(model);
        return Ok(response);
    }

    
    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _userService.GetAll();
        return Ok(users);
    }
    
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var user = _userService.GetById(id);
        var response = _mapper.Map<UserResponse>(user);
        return Ok(response);
    }
    
    [HttpPut("Update")]
    public IActionResult Update(UpdateRequest model)
    {
        _userService.Update(model);
        var user = (User) _httpContextAccessor.HttpContext?.Items["User"];
        var response = _mapper.Map<UserResponse>(user);
        return Ok(response);
    }
    
    [Authorize(Role.Admin)]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _userService.Delete(id);
        return Ok(new { message = "User deleted successfully"});
    }
    
    
}