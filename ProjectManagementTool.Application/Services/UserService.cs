using ProjectManagementTool.Application.Interfaces.Common;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Domain.Entities;
using ProjectManageMentTool.Application.Interfaces.Repositories;

namespace ProjectManagementTool.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            IUserRepository userRepository,
            IProjectRepository projectRepository,
            ITeamRepository teamRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(User user)
        {
            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<IEnumerable<User>> SearchAsync(string keyword)
        {
            return await _userRepository.SearchAsync(keyword);
        }

        public async Task<IEnumerable<User>> GetAllInProjectAsync(Guid projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                return Enumerable.Empty<User>();

            return project.Developers;
        }

        public async Task<IEnumerable<User>> GetAllInTeamAsync(Guid teamId)
        {
            var teamMembers = await _teamRepository.GetAllMembersAsync(teamId);
            return teamMembers.Select(tm => tm.User);
        }

        public async Task<bool> IsUserInProjectAsync(Guid userId, Guid projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                return false;

            return project.Developers.Any(u => u.Id == userId);
        }

        public async Task<bool> IsUserInTeamAsync(Guid userId, Guid teamId)
        {
            var teamMember = await _teamRepository.GetMemberAsync(teamId, userId);
            return teamMember != null;
        }

        public async Task UpdateAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            await _userRepository.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}