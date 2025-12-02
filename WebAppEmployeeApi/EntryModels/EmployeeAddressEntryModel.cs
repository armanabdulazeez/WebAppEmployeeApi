using WebAppEmployeeApi.Domain.Models;

namespace WebAppEmployeeApi.EntryModels
{
    public class EmployeeAddressEntryModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public string HouseNo { get; set; } = string.Empty;
        public string HouseName { get; set; } = string.Empty;
        public string Place { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;

        public EmployeeAddressModel ToModel()
        {
            return new EmployeeAddressModel
            {
                Id = this.Id,
                EmployeeId = this.EmployeeId,
                HouseNo = this.HouseNo,
                HouseName = this.HouseName,
                Place = this.Place,
                City = this.City,
                State = this.State
            };
        }
    }
}
