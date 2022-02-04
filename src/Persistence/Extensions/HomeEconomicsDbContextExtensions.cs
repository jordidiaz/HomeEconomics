using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.MovementMonth;
using Domain.Movements;
using Persistence;

namespace Microsoft.EntityFrameworkCore
{
    public static class HomeEconomicsDbContextExtensions
    {
        public static async Task<MovementMonth?> GetMovementMonthAsync(
            this HomeEconomicsDbContext dbContext, 
            Expression<Func<MovementMonth, bool>> predicate,
            CancellationToken cancellationToken)
        {
            return await GetAsync(dbContext, predicate, cancellationToken, "_monthMovements", "_statuses");
        }
        
        public static async Task<Movement?> GetMovementAsync(
            this HomeEconomicsDbContext dbContext, 
            Expression<Func<Movement, bool>> predicate,
            CancellationToken cancellationToken)
        {
            return await GetAsync(dbContext, predicate, cancellationToken, "Frequency");
        }
        
        public static IEnumerable<Movement> GetMovements(this HomeEconomicsDbContext dbContext)
        {
            return GetCollection<Movement>(dbContext, "Frequency");
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
        
        private static IEnumerable<T> GetCollection<T>(
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
}