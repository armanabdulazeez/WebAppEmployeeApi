using System.ComponentModel.DataAnnotations;
using WebAppEmployeeApi.Domain.Models;

namespace WebAppEmployeeApi.Data.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Role { get; set; } = "User";

        public Employee? Employee { get; set; }

        public User FromModel(UserModel userModel)
        {
            if (userModel != null)
            {
                UserId = userModel.UserId;
                Username = userModel.Username;
                Password = userModel.Password;
                Role = userModel.Role;
            }
            return this;
        }

        public UserModel ToModel()
        {
            return new UserModel
            {
                UserId = UserId,
                Username = Username,
                Role = Role,
                Password = Password
            };
        }
    }
}
