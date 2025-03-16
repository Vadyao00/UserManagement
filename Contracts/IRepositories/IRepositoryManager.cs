namespace Contracts.IRepositories;

public interface IRepositoryManager
{
    IUserRepository User { get; }
    Task SaveAsync();
}