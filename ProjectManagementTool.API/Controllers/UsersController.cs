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
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddUserDTO addUserDto)
        {
            UserDTO userDto = await _userService.AddAsync(addUserDto);
            return CreatedAtAction(
                nameof(GetById),
                new { userId = userDto.Id },
                userDto
            );
        }

        // Read
        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid userId)
        {
            UserDTO userDto = await _userService.GetByIdAsync(userId);
            return Ok(userDto);
        }

        [HttpGet]
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
        [HttpPut("{userId:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid userId, [FromQuery] UpdateUserDTO updateUserDto)
        {
            await _userService.UpdateAsync(userId, updateUserDto);
            return NoContent();
        }

        // Delete
        [HttpDelete("{userId:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid userId)
        {
            await _userService.DeleteAsync(userId);
            return NoContent();
        }
    }
}
