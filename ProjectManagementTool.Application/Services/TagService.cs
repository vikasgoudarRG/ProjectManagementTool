using ProjectManagementTool.Application.DTOs.Tag;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Application.Mappers;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IEnumerable<TagDto>> CreateTagsIfNotExistAsync(IEnumerable<string> tagNames)
        {
            ICollection<TagDto> tagDtos = new List<TagDto>();
            foreach (string tagName in tagNames)
            {
                Tag? tag = await _tagRepository.GetByNameAsync(tagName);
                if (tag == null)
                {
                    tag = new Tag(tagName);
                    await _tagRepository.AddAsync(tag);
                }
                tagDtos.Add(TagMapper.ToDto(tag));
            }
            return tagDtos;
        }

        
    }
}