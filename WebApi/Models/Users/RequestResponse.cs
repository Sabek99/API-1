using AutoMapper.Configuration.Conventions;
using WebApi.Entities;

namespace WebApi.Models.Users;

public class RequestResponse
{
    public int RequestId { get; set; }
    public string RequestBody { get; set; }

    public UserResponse UserResponse { get; set; }

}