using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Domain.Entities.ChangeLogs;
using ProjectManagementTool.Domain.Enums.ChangeLog;
using ProjectManagementTool.Domain.Interfaces.Repositories;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repositories
{
    public class TeamChangeLogRepository : ITeamChangeLogRepository
    {
        private readonly AppDbContext _context;

        public TeamChangeLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TeamChangeLog log)
        {
            await _context.TeamChangeLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<TeamChangeLog?> GetByIdAsync(Guid logId)
        {
            return await _context.TeamChangeLogs
                .Include(l => l.ChangedByUser)
                .FirstOrDefaultAsync(l => l.Id == logId);
        }

        public async Task<IEnumerable<TeamChangeLog>> GetAllByTeamIdAsync(
            Guid teamId,
            DateTime? from = null,
            DateTime? to = null,
            ChangeType? type = null)
        {
            var query = _context.TeamChangeLogs
                .Include(l => l.ChangedByUser)
                .Where(l => l.TeamId == teamId)
                .AsQueryable();

            if (from.HasValue)
                query = query.Where(l => l.CreatedOn >= from.Value);

            if (to.HasValue)
                query = query.Where(l => l.CreatedOn <= to.Value);

            if (type.HasValue)
                query = query.Where(l => l.ChangeType == type.Value);

            return await query.ToListAsync();
        }
    }
}
