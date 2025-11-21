namespace WebAppEmployeeApi.Domain.Models
{
    public class EmployeeAddressModel
    {
        public int Id { get; set; }

        public string HouseNo { get; set; }

        public string? HouseName { get; set; }

        public string Place { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int EmployeeId { get; set; }

        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual EmployeeModel Employee { get; set; }
    }
}
