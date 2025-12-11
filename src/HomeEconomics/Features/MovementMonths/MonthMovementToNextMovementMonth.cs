using HomeEconomics.Services;
using Domain.MovementMonth;
using JetBrains.Annotations;
using LiteBus.Commands.Abstractions;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.MovementMonths;

[UsedImplicitly]
public class MonthMovementToNextMovementMonth
{
    public record Command(int MovementMonthId, int MonthMovementId) : ICommand<MovementMonthResponse>;

    public class Handler(IMovementMonthResponseService movementMonthResponseService, HomeEconomicsDbContext dbContext)
        : ICommandHandler<Command, MovementMonthResponse>
    {
        public async Task<MovementMonthResponse> HandleAsync(Command request, CancellationToken cancellationToken = default)
        {
            var movementMonth = await dbContext
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
            var nextMovementMonth = await dbContext
                .GetMovementMonthAsync(mm => mm.Year == year && mm.Month == month, cancellationToken);
            if (nextMovementMonth is null)
            {
                throw new InvalidOperationException(Properties.Messages.NextMovementMonthNotExists);
            }

            movementMonth.DeleteMonthMovement(request.MonthMovementId);
            nextMovementMonth.AddMonthMovement(monthMovement.Name, monthMovement.Amount, monthMovement.Type);

            await dbContext.SaveChangesAsync(cancellationToken);

            return await movementMonthResponseService.Get(movementMonth, cancellationToken);
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
