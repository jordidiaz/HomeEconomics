using HomeEconomics.Services;
using JetBrains.Annotations;
using LiteBus.Commands.Abstractions;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.MovementMonths;

[UsedImplicitly]
public class UnStarMonthMovement
{
    public record Command(int MovementMonthId, int MonthMovementId) : ICommand<MovementMonthResponse>;

    [UsedImplicitly]
    public class Handler(IMovementMonthResponseService movementMonthResponseService, HomeEconomicsDbContext dbContext)
        : ICommandHandler<Command, MovementMonthResponse>
    {
        public async Task<MovementMonthResponse> HandleAsync(Command request, CancellationToken cancellationToken = default)
        {
            var movementMonth =
                await dbContext.GetMovementMonthAsync(mm => mm.Id == request.MovementMonthId,
                    cancellationToken: cancellationToken);

            if (movementMonth is null)
            {
                throw new InvalidOperationException(Properties.Messages.MovementMonthNotExists);
            }

            movementMonth.UnStarMonthMovement(request.MonthMovementId);

            await dbContext.SaveChangesAsync(cancellationToken);

            return await movementMonthResponseService.Get(movementMonth, cancellationToken);
        }
    }
}
