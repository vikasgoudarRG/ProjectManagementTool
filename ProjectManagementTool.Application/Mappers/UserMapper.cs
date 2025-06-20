using ProjectManagementTool.Application.DTOs.User;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }
        public static SimpleUserDto ToSimpleDto(User user)
        {
            return new SimpleUserDto
            {
                Id = user.Id,
                Username = user.Username
            };
        }
    }
}