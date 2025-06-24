using ProjectManagementTool.Domain.Interfaces.Repositories.Common;
using ProjectManagementTool.Infrastructure.Contexts;
namespace ProjectManagementTool.Infrastructure.Repositories.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}


