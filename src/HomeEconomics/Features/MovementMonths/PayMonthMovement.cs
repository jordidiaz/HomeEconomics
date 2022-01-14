using HomeEconomics.Services;
using MediatR;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HomeEconomics.Features.MovementMonths
{
    public class PayMonthMovement
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

                movementMonth.PayMonthMovement(request.MonthMovementId);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return await _movementMonthService.MapToMovementMonthResponseAsync(movementMonth, cancellationToken);
            }
        }
    }
}
