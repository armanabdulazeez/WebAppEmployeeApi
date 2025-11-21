using WebAppEmployeeApi.Domain.Models;

namespace WebAppEmployeeApi.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel?> GetByUsernameAsync(string username);
        Task<UserModel> AddAsync(UserModel model);
    }
}
