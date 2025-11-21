using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppEmployeeApi.Domain.Models
{
    public class PaginatedResponse<T>
    {
        public bool FromCache { get; set; } = false;
        public int TotalCount { get; set; } = 0;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 3;
        public List<T> Data { get; set; } = new List<T>();
    }

}
