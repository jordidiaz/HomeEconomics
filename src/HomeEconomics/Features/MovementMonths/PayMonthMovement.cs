using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Movements;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.MovementMonths
{
    public class PayMonthMovement
    {
        public class Command : IRequest<Result>
        {
            public int MovementMonthId { get; set; }
            public int MonthMovementId { get; set; }
        }

        public class Result : PayMonthMovementResponse
        {
            
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly HomeEconomicsDbContext _dbContext;

            public Handler(HomeEconomicsDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var movementMonth = await _dbContext
                    .MovementMonths
                    .Include(mm => mm.MonthMovements)
                    .SingleOrDefaultAsync(mm => mm.Id == request.MovementMonthId, cancellationToken: cancellationToken);

                if (movementMonth is null)
                {
                    throw new InvalidOperationException(Properties.Messages.MovementMonthNotExists);
                }

                movementMonth.PayMonthMovement(request.MonthMovementId);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new Result
                {
                    PendingTotalExpenses = movementMonth.MonthMovements.Where(mm => mm.Type == MovementType.Expense && !mm.Paid).Sum(mm => mm.Amount),
                    PendingTotalIncomes = movementMonth.MonthMovements.Where(mm => mm.Type == MovementType.Income && !mm.Paid).Sum(mm => mm.Amount)
                };
            }
        }
    }
}
