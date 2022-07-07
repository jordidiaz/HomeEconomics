using HomeEconomics.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.MovementMonths
{
    public class PayMonthMovement
    {
        public record Command(int MovementMonthId, int MonthMovementId) : IRequest<MovementMonthResponse>;

        public class Handler : IRequestHandler<Command, MovementMonthResponse>
        {
            private readonly IMovementMonthResponseService _movementMonthResponseService;
            private readonly HomeEconomicsDbContext _dbContext;

            public Handler(IMovementMonthResponseService movementMonthResponseService, HomeEconomicsDbContext dbContext)
            {
                _movementMonthResponseService = movementMonthResponseService;
                _dbContext = dbContext;
            }

            public async Task<MovementMonthResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                var movementMonth =
                    await _dbContext.GetMovementMonthAsync(mm => mm.Id == request.MovementMonthId,
                        cancellationToken: cancellationToken);

                if (movementMonth is null)
                {
                    throw new InvalidOperationException(Properties.Messages.MovementMonthNotExists);
                }

                movementMonth.PayMonthMovement(request.MonthMovementId);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return await _movementMonthResponseService.Get(movementMonth, cancellationToken);
            }
        }
    }
}
