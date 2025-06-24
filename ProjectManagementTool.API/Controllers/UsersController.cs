using Microsoft.AspNetCore.Mvc;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Application.DTOs.User;

namespace ProjectManagementTool.API.Controllers
{

    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        // ======================= Fields ======================= //
        private readonly IUserService _userService;

        // ==================== Constructors ==================== //
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // ======================= Methods ====================== //
        // Create
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddUserDTO addUserDto)
        {
            UserDTO userDto = await _userService.AddAsync(addUserDto);
            return Ok(userDto);
        }

        // Read
        [HttpGet("get")]
        public async Task<IActionResult> GetById([FromQuery] Guid userId)
        {
            UserDTO userDto = await _userService.GetByIdAsync(userId);
            return Ok(userDto);
        }

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<UserDTO> users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            IEnumerable<UserDTO> users = await _userService.SearchAsync(keyword);
            return Ok(users);
        }

        // Update
        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateUserDTO updateUserDto)
        {
            await _userService.UpdateAsync(updateUserDto);
            return NoContent();
        }

        // Delete
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            await _userService.DeleteAsync(userId);
            return NoContent();
        }
    }
}
