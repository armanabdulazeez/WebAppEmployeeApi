namespace WebAppEmployeeApi.Domain.Models
{
    public class EmployeeModel
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Department { get; set; }
        public string? Designation { get; set; }
        public decimal Salary { get; set; }
        public DateTime JoinDate { get; set; }
        public int UserId { get; set; }
        public UserModel? User { get; set; }

        public List<EmployeeAddressModel>? Addresses { get; set; }
    }
}
