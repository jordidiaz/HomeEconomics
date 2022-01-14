using HomeEconomics.Services;
using MediatR;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HomeEconomics.Features.MovementMonths
{
    public class MonthMovementToNextMovementMonth
    {
        public record Command(int MovementMonthId, int MonthMovementId) : IRequest<MovementMonthResponse>;

        public class Handler : IRequestHandler<Command, MovementMonthResponse>
        {
            private readonly IMovementMonthService _movementMonthService;
            private readonly HomeEconomicsDbContext _dbContext;

            public Handler(HomeEconomicsDbContext dbContext, IMovementMonthService movementMonthService)
            {
                _dbContext = dbContext;
                _movementMonthService = movementMonthService;
            }

            public async Task<MovementMonthResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                var movementMonth =
                    await _movementMonthService.GetMovementMonthAsync(mm => mm.Id == request.MovementMonthId,
                        cancellationToken: cancellationToken);

                if (movementMonth is null)
                {
                    throw new InvalidOperationException(Properties.Messages.MovementMonthNotExists);
                }

                var monthMovement = movementMonth.GetMonthMovement(request.MonthMovementId);
                if (monthMovement is null)
                {
                    throw new InvalidOperationException(Properties.Messages.MonthMovementNotExists);
                }

                var nextMovementMonth =
                    await _movementMonthService.GetNextMovementMonthAsync(movementMonth, cancellationToken);
                if (nextMovementMonth is null)
                {
                    throw new InvalidOperationException(Properties.Messages.NextMovementMonthNotExists);
                }

                movementMonth.DeleteMonthMovement(request.MonthMovementId);
                nextMovementMonth.AddMonthMovement(monthMovement.Name, monthMovement.Amount, monthMovement.Type);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return await _movementMonthService.MapToMovementMonthResponseAsync(movementMonth, cancellationToken);
            }
        }
    }
}
