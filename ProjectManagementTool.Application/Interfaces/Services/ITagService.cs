using ProjectManagementTool.Application.DTOs.Tag;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface ITagService
    {
        // Task<TagDto> CreateTagAsync(string tagName);
        Task<IEnumerable<TagDto>> CreateTagsIfNotExistAsync(IEnumerable<string> tagNames);
        
    }
}