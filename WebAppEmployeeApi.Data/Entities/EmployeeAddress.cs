using System.ComponentModel.DataAnnotations;

namespace WebAppEmployeeApi.Data.Entities
{
    public class EmployeeAddress
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string HouseNo { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? HouseName { get; set; }

        [Required, MaxLength(50)]
        public string Place { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string City { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string State { get; set; } = string.Empty;

        public int EmployeeId { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Employee Employee { get; set; }
    }
}
