using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppEmployeeApi.Domain.Models;

namespace WebAppEmployeeApi.Domain.Models
{
    public class UpdateEmployeeRequestModel
    {
        public EmployeeModel Employee { get; set; }
        public List<EmployeeAddressModel> Addresses { get; set; }
    }

}
