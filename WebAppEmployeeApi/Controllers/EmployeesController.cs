using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppEmployeeApi.Domain.Models;
using WebAppEmployeeApi.Domain.Services;
using WebAppEmployeeApi.EntryModels;
using WebAppEmployeeApi.Services.RepServices;
using WebAppEmployeeApi.ViewModels;

namespace WebAppEmployeeApi.Controllers
{
    public class EmployeesController : BaseApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly EmpCacheService _cacheService;

        public EmployeesController(IEmployeeService employeeService, EmpCacheService cacheService)
        {
            _employeeService = employeeService;
            _cacheService = cacheService;
        }

        [HttpGet("GetAllEmployeesServer")]
        public async Task<IActionResult> GetAllEmployeesServer(
            [FromQuery] int draw,
            [FromQuery] int start,
            [FromQuery] int length)
        {
            string searchValue = Request.Query["search[value]"];

            string sortColumnIndex = Request.Query["order[0][column]"];
            string sortDirection = Request.Query["order[0][dir]"];

            string sortColumn = "";
            if (!string.IsNullOrWhiteSpace(sortColumnIndex))
            {
                sortColumn = Request.Query[$"columns[{sortColumnIndex}][data]"];
            }

            int pageNumber = (start / length) + 1;
            int pageSize = length;

            var result = await _employeeService.GetAllPagedAsync(pageNumber, pageSize, searchValue, sortColumn, sortDirection);

            return Ok(new
            {
                draw = draw,
                recordsTotal = result.TotalCount,
                recordsFiltered = result.TotalCount,
                data = result.Data
            });
        }



        [HttpGet("GetAllEmployees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        [Authorize]
        public async Task<ActionResult> GetAllEmployees(int pageNumber = 1, int pageSize = 3)
        {
            var result = await _employeeService.GetAllAsync(pageNumber, pageSize);
            var employees = result.employees;
            int totalCount = result.totalCount;

            string cacheKey = $"employees_page_{pageNumber}_size_{pageSize}";

            if (_cacheService.TryGet(cacheKey, out var cachedEmployees))
            {
                var cachedResponse = new
                {
                    FromCache = true,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Data = cachedEmployees
                            .Select(EmployeeViewModel.FromModel)
                            .ToList()
                };

                return Ok(cachedResponse);
            }

            _cacheService.Set(cacheKey, employees);

            var response = new
            {
                FromCache = false,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = employees
                        .Select(EmployeeViewModel.FromModel)
                        .ToList()
            };

            if (response.Data.Count == 0)
                return NoContent();

            return Ok(response);
        }

        [HttpGet("GetEmployeeById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize]
        public async Task<ActionResult<EmployeeViewModel>> GetById(int id)
        {
            EmployeeModel? employeeModel = await _employeeService.GetByIdAsync(id);
            if (employeeModel == null)
                return NotFound("Employee not found");

            EmployeeViewModel? employeeViewModel = EmployeeViewModel.FromModel(employeeModel);
            return Ok(employeeViewModel);

        }

        [HttpGet("GetDesignation/{id}")]
        public ActionResult GetDesignation(int id)
        {
            if (_employeeService.TryGetDesignation(id, out string designation))
            {
                return Ok(new { EmployeeId = id, Designation = designation });
            }
            return NotFound(new { message = designation });
        }


        [HttpPost("CreateEmployee")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<EmployeeViewModel>> CreateAsync([FromBody] EmployeeEntryModel employeeEntryModel)
        {
            EmployeeModel employeeModel = employeeEntryModel.ToModel();
            EmployeeModel createdEmployee = await _employeeService.AddAsync(employeeModel);
            _cacheService.ClearAll();

            if (createdEmployee == null)
                return BadRequest(new { message = "Employee creation failed." });

            var employeeViewModel = EmployeeViewModel.FromModel(createdEmployee);

            return CreatedAtAction(
                nameof(GetById),
                new { id = employeeViewModel.EmployeeId },
                employeeViewModel);
            //return Ok(EmployeeViewModel.FromModel(createdEmployee));

        }

        [HttpPut("UpdateEmployee/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> UpdateEmployeeAsync(int id, [FromBody] EmployeeEntryModel employeeEntryModel)
        {
            if (id != employeeEntryModel.EmployeeId)
                return BadRequest("Employee ID mismatch.");

            EmployeeModel employeeModel = employeeEntryModel.ToModel();
            bool result = await _employeeService.UpdateAsync(employeeModel);
            if (result)
            {
                _cacheService.ClearAll();
                return NoContent();
            }
            else
            {
                return NotFound(new { message = "Employee not found or could not be updated." });
            }
        }

        [HttpPut("UpsertEmployee/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpsertEmployeeAsync(int id, [FromBody] EmployeeEntryModel employeeEntryModel)
        {
            if (id != employeeEntryModel.EmployeeId)
                return BadRequest("Employee ID mismatch.");

            var employeeModel = employeeEntryModel.ToModel();
            bool result = await _employeeService.UpsertAsync(employeeModel);

            if (result)
            {
                _cacheService.ClearAll();
                return NoContent();
            }

            return NotFound(new { message = "Employee could not be upserted." });
        }



        [HttpDelete("DeleteEmployee/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> DeleteEmployeeAsync(int id)
        {
            if (id > 0)
            {

                bool result = await _employeeService.DeleteAsync(id);
                if (result)
                {
                    _cacheService.ClearAll();
                    return NoContent();
                }
                else
                    return NotFound(new { message = "Employee not found or could not be deleted." });
            }
            return BadRequest("Invalid employee ID.");
        }


    }
}