namespace ProjectManageMentTool.Application.Interfaces.Common
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}