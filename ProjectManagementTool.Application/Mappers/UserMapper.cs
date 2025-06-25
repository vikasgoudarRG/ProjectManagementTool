using ProjectManagementTool.Application.DTOs.User;
using ProjectManagementTool.Application.Interfaces.Mappers;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Mappers
{
    public class UserMapper : IUserMapper
    {
        public UserDTO ToDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}