using Domain.Entities;
using Persistance.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Persistance.Extensions;

public static class RepositoryUserExtensions
{
    public static IQueryable<User> Sort(this IQueryable<User> users, string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return users.OrderBy(e => e.Name);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<User>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return users.OrderBy(e => e.Name);

        return users.OrderBy(orderQuery);
    }
}