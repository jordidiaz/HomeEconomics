using System.Linq.Expressions;
using Domain.MovementMonth;
using Domain.Movements;
using Persistence;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore;

public static class HomeEconomicsDbContextExtensions
{
    extension(HomeEconomicsDbContext dbContext)
    {
        public async Task<MovementMonth?> GetMovementMonthAsync(Expression<Func<MovementMonth, bool>> predicate,
            CancellationToken cancellationToken) =>
            await GetAsync(dbContext, predicate, cancellationToken, "_monthMovements", "_statuses");

        public async Task<bool> ExistsMovementMonthAsync(Expression<Func<MovementMonth, bool>> predicate,
            CancellationToken cancellationToken) =>
            await dbContext.MovementMonths.AnyAsync(predicate, cancellationToken);

        public async Task<Movement?> GetMovementAsync(Expression<Func<Movement, bool>> predicate,
            CancellationToken cancellationToken) =>
            await GetAsync(dbContext, predicate, cancellationToken, "Frequency");

        public IQueryable<Movement> GetMovements() => GetCollection<Movement>(dbContext, "Frequency").AsNoTracking();
    }

    private static async Task<T?> GetAsync<T>(
        DbContext dbContext, 
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken,
        params string[] includes) where T : class
    {
        var context = dbContext
            .Set<T>()
            .AsQueryable();
            
        foreach (var include in includes)
        {
            context = context.Include(include);
        }

        return await context.SingleOrDefaultAsync(predicate, cancellationToken);
    }
        
    private static IQueryable<T> GetCollection<T>(
        DbContext dbContext, 
        params string[] includes) where T : class
    {
        var queryable = dbContext
            .Set<T>()
            .AsQueryable();
            
        foreach (var include in includes)
        {
            queryable = queryable.Include(include);
        }

        return queryable;
    }
}
