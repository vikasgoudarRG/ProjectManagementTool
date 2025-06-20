using ProjectManagementTool.Application.DTOs.Tag;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Mappers
{
    public static class TagMapper
    {
        public static TagDto ToDto(Tag tag)
        {
            return new TagDto
            {
                Id = tag.Id,
                Name = tag.Name
            };
        }
    }
}