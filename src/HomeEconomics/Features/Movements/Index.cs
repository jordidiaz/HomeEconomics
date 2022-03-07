using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HomeEconomics.Features.Movements
{
    public class Index
    {
        public record Query : IRequest<Result>;

        public record Result
        {
            public Movement[] Movements { get; init; } = { };

            public record Movement
            {
                public int Id { get; init; }

                public string Name { get; init; } = string.Empty;

                public decimal Amount { get; init; }

                public int Type { get; init; }

                public int FrequencyType { get; init; }

                public bool[] FrequencyMonths { get; init; } = new bool[12];

                public int FrequencyMonth { get; init; }

                public static Movement FromMovement(Domain.Movements.Movement movement)
                {
                    return new Movement
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
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly HomeEconomicsDbContext _dbContext;

            public Handler(HomeEconomicsDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var movements = _dbContext.GetMovements()
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
}