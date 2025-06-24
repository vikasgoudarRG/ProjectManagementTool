namespace ProjectManagementTool.Domain.Interfaces.Repositories.Common
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}