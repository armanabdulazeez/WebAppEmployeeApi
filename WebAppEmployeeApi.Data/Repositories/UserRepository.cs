using Microsoft.EntityFrameworkCore;
using WebAppEmployeeApi.Data.DbEntities;
using WebAppEmployeeApi.Data.Entities;
using WebAppEmployeeApi.Domain.Models;
using WebAppEmployeeApi.Domain.Repositories;

namespace WebAppEmployeeApi.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MVPDbContext _context;

        public UserRepository(MVPDbContext context)
        {
            _context = context;
        }

        public async Task<UserModel?> GetByUsernameAsync(string username)
        {
            var user = await _context.Users
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user != null)
                return user.ToModel();
            else return null;
        }

        public async Task<UserModel> AddAsync(UserModel model)
        {
            var user = new User().FromModel(model);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.ToModel();
        }
    }
}
