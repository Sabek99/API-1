using System.Collections;
using WebApi.Entities;

namespace WebApi.Services.RequestServices;

public interface IRequestService
{
    Task<IEnumerable> GetAllRequests();
    Task<IEnumerable> GetAllRequestsAsMentor();
    Task<IEnumerable> GetAllRequestsAsStudent();

    Task<IEnumerable> GetRequestById(int requestId);
    Task<Request> CreateRequest(Request request);
    Request UpdateRequest(Request request);
    Request DeleteRequest(Request request);

    Task<Request> CheckIfTheRequestExists(int requestId);
}