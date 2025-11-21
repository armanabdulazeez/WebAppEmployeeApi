using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppEmployeeApi.Domain.Models;

namespace WebAppEmployeeApi.Data.Entities
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Department { get; set; }

        [MaxLength(50)]
        public string? Designation { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Salary { get; set; }

        public DateTime JoinDate { get; set; } = DateTime.Now;

        public int UserId { get; set; }
        public User User { get; set; }
        public List<EmployeeAddress>? Addresses { get; set; }

        public Employee FromModel(EmployeeModel employeeModel)
        {
            if (employeeModel != null)
            {
                EmployeeId = employeeModel.EmployeeId;
                FullName = employeeModel.FullName;
                Department = employeeModel.Department;
                Designation = employeeModel.Designation;
                Salary = employeeModel.Salary;
                JoinDate = employeeModel.JoinDate;
                UserId = employeeModel.UserId;
            }
            return this;
        }

        public EmployeeModel ToModel()
        {
            return new EmployeeModel
            {
                EmployeeId = EmployeeId,
                FullName = FullName,
                Department = Department,
                Designation = Designation,
                Salary = Salary,
                JoinDate = JoinDate,
                UserId = UserId,
                User = User != null ? User.ToModel() : null
            };
        }
    }
}
