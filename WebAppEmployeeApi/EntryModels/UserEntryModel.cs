using System.ComponentModel.DataAnnotations;
using WebAppEmployeeApi.CustomValidations;
using WebAppEmployeeApi.Domain.Models;

namespace WebAppEmployeeApi.EntryModels
{
    public class UserEntryModel
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        [ValidatePassword("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public UserModel ToModel(string role = "User")
        {
            return new UserModel
            {
                Username = this.Username,
                Password = this.Password,
                Role = role
            };
        }
    }
}
