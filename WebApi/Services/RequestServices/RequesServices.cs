using System.Collections;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services.RequestServices;

public class RequesServices : IRequestService
{
    private readonly DataContext _context;

    public RequesServices(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable> GetAllRequests()
    {
        var request = await _context.Requests.Select(r => new
        {
            request_id = r.Id,
            request_body = r.RequestBody,
            request_status = r.Status,

            mentor = new
            {
                mentor_id = r.Mentor.Id,
                mentor_first_name = r.Mentor.FirstName,
                mentor_last_name = r.Mentor.LastName
            },
            
            student = new
            {
                student_id = r.Student.Id,
                student_first_name  = r.Student.FirstName,
                student_last_name = r.Student.LastName
            }
        }).ToListAsync();
        return request;
    }

    public async Task<IEnumerable> GetAllRequestsAsMentor()
    {
        var requests = await _context.Requests.Select(r => new
        {
            request_id = r.Id,
            request_body = r.RequestBody,
            request_status = r.Status,
            student = new
            {
                student_id = r.Student.Id,
                student_first_name = r.Student.FirstName,
                student_last_name = r.Student.LastName
            }
        }).ToListAsync();
        return requests;
    }

    public async Task<IEnumerable> GetAllRequestsAsStudent()
    {
        var requests = await _context.Requests.Select(r => new
        {
            request_id = r.Id,
            request_body = r.RequestBody,
            request_status = r.Status,
            mentor = new
            {
                mentor_id = r.Mentor.Id,
                mentor_first_name = r.Mentor.FirstName,
                mentor_last_name = r.Mentor.LastName
            }
        }).ToListAsync();
        return requests;
    }

    public async Task<IEnumerable> GetRequestById(int requestId)
    {
        var request = await _context.Requests
            .Where(r => r.Id == requestId)
            .Select(r => new
            {
                request_id = r.Id,
                request_body = r.RequestBody,
                request_status = r.Status,
                mentor = new
                {
                    mentor_id = r.Mentor.Id,
                    mentor_first_name = r.Mentor.FirstName,
                    mentor_Last_name = r.Mentor.LastName
                },
                student = new
                {
                    student_id = r.Student.Id,
                    student_first_name = r.Student.LastName,
                    student_last_name = r.Student.FirstName
                }
            }).ToListAsync();
        return request;
    }

    public async Task<Request> CreateRequest(Request request)
    {
        await _context.AddAsync(request);
        await _context.SaveChangesAsync();
        return request;
    }

    public Request UpdateRequest(Request request)
    {
        _context.Update(request);
        _context.SaveChanges();
        return request;
    }

    public Request DeleteRequest(Request request)
    {
        _context.Remove(request);
        _context.SaveChanges();
        return request;
    }

    public async Task<Request> CheckIfTheRequestExists(int requestId)
    {
        return await _context.Requests
            .SingleOrDefaultAsync(r => r.Id == requestId);
    }
}