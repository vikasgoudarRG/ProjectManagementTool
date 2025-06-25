using ProjectManagementTool.Application.DTOs.User;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Mappers
{
    public interface IUserMapper
    {
        UserDTO ToDTO(User user);
    }
}