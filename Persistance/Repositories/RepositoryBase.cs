using System.Linq.Expressions;
using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories;

public class RepositoryBase<T>(UserManagementContext dbContext) : IRepositoryBase<T> where T : class
{
    protected UserManagementContext DbContext = dbContext;

    public void Create(T entity) => DbContext.Set<T>().Add(entity);

    public void Delete(T entity) => DbContext.Set<T>().Remove(entity);
    public void Attach(T entity) => DbContext.Set<T>().Attach(entity);

    public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges ?
            DbContext.Set<T>().AsNoTracking() :
            DbContext.Set<T>();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
        !trackChanges ?
            DbContext.Set<T>().Where(expression).AsNoTracking() :
            DbContext.Set<T>().Where(expression);

    public void Update(T entity) => DbContext.Set<T>().Update(entity);
}