using Domain.Movements;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.Movements;

public class Edit
{
    public record Command : IRequest<Unit>
    {
        public int Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public decimal Amount { get; init; }

        public MovementType Type { get; init; }

        public Frequency Frequency { get; init; } = new();
    }

    public record Frequency
    {
        public Frequency()
        {
            Months = new List<bool>().ToArray();
        }

        public FrequencyType Type { get; init; }

        public int Month { get; init; }

        public bool[] Months { get; init; }
    }

    public class Validator : Create.Validator
    {

    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly HomeEconomicsDbContext _dbContext;

        public Handler(HomeEconomicsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var movement =
                await _dbContext.GetMovementAsync(m => m.Id == request.Id,
                    cancellationToken: cancellationToken);

            if (movement is null)
            {
                throw new InvalidOperationException(Properties.Messages.MovementNotExists);
            }

            movement.SetName(request.Name);
            movement.SetAmount(request.Amount);
            movement.Type = request.Type;

            switch (request.Frequency.Type)
            {
                case FrequencyType.None:
                    movement.SetNoneFrequency();
                    break;
                case FrequencyType.Monthly:
                    movement.SetMonthlyFrequency();
                    break;
                case FrequencyType.Yearly:
                    movement.SetYearlyFrequency(request.Frequency.Month);
                    break;
                case FrequencyType.Custom:
                    movement.SetCustomFrequency(request.Frequency.Months);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(request));
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}