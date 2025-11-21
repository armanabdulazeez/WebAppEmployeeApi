using WebAppEmployeeApi.Domain.Models;

namespace WebAppEmployeeApi.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public static UserViewModel FromModel(UserModel model)
        {

            return new UserViewModel
            {
                UserId = model.UserId,
                Username = model.Username,
                Role = model.Role
            };

        }
    }
}
