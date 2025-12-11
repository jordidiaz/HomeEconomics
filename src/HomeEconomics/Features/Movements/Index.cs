using JetBrains.Annotations;
using LiteBus.Queries.Abstractions;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.Movements;

[UsedImplicitly]
public class Index
{
    public record Query : IQuery<Result>;

    public record Result(Movement[] Movements);
    
    [UsedImplicitly]
    public record Movement(
        int Id, 
        string Name, 
        decimal Amount, 
        int Type, 
        int FrequencyType, 
        bool[] FrequencyMonths, 
        int FrequencyMonth);
        
    public static Movement FromMovement(Domain.Movements.Movement movement) =>
        new(movement.Id,
            movement.Name,
            movement.Amount,
            (int)movement.Type,
            (int)movement.GetFrequencyType(),
            movement.GetMonths().ToArray(),
            movement.GetFrequencyType() == Domain.Movements.FrequencyType.Yearly
                ? Array.IndexOf(movement.GetMonths(), true) + 1
                : 0);

    public class Handler(HomeEconomicsDbContext dbContext) : IQueryHandler<Query, Result>
    {
        public Task<Result> HandleAsync(Query request, CancellationToken cancellationToken = default)
        {
            var movements = dbContext.GetMovements()
                .OrderBy(m => m.Name);

            return Task.FromResult(new Result(movements
                .Select(FromMovement)
                .ToArray()));
        }
    }
}
