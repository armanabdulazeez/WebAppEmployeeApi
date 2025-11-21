using WebAppEmployeeApi.Domain.Models;

namespace WebAppEmployeeApi.Domain.Services
{
    public interface IUserService
    {
        Task<UserModel> RegisterAsync(UserModel model);
        Task<string?> LoginAsync(string username, string password);
    }
}
