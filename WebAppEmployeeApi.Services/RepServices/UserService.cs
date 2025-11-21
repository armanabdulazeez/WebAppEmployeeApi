using Microsoft.AspNetCore.Identity;
using WebAppEmployeeApi.Domain.Models;
using WebAppEmployeeApi.Domain.Repositories;
using WebAppEmployeeApi.Domain.Services;
using WebAppEmployeeApi.Services.JwtToken;

namespace WebAppEmployeeApi.Services.RepServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenCreator _jwtTokenCreator;
        private readonly IPasswordHasher<UserModel> _passwordHasher;

        public UserService(IUserRepository userRepository, JwtTokenCreator jwtTokenCreator, IPasswordHasher<UserModel> passwordHasher)
        {
            _userRepository = userRepository;
            _jwtTokenCreator = jwtTokenCreator;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserModel> RegisterAsync(UserModel model)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(model.Username);
            if (existingUser != null)
                throw new Exception("Username already exists.");

            model.Password = _passwordHasher.HashPassword(model, model.Password);

            var createdUser = await _userRepository.AddAsync(model);
            return createdUser;
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            if (user == null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

            if (result == PasswordVerificationResult.Failed)
                return null;

            return _jwtTokenCreator.GenerateJwtToken(user);
        }
    }
}
