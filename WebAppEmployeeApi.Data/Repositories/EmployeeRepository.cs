using Microsoft.EntityFrameworkCore;
using WebAppEmployeeApi.Data.DbEntities;
using WebAppEmployeeApi.Data.Entities;
using WebAppEmployeeApi.Domain.Models;
using WebAppEmployeeApi.Domain.Repositories;

namespace WebAppEmployeeApi.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MVPDbContext _context;

        public EmployeeRepository(MVPDbContext context)
        {
            _context = context;
        }

        public async Task<(List<EmployeeModel> employees,int totalCount)> GetAllAsync(int pageNumber, int pageSize)
        {
            int totalCount=await _context.Employees.CountAsync();

            var employees= await _context.Employees
                .AsNoTracking()
                .Include(e => e.User)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => e.ToModel())
                .ToListAsync();

            return (employees,totalCount);
        }

        
        public async Task<PaginatedResponse<EmployeeModel>> GetAllPagedAsync(int pageNumber, int pageSize, string? search, string? sortColumn, string? sortDirection)
        {
            var query = _context.Users
                .Where(u => u.Role != "Admin")
                .AsNoTracking()
                .Include(u => u.Employee)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string s = search.Trim();
                query = query.Where(u =>
                    u.Username.Contains(s) ||
                    (u.Employee != null && (
                        u.Employee.FullName.Contains(s) ||
                        (u.Employee.Department != null && u.Employee.Department.Contains(s)) ||
                        (u.Employee.Designation != null && u.Employee.Designation.Contains(s))
                    ))
                );
            }

            var totalCount = await query.CountAsync();

            bool isAscending = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);

            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                query = sortColumn switch
                {
                    "username" => isAscending ? query.OrderBy(u => u.Username) : query.OrderByDescending(u => u.Username),
                    "fullName" => isAscending ? query.OrderBy(u => u.Employee!.FullName) : query.OrderByDescending(u => u.Employee!.FullName),
                    "department" => isAscending ? query.OrderBy(u => u.Employee!.Department) : query.OrderByDescending(u => u.Employee!.Department),
                    "designation" => isAscending ? query.OrderBy(u => u.Employee!.Designation) : query.OrderByDescending(u => u.Employee!.Designation),
                    "salary" => isAscending ? query.OrderBy(u => u.Employee!.Salary) : query.OrderByDescending(u => u.Employee!.Salary),
                    "joinDate" => isAscending ? query.OrderBy(u => u.Employee!.JoinDate) : query.OrderByDescending(u => u.Employee!.JoinDate),
                    _ => query.OrderBy(u => u.UserId)
                };
            }
            else
            {
                query = query.OrderBy(u => u.UserId);
            }

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new EmployeeModel
                {
                    EmployeeId = u.Employee != null ? u.Employee.EmployeeId : 0,
                    FullName = u.Employee != null ? u.Employee.FullName : "",
                    Department = u.Employee.Department,
                    Designation = u.Employee.Designation,
                    Salary = u.Employee != null ? u.Employee.Salary : 0,
                    JoinDate = u.Employee != null ? u.Employee.JoinDate : DateTime.MinValue,
                    UserId = u.UserId,

                    User = new UserModel
                    {
                        UserId = u.UserId,
                        Username = u.Username,
                        Role = u.Role,
                        Password = u.Password
                    }
                })
                .ToListAsync();

            return new PaginatedResponse<EmployeeModel>
            {
                FromCache = false,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = items
            };
        }


        public async Task<EmployeeModel?> GetByIdAsync(int id)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            return employee.ToModel();
        }

        public async Task<EmployeeModel> AddAsync(EmployeeModel model)
        {
            var employee = new Employee().FromModel(model);
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee.ToModel();
        }

        public async Task<bool> UpdateAsync(UpdateEmployeeRequestModel model)
        {
            var employee = await _context.Employees
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.EmployeeId == model.Employee.EmployeeId);
            if (employee == null)
                return false;

            employee.FullName = model.Employee.FullName;
            employee.Department = model.Employee.Department;
            employee.Designation = model.Employee.Designation;
            employee.Salary = model.Employee.Salary;
            employee.JoinDate = model.Employee.JoinDate;

            if (employee.User != null)
            {
                employee.User.Username = model.Employee.User.Username;
                employee.User.Role = model.Employee.User.Role;
            }

            _context.Employees.Update(employee);
            bool result = await _context.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return false;

            _context.Employees.Remove(employee);
            bool result = await _context.SaveChangesAsync() > 0;
            return result;
        }

        public bool TryGetDesignationById(int id, out string designation)//make async
        {
            designation = string.Empty;

            var employee = _context.Employees
                .AsNoTracking()
                .FirstOrDefault(e => e.EmployeeId == id);

            if (employee != null)
            {
                designation = employee.Designation ?? string.Empty;
                return true;
            }

            return false;
        }

        public async Task<int> GetCountAsync(string? search)
        {
            var query = _context.Users
                .AsNoTracking()
                .Include(u => u.Employee)
                .Where(u => u.Role != "Admin")  
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string s = search.Trim();
                query = query.Where(u =>
                    u.Username.Contains(s) ||
                    (u.Employee != null && (
                        u.Employee.FullName.Contains(s) ||
                        (u.Employee.Department != null && u.Employee.Department.Contains(s)) ||
                        (u.Employee.Designation != null && u.Employee.Designation.Contains(s))
                    ))
                );
            }

            return await query.CountAsync();
        }


        public Task<bool> UpdateAsync(EmployeeModel model)
        {
            throw new NotImplementedException();
        }
    }
}