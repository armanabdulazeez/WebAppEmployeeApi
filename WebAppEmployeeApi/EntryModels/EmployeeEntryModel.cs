using System.ComponentModel.DataAnnotations;
using WebAppEmployeeApi.Domain.Models;

namespace WebAppEmployeeApi.EntryModels
{
    public class EmployeeEntryModel
    {
        public int EmployeeId { get; set; }
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Department { get; set; }

        [MaxLength(50)]
        public string? Designation { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }
        public DateTime JoinDate { get; set; }

        public int UserId { get; set; }

        public EmployeeModel ToModel()
        {
            return new EmployeeModel
            {
                EmployeeId = EmployeeId,
                FullName = this.FullName,
                Department = this.Department,
                Designation = this.Designation,
                Salary = this.Salary,
                JoinDate = this.JoinDate,
                UserId = this.UserId
            };
        }
    }
}
