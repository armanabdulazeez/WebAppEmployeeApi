using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppEmployeeApi.Domain.Services;
using WebAppEmployeeApi.EntryModels;
using WebAppEmployeeApi.ViewModels;

namespace WebAppEmployeeApi.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register([FromBody] UserEntryModel model)
        {
            try
            {
                var userModel = model.ToModel();

                var createdUser = await _userService.RegisterAsync(userModel);
                if (createdUser == null)
                    return BadRequest(new { message = "Failed to create user." });

                var response = UserViewModel.FromModel(createdUser);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginEntryModel model)
        {
            var token = await _userService.LoginAsync(model.Username, model.Password);

            if (token == null)
                return Unauthorized(new { message = "Invalid username or password." });

            return Ok(new { Token = token });
        }

        
    }
}
