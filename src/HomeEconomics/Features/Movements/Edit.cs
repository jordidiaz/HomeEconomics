using Domain.Movements;
using JetBrains.Annotations;
using LiteBus.Commands.Abstractions;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.Movements;

[UsedImplicitly]
public class Edit
{
    public record Command(int Id, string Name, decimal Amount, MovementType Type, Frequency Frequency) : ICommand;

    public record Frequency(FrequencyType Type, int Month, bool[] Months);

    [UsedImplicitly]
    public class Validator : Create.Validator;

    public class Handler(HomeEconomicsDbContext dbContext) : ICommandHandler<Command>
    {
        public async Task HandleAsync(Command request, CancellationToken cancellationToken = default)
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
