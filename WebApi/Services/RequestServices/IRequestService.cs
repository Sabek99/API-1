using System.Collections;
using System.Text;
using WebApi.Entities;

namespace WebApi.Services.RequestServices;

public interface IRequestService
{
    Task<IEnumerable> GetAllRequests();
    Task<IEnumerable> GetAllRequestsAsMentor();
    Task<IEnumerable> GetAllRequestsAsStudent();
    
    Task<Request> CreateRequest(Request request);
    Request UpdateRequest(Request request);
    Request DeleteRequest(Request request);
    
}