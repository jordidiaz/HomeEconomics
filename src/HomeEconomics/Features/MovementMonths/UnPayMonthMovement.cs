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
    public class UnPayMonthMovement
    {
        public class Command : PayMonthMovement.Command, IRequest<Result>
        {
            
        }

        public class Result : PayMonthMovement.Result
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

                movementMonth.UnPayMonthMovement(request.MonthMovementId);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new Result
                {
                    PendingTotalExpenses = movementMonth.MonthMovements.Where(mm => mm.Type == MovementType.Expense).Sum(mm => mm.Amount),
                    PendingTotalIncomes = movementMonth.MonthMovements.Where(mm => mm.Type == MovementType.Income).Sum(mm => mm.Amount)
                };
            }
        }
    }
}
