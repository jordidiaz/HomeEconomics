using HomeEconomics.Services;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.MovementMonth;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.MovementMonths
{
    public class MonthMovementToNextMovementMonth
    {
        public record Command(int MovementMonthId, int MonthMovementId) : IRequest<MovementMonthResponse>;

        public class Handler : IRequestHandler<Command, MovementMonthResponse>
        {
            private readonly IMovementMonthService _movementMonthService;
            private readonly HomeEconomicsDbContext _dbContext;

            public Handler(IMovementMonthService movementMonthService, HomeEconomicsDbContext dbContext)
            {
                _movementMonthService = movementMonthService;
                _dbContext = dbContext;
            }

            public async Task<MovementMonthResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                var movementMonth = await _dbContext
                    .GetMovementMonthAsync(mm => mm.Id == request.MovementMonthId,
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

                var (year, month) = GetNext(movementMonth);
                var nextMovementMonth = await _dbContext
                    .GetMovementMonthAsync(mm => mm.Year == year && mm.Month == month, cancellationToken);
                if (nextMovementMonth is null)
                {
                    throw new InvalidOperationException(Properties.Messages.NextMovementMonthNotExists);
                }

                movementMonth.DeleteMonthMovement(request.MonthMovementId);
                nextMovementMonth.AddMonthMovement(monthMovement.Name, monthMovement.Amount, monthMovement.Type);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return await _movementMonthService.MapToMovementMonthResponseAsync(movementMonth, cancellationToken);
            }

            private (int year, Month month) GetNext(MovementMonth movementMonth)
            {
                var year = movementMonth.Year;
                var month = movementMonth.Month;

                int nextYear;
                Month nextMonth;

                if (month == Month.Dec)
                {
                    nextYear = year + 1;
                    nextMonth = Month.Jan;
                }
                else
                {
                    nextYear = year;
                    nextMonth = (Month)((int)month + 1);
                }

                return (nextYear, nextMonth);
            }
        }
    }
}
