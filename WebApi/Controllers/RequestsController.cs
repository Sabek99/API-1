using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;
using WebApi.Models.Requests;
using WebApi.Services;
using WebApi.Services.RequestServices;

namespace WebApi.Controllers
{
    [Route("api/users/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;


        public RequestsController(IRequestService requestService, IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _requestService = requestService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRequests()
        {
            var requests = await _requestService.GetAllRequests();
            return Ok(requests);
        }

        [HttpGet("GetAllRequestAsMentor")]
        public async Task<IActionResult> GetAllRequestAsMentor()
        {
            var requests = await _requestService.GetAllRequestsAsMentor();
            return Ok(requests);
        }

        [HttpGet("GetAllRequestAsStudent")]
        public async Task<IActionResult> GetAllRequestAsStudent()
        {
            var requests = await _requestService.GetAllRequestsAsStudent();
            return Ok(requests);
        }

        [HttpGet("GetRequestById {requestId}")]
        public async Task<IActionResult> GetRequestById(int requestId)
        {
            var checkRequest = await _requestService.CheckIfTheRequestExists(requestId);
            
            if (checkRequest == null)
                return NotFound(new {error = "Request is not found!",status_code = 404 });
            
            var request = await _requestService.GetRequestById(requestId);
            return Ok(request);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest(BaseRequestModel model)
        {
            var userObject = (User) _httpContextAccessor.HttpContext?.Items["User"];

            if (userObject == null)
                return NotFound(new {error = "User is not found!",status_code = 404 });

            var checkUserRole = await _userService.CheckUserRole(model.MentorId);
            
            if(checkUserRole == null)
                return NotFound(new {error = "Mentor is not found!",status_code = 404 });
            
            if(string.IsNullOrWhiteSpace(model.RequestBody))
                return BadRequest(new {error = "Request body is required!",status_code = 400 });

            var request = new Request
            {
                StudentId = userObject.Id,
                MentorId = model.MentorId,
                RequestBody = model.RequestBody,
                Status = Status.OnHold
            };

            await _requestService.CreateRequest(request);

            return Ok(await _requestService.GetRequestById(request.Id));
        }

        [HttpPut("{requestId}")]
        public async Task<IActionResult> UpdateRequest(int requestId,BaseRequestModel model)
        {
            var userObject = (User) _httpContextAccessor.HttpContext?.Items["User"];

            if (userObject == null)
                return NotFound(new {error = "User is not found!",status_code = 404 });

            var request = await _requestService.CheckIfTheRequestExists(requestId);
            
            if(request == null)
                return NotFound(new {error = "Request is not found!",status_code = 404 });
            
            if(request.StudentId != userObject.Id)
                return BadRequest(new {error = "You are not allowed!",status_code = 400 });
            
            if(string.IsNullOrWhiteSpace(model.RequestBody))
                return BadRequest(new {error = "Request body is required!",status_code = 400 });

            request.MentorId = model.MentorId;
            request.RequestBody = model.RequestBody;

            _requestService.UpdateRequest(request);
            return Ok(await _requestService.GetRequestById(request.Id));
            
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRequest(int requestId)
        {
            var userObject = (User) _httpContextAccessor.HttpContext?.Items["User"];

            if (userObject == null)
                return NotFound(new {error = "User is not found!",status_code = 404 });

            var request = await _requestService.CheckIfTheRequestExists(requestId);
            
            if (request == null)
                return NotFound(new {error = "Request is not found!",status_code = 404 });
            
            if(request.StudentId != userObject.Id)
                return BadRequest(new {error = "You are not allowed!",status_code = 400 });

            _requestService.DeleteRequest(request);
            return Ok("Request has been deleted");

        }


    }
    

}
