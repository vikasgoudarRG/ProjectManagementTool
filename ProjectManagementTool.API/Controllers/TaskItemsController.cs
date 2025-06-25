using Microsoft.AspNetCore.Mvc;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Application.DTOs.TaskItem;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementTool.API.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskItemController : ControllerBase
    {
        private readonly ITaskItemService _taskService;

        public TaskItemController(ITaskItemService taskService)
        {
            _taskService = taskService;
        }

        // === Create ===
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskItemCreateDTO dto)
        {
            TaskItemDTO task = await _taskService.CreateTaskAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = task.Id, requesterId = dto.RequestorId }, task);
        }

        // === Read ===
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id, [FromQuery] Guid requesterId)
        {
            TaskItemDTO? task = await _taskService.GetByIdAsync(id, requesterId);
            return task is null ? NotFound() : Ok(task);
        }

        [HttpGet("project/{projectId:guid}")]
        public async Task<IActionResult> GetByProject([FromRoute] Guid projectId, [FromQuery] Guid requesterId)
        {
            IEnumerable<TaskItemDTO> tasks = await _taskService.GetByProjectAsync(projectId, requesterId);
            return Ok(tasks);
        }

        [HttpGet("team/{teamId:guid}")]
        public async Task<IActionResult> GetByTeam([FromRoute] Guid teamId, [FromQuery] Guid requesterId)
        {
            IEnumerable<TaskItemDTO> tasks = await _taskService.GetByTeamAsync(teamId, requesterId);
            return Ok(tasks);
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetByUser([FromRoute] Guid userId)
        {
            IEnumerable<TaskItemDTO> tasks = await _taskService.GetByUserAsync(userId);
            return Ok(tasks);
        }

        // === Update ===
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromQuery] Guid updaterId, [FromBody] UpdateTaskItemDTO dto)
        {
            TaskItemDTO updated = await _taskService.UpdateAsync(id, dto, updaterId);
            return Ok(updated);
        }

        [HttpPost("{id:guid}/assign")]
        public async Task<IActionResult> Assign([FromRoute] Guid id, [FromBody] AssignTaskItemDTO dto)
        {
            dto.TaskItemId = id;
            TaskItemDTO updated = await _taskService.AssignAsync(dto);
            return Ok(updated);
        }

        [HttpPost("{id:guid}/status")]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] ChangeTaskItemStatusDTO dto)
        {
            TaskItemDTO updated = await _taskService.ChangeStatusAsync(id, dto);
            return Ok(updated);
        }

        // === Delete ===
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, [FromQuery] Guid requesterId)
        {
            await _taskService.DeleteAsync(id, requesterId);
            return NoContent();
        }
    }
}
