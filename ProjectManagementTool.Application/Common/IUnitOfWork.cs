namespace ProjectManagementTool.Application.Interfaces.Common
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}