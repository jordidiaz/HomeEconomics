using Domain.Movements;
using JetBrains.Annotations;
using LiteBus.Commands.Abstractions;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.Movements;

[UsedImplicitly]
public class Edit
{
    public record Command : ICommand
    {
        public int Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public decimal Amount { get; init; }

        public MovementType Type { get; init; }

        public Frequency Frequency { get; init; } = new();
    }

    public record Frequency
    {
        public FrequencyType Type { get; init; }

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public int Month { get; }

        public bool[] Months { get; init; } = new List<bool>().ToArray();
    }

    public class Validator : Create.Validator;

    public class Handler(HomeEconomicsDbContext dbContext) : ICommandHandler<Command>
    {
        public async Task HandleAsync(Command request, CancellationToken cancellationToken)
        {
            var movement =
                await dbContext.GetMovementAsync(m => m.Id == request.Id,
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

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
