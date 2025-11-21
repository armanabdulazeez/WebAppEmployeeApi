using Microsoft.IdentityModel.Tokens;
using System.Numerics;
using WebAppEmployeeApi.Domain.Models;
using WebAppEmployeeApi.Domain.Repositories;
using WebAppEmployeeApi.Domain.Services;

namespace WebAppEmployeeApi.Services.RepServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly AddressApiClient _addressApiClient;
        private readonly EmpCacheService _cacheService;

        public EmployeeService(IEmployeeRepository employeeRepository, AddressApiClient addressApiClient, EmpCacheService cacheService)
        {
            _employeeRepository = employeeRepository;
            _addressApiClient = addressApiClient;
            _cacheService = cacheService;
        }

        public async Task<EmployeeModel> AddAsync(EmployeeModel employeeModel)
        {
            employeeModel.JoinDate = DateTime.Now.Date;

            return await _employeeRepository.AddAsync(employeeModel);
        }

        public async Task<EmployeeModel?> GetByIdAsync(int employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                return null;

            var addresses = await _addressApiClient.GetAddressByEmpIdAsync(employeeId);
            employee.Addresses = addresses ?? new List<EmployeeAddressModel>();

            return employee;
        }

        //public async Task<PaginatedResponse<EmployeeModel>> GetAllPagedAsync(int pageNumber,int pageSize,string? search,string? sortColumn,string? sortDirection)
        //{
        //    return await _employeeRepository.GetAllPagedAsync(pageNumber, pageSize, search, sortColumn, sortDirection);
        //}

        public async Task<PaginatedResponse<EmployeeModel>> GetAllPagedAsync(int pageNumber,int pageSize,string? search,string? sortColumn,string? sortDirection)
        {
            string cacheKey= $"employees_page_{pageNumber}_size_{pageSize}_search_{search}_sort_{sortColumn}_{sortDirection}";
            int totalCount=await _employeeRepository.GetCountAsync(search);

            if (_cacheService.TryGet(cacheKey,out var cachedEmployees))
            {
                return new PaginatedResponse<EmployeeModel>
                {
                    FromCache = true,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Data = cachedEmployees
                };
            }

            var result = await _employeeRepository.GetAllPagedAsync(pageNumber, pageSize, search, sortColumn, sortDirection);

            _cacheService.Set(cacheKey, result.Data);

            result.FromCache = false;
            result.TotalCount = totalCount;
            return result;
        }


        public async Task<(List<EmployeeModel> employees,int totalCount)> GetAllAsync(int pageNumber, int pageSize)
            => await _employeeRepository.GetAllAsync(pageNumber, pageSize);

        public async Task<bool> DeleteAsync(int employeeId)
            => await _employeeRepository.DeleteAsync(employeeId);

        public async Task<bool> UpdateAsync(EmployeeModel employeeModel)
            => await _employeeRepository.UpdateAsync(employeeModel);

        public bool TryGetDesignation(int empId, out string designation)
            => _employeeRepository.TryGetDesignationById(empId, out designation);

    }
}
