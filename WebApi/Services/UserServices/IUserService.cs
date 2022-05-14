using WebApi.Entities;
using WebApi.Models.Users;

namespace WebApi.Services;

public interface IUserService
{
    AuthenticateResponse Authenticate(AuthenticateRequest model);
    RegisterResponse Register(RegisterRequest model);
    IEnumerable<UserResponse> GetAll();
    User GetById(int id);
    //void Register(RegisterRequest model);
    void Update( UpdateRequest model);
    void Delete(int id);
    void SignOut();
}