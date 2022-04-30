using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;

namespace WebApi.Services;

using AutoMapper;
using BCrypt.Net;
using Authorization;
using Entities;
using Helpers;
using Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

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
    

    public IEnumerable<User> GetAll()
    {
        return _context.AspNetUsers;
    }

    public User GetById(int id)
    {
        return getUser(id);
    }

    public RegisterResponse Register(RegisterRequest model)
    {
        // validate
        if (_context.AspNetUsers.Any(x => x.Email == model.Email))
            throw new AppException("Email '" + model.Email + "' is already taken");
        if (_context.AspNetUsers.Any(x => x.UserName == model.Username))
            throw new AppException("Username '" + model.Username + "' is already taken");
        
        // map model to new user object
        var user = _mapper.Map<User>(model);
        // hash password
        user.PasswordHash = BCrypt.HashPassword(model.Password);
        
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

    public void Update(int id, UpdateRequest model)
    {
        var user = getUser(id);

        // validate
        if (model.Username != user.UserName && _context.AspNetUsers.Any(x => x.UserName == model.Username))
            throw new AppException("Username '" + model.Username + "' is already taken");

        // hash password if it was entered
        if (!string.IsNullOrEmpty(model.Password))
            user.PasswordHash = BCrypt.HashPassword(model.Password);

        // copy model to user and save
        _mapper.Map(model, user);
        _context.AspNetUsers.Update(user);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var user = getUser(id);
        _context.AspNetUsers.Remove(user);
        _context.SaveChanges();
    }

    // helper methods

    private User getUser(int id)
    {
        var user = _context.AspNetUsers.Find(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }
    public void signOut()
    {
        var user = (User)_httpContextAccessor.HttpContext.Items["User"];
        user.Token = null;
        _context.AspNetUsers.Update(user);
        _context.SaveChanges();
    }
    
}