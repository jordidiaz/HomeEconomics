using Domain.MovementMonth;
using Domain.Movements;
using HomeEconomics.Features.MovementMonths;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq.Expressions;

namespace HomeEconomics.Services;

public class MovementMonthResponseService(HomeEconomicsDbContext dbContext) : IMovementMonthResponseService
{
    public async Task<MovementMonthResponse?> Get(Expression<Func<MovementMonth, bool>> predicate, CancellationToken cancellationToken)
    {
        var movementMonthResponse = await GetMovementMonthResponseAsync(predicate, cancellationToken);
        if (movementMonthResponse is null)
        {
            return null;
        }

        movementMonthResponse.NextMovementMonthExists = await GetNextMovementMonthExistsAsync(
            movementMonthResponse.Year,
            (Month)movementMonthResponse.Month,
            cancellationToken);

        movementMonthResponse.PreviousMovementMonthExists = await GetPreviousMovementMonthExistsAsync(
            movementMonthResponse.Year,
            (Month)movementMonthResponse.Month,
            cancellationToken);

        return movementMonthResponse;
    }

    public async Task<MovementMonthResponse> Get(MovementMonth movementMonth, CancellationToken cancellationToken)
    {
        var movementMonthResponse = await GetMovementMonthResponseAsync(mm => mm.Id == movementMonth.Id, cancellationToken)
            ?? throw new InvalidOperationException("Movement month response could not be built.");

        movementMonthResponse.NextMovementMonthExists = await GetNextMovementMonthExistsAsync(
            movementMonth.Year,
            movementMonth.Month,
            cancellationToken);

        movementMonthResponse.PreviousMovementMonthExists = await GetPreviousMovementMonthExistsAsync(
            movementMonth.Year,
            movementMonth.Month,
            cancellationToken);

        return movementMonthResponse;
    }

    private async Task<MovementMonthResponse?> GetMovementMonthResponseAsync(
        Expression<Func<MovementMonth, bool>> predicate,
        CancellationToken cancellationToken) =>
        await dbContext.MovementMonths
            .AsNoTracking()
            .Where(predicate)
            .Select(movementMonth => new MovementMonthResponse
            {
                Id = movementMonth.Id,
                Year = movementMonth.Year,
                Month = (int)movementMonth.Month,
                Status = new MovementMonthResponse.StatusResult
                {
                    AccountAmount = EF.Property<IEnumerable<Status>>(movementMonth, "_statuses")
                        .OrderByDescending(status => status.Day)
                        .Select(status => (decimal?)status.AccountAmount)
                        .FirstOrDefault() ?? 0m,
                    CashAmount = EF.Property<IEnumerable<Status>>(movementMonth, "_statuses")
                        .OrderByDescending(status => status.Day)
                        .Select(status => (decimal?)status.CashAmount)
                        .FirstOrDefault() ?? 0m,
                    PendingTotalExpenses = EF.Property<IEnumerable<MonthMovement>>(movementMonth, "_monthMovements")
                        .Where(monthMovement => monthMovement.Type == MovementType.Expense && !monthMovement.Paid)
                        .Select(monthMovement => (decimal?)monthMovement.Amount)
                        .Sum() ?? 0m,
                    PendingTotalIncomes = EF.Property<IEnumerable<MonthMovement>>(movementMonth, "_monthMovements")
                        .Where(monthMovement => monthMovement.Type == MovementType.Income && !monthMovement.Paid)
                        .Select(monthMovement => (decimal?)monthMovement.Amount)
                        .Sum() ?? 0m,
                },
                MonthMovements = EF.Property<IEnumerable<MonthMovement>>(movementMonth, "_monthMovements")
                    .OrderByDescending(monthMovement => monthMovement.Starred)
                    .ThenBy(monthMovement => monthMovement.Name)
                    .Select(monthMovement => new MovementMonthResponse.MonthMovementResult
                    {
                        Id = monthMovement.Id,
                        Name = monthMovement.Name,
                        Amount = monthMovement.Amount,
                        Type = (int)monthMovement.Type,
                        Paid = monthMovement.Paid,
                        Starred = monthMovement.Starred
                    })
                    .ToArray()
            })
            .AsSplitQuery()
            .SingleOrDefaultAsync(cancellationToken);

    private async Task<bool> GetNextMovementMonthExistsAsync(int year, Month month, CancellationToken cancellationToken)
    {
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

    private async Task<bool> GetPreviousMovementMonthExistsAsync(int year, Month month, CancellationToken cancellationToken)
    {
        int previousYear;
        Month previousMonth;

        if (month == Month.Jan)
        {
            previousYear = year - 1;
            previousMonth = Month.Dec;
        }
        else
        {
            previousYear = year;
            previousMonth = (Month)((int)month - 1);
        }

        return await dbContext.ExistsMovementMonthAsync(mm => mm.Year == previousYear && mm.Month == previousMonth,
            cancellationToken: cancellationToken);
    }
}
