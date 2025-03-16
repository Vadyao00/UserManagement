using Contracts.IRepositories;
using Domain.Entities;
using Domain.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Persistance.Repositories;

public class UserRepository(UserManagementContext dbContext) : RepositoryBase<User>(dbContext), IUserRepository
{
    public async Task<PagedList<User>> GetAllUsersAsync(UserParameters userParameters, bool trackChanges)
    {
        var users = await FindAll(trackChanges)
            .Skip((userParameters.PageNumber - 1) * userParameters.PageSize)
            .Take(userParameters.PageSize)
            .ToListAsync();

        var count = await FindAll(trackChanges).CountAsync();

        return new PagedList<User>(users, count, userParameters.PageNumber, userParameters.PageSize);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var existingUser = await FindByCondition(u => u.Email == email && u.DeletedAt == null, false).FirstOrDefaultAsync();

        return existingUser;
    }

    public async Task<IEnumerable<User>> GetAllUsersWithoutMetaAsync(bool trackChanges)
    {
        var users = await FindAll(trackChanges)
            .ToListAsync();

        return users;
    }

    public void CreateUser(User user) => Create(user);

    public void DeleteUser(User user) => Delete(user);

    public void UpdateUser(User user) => Update(user);
}