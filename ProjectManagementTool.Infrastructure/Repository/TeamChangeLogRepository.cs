using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementToold.Infrastructure.Repository
{
    public class TeamChangeLogRepository : ITeamChangeLogRepository
    {
        #region Fields
        private readonly AppDbContext _context;
        #endregion Fields

        #region Constructors
        public TeamChangeLogRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "AppDbContext cannot be emprty");
        }
        #endregion Constructors

        #region Methods
        public async Task AddAsync(TeamChangeLog log)
        {
            if (log == null) throw new ArgumentNullException(nameof(log), "TeamChangeLog cannot be null");
            await _context.TeamChangeLogs.AddAsync(log);
        }

        public async Task<TeamChangeLog?> GetByIdAsync(Guid logId)
        {
            return await _context.TeamChangeLogs
                .Include(t => t.Team)
                .Include(t => t.ChangedByUser)
                .FirstOrDefaultAsync(t => t.Id == logId);
        }

        public async Task<IEnumerable<TeamChangeLog>> GetAllByTeamIdAsync(Guid teamId)
        {
            return await _context.TeamChangeLogs
                .Where(t => t.T == teamId)
                .Include(t => t.Team)
                .Include(t => t.ChangedByUser)
                .OrderByDescending(t => t.CreatedOn)
                .ToListAsync();
        }
        

    }
}