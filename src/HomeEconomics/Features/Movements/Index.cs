using JetBrains.Annotations;
using LiteBus.Queries.Abstractions;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.Movements;

[UsedImplicitly]
public class Index
{
    public record Query : IQuery<Result>;

    public record Result
    {
        public Movement[] Movements { get; init; } = [];

        public record Movement
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Global
            public int Id { get; init; }

            public string Name { get; private init; } = string.Empty;

            public decimal Amount { get; private init; }

            public int Type { get; private init; }

            public int FrequencyType { get; private init; }

            public bool[] FrequencyMonths { get; private init; } = new bool[12];

            // ReSharper disable once UnusedAutoPropertyAccessor.Global
            public int FrequencyMonth { get; init; }

            public static Movement FromMovement(Domain.Movements.Movement movement) =>
                new()
                {
                    Id = movement.Id,
                    Name = movement.Name,
                    Amount = movement.Amount,
                    Type = (int)movement.Type,
                    FrequencyType = (int)movement.GetFrequencyType(),
                    FrequencyMonths = movement.GetMonths().ToArray(),
                    FrequencyMonth = movement.GetFrequencyType() == Domain.Movements.FrequencyType.Yearly
                        ? Array.IndexOf(movement.GetMonths(), true) + 1
                        : 0
                };
        }
    }

    [UsedImplicitly]
    public class Handler(HomeEconomicsDbContext dbContext) : IQueryHandler<Query, Result>
    {
        public Task<Result> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            var movements = dbContext.GetMovements()
                .OrderBy(m => m.Name);

            return Task.FromResult(new Result
            {
                Movements = movements
                    .Select(Result.Movement.FromMovement)
                    .ToArray()
            });
        }
    }
}
