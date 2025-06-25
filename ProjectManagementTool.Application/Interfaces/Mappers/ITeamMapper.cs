using ProjectManagementTool.Application.DTOs.Team;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Mappers
{
    public interface ITeamMapper
    {
        public TeamDTO ToDTO(Team team);
    }
}