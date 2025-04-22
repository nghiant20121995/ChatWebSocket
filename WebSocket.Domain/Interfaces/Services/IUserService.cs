using System.Threading.Tasks;

namespace ChatWebSocket.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<string> LoginAsync(string username, string password);
    }
}
