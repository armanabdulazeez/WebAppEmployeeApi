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

        public async Task<PaginatedResponse<EmployeeModel>> GetAllPagedAsync(int pageNumber,int pageSize,string? search,string? sortColumn,string? sortDirection)
        {
            var query = _context.Employees
                .AsNoTracking()
                .Include(e => e.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string s = search.Trim();
                query = query.Where(e =>
                    e.FullName.Contains(s) ||
                    (e.Department != null && e.Department.Contains(s)) ||
                    (e.Designation != null && e.Designation.Contains(s)) ||
                    (e.User != null && e.User.Username.Contains(s))
                );
            }

            var totalCount = await query.CountAsync();

            bool isAscending  = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);
            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                switch (sortColumn)
                {
                    case "employeeId":
                        query = isAscending ? query.OrderBy(e => e.EmployeeId) : query.OrderByDescending(e => e.EmployeeId);
                        break;
                    case "fullName":
                        query = isAscending ? query.OrderBy(e => e.FullName) : query.OrderByDescending(e => e.FullName);
                        break;
                    case "department":
                        query = isAscending ? query.OrderBy(e => e.Department) : query.OrderByDescending(e => e.Department);
                        break;
                    case "designation":
                        query = isAscending ? query.OrderBy(e => e.Designation) : query.OrderByDescending(e => e.Designation);
                        break;
                    case "salary":
                        query = isAscending ? query.OrderBy(e => e.Salary) : query.OrderByDescending(e => e.Salary);
                        break;
                    case "joinDate":
                        query = isAscending ? query.OrderBy(e => e.JoinDate) : query.OrderByDescending(e => e.JoinDate);
                        break;
                    case "user.username":
                        query = isAscending ? query.OrderBy(e => e.User.Username) : query.OrderByDescending(e => e.User.Username);
                        break;
                    default:
                        query = query.OrderBy(e => e.EmployeeId);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(e => e.EmployeeId);
            }

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => e.ToModel())
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

        public async Task<bool> UpdateAsync(EmployeeModel model)
        {
            var employee = await _context.Employees.FindAsync(model.EmployeeId);
            if (employee == null)
                return false;

            employee.FromModel(model);
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
            var query = _context.Employees.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string s = search.Trim();
                query = query.Where(e =>
                    e.FullName.Contains(s) ||
                    (e.Department != null && e.Department.Contains(s)) ||
                    (e.Designation != null && e.Designation.Contains(s)) ||
                    (e.User != null && e.User.Username.Contains(s))
                );
            }

            return await query.CountAsync();
        }


    }
}
