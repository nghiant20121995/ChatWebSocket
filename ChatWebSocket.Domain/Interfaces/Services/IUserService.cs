using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.RequestModel;
using ChatWebSocket.Domain.Response;
using System.Threading.Tasks;

namespace ChatWebSocket.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<LoginResponse> LoginAsync(LoginReq req);
        Task<User> GetByEmailAsync(string email);
    }
}
