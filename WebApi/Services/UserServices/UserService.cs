using System.Globalization;

namespace WebApi.Services;

using AutoMapper;
using BCrypt.Net;
using Authorization;
using Entities;
using Helpers;
using Models.Users;


public class UserService : IUserService
{
    private DataContext _context;
    private IJwtUtils _jwtUtils;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
 
  
    public UserService(
        DataContext context,
        IJwtUtils jwtUtils,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _context = context;
        _jwtUtils = jwtUtils;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
       
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
        var user = _context.AspNetUsers.SingleOrDefault(x => x.Email == model.Email);

        // validate
        if (user == null || !BCrypt.Verify(model.Password, user.PasswordHash))
            throw new AppException("Username or password is incorrect");
        

        // authentication successful
        var response = _mapper.Map<AuthenticateResponse>(user);
        response.Token = _jwtUtils.GenerateToken(user);
        user.Token= response.Token;
        _context.AspNetUsers.Update(user);
        _context.SaveChanges();
        return response;
    }
    

    public IEnumerable<UserResponse> GetAll()
    {
        var users = _context.AspNetUsers.ToList();
        var response = _mapper.Map<IEnumerable<UserResponse>>(users);
        return response;
    }

    public User GetById(int id)
    {
        return GetUser(id);
    }

    public RegisterResponse Register(RegisterRequest model)
    {
        // validate
        if (_context.AspNetUsers.Any(x => x.Email == model.Email))
            throw new AppException("Email '" + model.Email + "' is already taken");
        
        if (_context.AspNetUsers.Any(x => x.UserName == model.Username))
            throw new AppException("Username '" + model.Username + "' is already taken");
        
        if (model.Role != Role.Mentor && model.Role != Role.Student)
            throw new AppException("Role: " + model.Role + " is not valid");
        
        // map model to new user object
        var user = _mapper.Map<User>(model);
        // hash password
        user.PasswordHash = BCrypt.HashPassword(model.Password);
        user.CreationTime = DateTime.UtcNow;
        // save user
        _context.AspNetUsers.Add(user);
        _context.SaveChanges();
        
        // generate token
        var response = _mapper.Map<RegisterResponse>(user);
        response.Token = _jwtUtils.GenerateToken(user);
        user.Token= response.Token;
        _context.AspNetUsers.Update(user);
        _context.SaveChanges();
        // return user's token to client 
        return response;
    }

    public void Update(UpdateRequest model)
    {
        var user = (User) _httpContextAccessor.HttpContext?.Items["User"];
        
        if (user == null)
            throw new AppException("User is not found!");

        // validate
        if (model.Username != user.UserName && _context.AspNetUsers.Any(x => x.UserName == model.Username))
            throw new AppException("Username '" + model.Username + "' is already taken");
        
        if (model.Role != Role.Mentor && model.Role != Role.Student)
            throw new AppException("Role: " + model.Role + " is not valid");

        // hash password if it was entered
        if (!string.IsNullOrEmpty(model.Password))
            user.PasswordHash = BCrypt.HashPassword(model.Password);
        
        user.UpdateTime = DateTime.UtcNow;

        // copy model to user and save
        _mapper.Map(model, user);
        _context.AspNetUsers.Update(user);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var user = GetUser(id);
        _context.AspNetUsers.Remove(user);
        _context.SaveChanges();
    }

    // helper methods

    private User GetUser(int id)
    {
        var user = _context.AspNetUsers.Find(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }
    public void SignOut()
    {
        var user = (User) _httpContextAccessor.HttpContext?.Items["User"];
        if (user != null)
        {
            user.Token = null;
            _context.AspNetUsers.Update(user);
        }

        _context.SaveChanges();
    }
    
}