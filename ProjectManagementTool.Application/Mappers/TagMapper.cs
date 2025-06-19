using System.Security.Cryptography.X509Certificates;
using ProjectManagementTool.Application.DTOs.Tag;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Mappers
{
    public static class TagMapper
    {
        public static ICollection<TagDto> ToDtos(ICollection<Tag> tags)
        {
            ICollection<TagDto> tagDtos = new List<TagDto>();
            foreach (Tag tag in tags)
            {
                tagDtos.Add(ToDto(tag));
            }

            return tagDtos;
        }

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