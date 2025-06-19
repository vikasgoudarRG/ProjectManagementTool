using ProjectManagementTool.Application.DTOs.Tag;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface ITagService
    {
        Task<TagDto> GetOrCreateIfNotExists(string tagName);
        Task<ICollection<TagDto>> GetAllTagsAsync();
    }
}