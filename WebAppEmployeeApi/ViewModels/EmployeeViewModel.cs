using WebAppEmployeeApi.Domain.Models;

namespace WebAppEmployeeApi.ViewModels
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Department { get; set; }
        public string? Designation { get; set; }
        public decimal Salary { get; set; }
        public DateTime JoinDate { get; set; }

        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public List<EmployeeAddressViewModel?>? Addresses { get; set; }

        public static EmployeeViewModel? FromModel(EmployeeModel employeeModel)
        {
            if (employeeModel != null)
            {
                return new EmployeeViewModel
                {
                    EmployeeId = employeeModel.EmployeeId,
                    FullName = employeeModel.FullName,
                    Department = employeeModel.Department,
                    Designation = employeeModel.Designation,
                    Salary = employeeModel.Salary,
                    JoinDate = employeeModel.JoinDate,
                    Username = employeeModel.User?.Username ?? string.Empty,
                    Role = employeeModel.User?.Role ?? string.Empty,
                    Addresses = employeeModel.Addresses?
                        .Select(a => EmployeeAddressViewModel.FromModel(a))
                        .ToList()
                };
            }
            return null;
        }
    }
}
