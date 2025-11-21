using System.ComponentModel.DataAnnotations;

namespace WebAppEmployeeApi.EntryModels
{
    public class LoginEntryModel
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
