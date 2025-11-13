using HomeEconomics.Services;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.MovementMonths;

[UsedImplicitly]
public class DeleteMonthMovement
{
    public record Command(int MovementMonthId, int MonthMovementId) : IRequest<MovementMonthResponse>;

    public class Handler(IMovementMonthResponseService movementMonthResponseService, HomeEconomicsDbContext dbContext)
        : IRequestHandler<Command, MovementMonthResponse>
    {
        public async Task<MovementMonthResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var movementMonth = await dbContext
                .GetMovementMonthAsync(mm => mm.Id == request.MovementMonthId, cancellationToken: cancellationToken);

            if (movementMonth is null)
            {
                throw new InvalidOperationException(Properties.Messages.MovementMonthNotExists);
            }

            movementMonth.DeleteMonthMovement(request.MonthMovementId);

            await dbContext.SaveChangesAsync(cancellationToken);

            return await movementMonthResponseService.Get(movementMonth, cancellationToken);
        }
    }
}
