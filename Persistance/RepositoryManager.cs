using Contracts.IRepositories;
using Persistance.Repositories;

namespace Persistance;

public class RepositoryManager : IRepositoryManager
{
    private readonly UserManagementContext _userManagementContext;
    private readonly Lazy<IUserRepository> _userRepository;

    public RepositoryManager(UserManagementContext userManagementContext)
    {
        _userManagementContext = userManagementContext;
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(userManagementContext));
    }

    public IUserRepository User => _userRepository.Value;
    
    public async Task SaveAsync() => await _userManagementContext.SaveChangesAsync();
}