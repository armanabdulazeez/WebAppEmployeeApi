using WebAppEmployeeApi.Domain.Models;

namespace WebAppEmployeeApi.ViewModels
{
    public class EmployeeAddressViewModel
    {
        public int Id { get; set; }

        public string HouseNo { get; set; }

        public string? HouseName { get; set; }

        public string Place { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public static EmployeeAddressViewModel? FromModel(EmployeeAddressModel employeeAddressModel)
        {
            if (employeeAddressModel != null)
            {
                return new EmployeeAddressViewModel
                {
                    Id = employeeAddressModel.Id,
                    HouseNo = employeeAddressModel.HouseNo,
                    HouseName = employeeAddressModel.HouseName,
                    Place = employeeAddressModel.Place,
                    City = employeeAddressModel.City,
                    State = employeeAddressModel.State
                };
            }
            return null;
        }
    }
}
