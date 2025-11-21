using WebAppEmployeeApi.Domain.Models;

namespace WebAppEmployeeApi.Domain.Services
{
    public interface IEmployeeService
    {
        Task<(List<EmployeeModel> employees,int totalCount)> GetAllAsync(int pageNumber, int pageSize);
        Task<PaginatedResponse<EmployeeModel>> GetAllPagedAsync(int pageNumber, int pageSize, string? search, string? sortColumn, string? sortDirection);

        Task<EmployeeModel?> GetByIdAsync(int id);
        Task<EmployeeModel> AddAsync(EmployeeModel model);
        Task<bool> UpdateAsync(EmployeeModel model);
        Task<bool> DeleteAsync(int id);
        bool TryGetDesignation(int empId, out string designation);

    }
}
