using Domain.MovementMonth;
using HomeEconomics.Features.MovementMonths;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq.Expressions;

namespace HomeEconomics.Services;

public class MovementMonthResponseService(HomeEconomicsDbContext dbContext) : IMovementMonthResponseService
{
    public async Task<MovementMonthResponse?> Get(Expression<Func<MovementMonth, bool>> predicate, CancellationToken cancellationToken)
    {
        var movementMonth = await dbContext.GetMovementMonthAsync(predicate, cancellationToken);
        if (movementMonth is null)
        {
            return null;
        }

        return await Get(movementMonth, cancellationToken);
    }

    public async Task<MovementMonthResponse> Get(MovementMonth movementMonth, CancellationToken cancellationToken)
    {
        var nextMovementMonthExists = await GetNextMovementMonthExistsAsync(movementMonth, cancellationToken);

        var movementMonthResponse = MovementMonthResponse.FromMovementMonth(movementMonth);
        movementMonthResponse.NextMovementMonthExists = nextMovementMonthExists;

        return movementMonthResponse;
    }

    private async Task<bool> GetNextMovementMonthExistsAsync(MovementMonth movementMonth, CancellationToken cancellationToken)
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

        return await dbContext.ExistsMovementMonthAsync(mm => mm.Year == nextYear && mm.Month == nextMonth,
            cancellationToken: cancellationToken);
    }
}
