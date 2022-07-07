using Domain.MovementMonth;
using HomeEconomics.Features.MovementMonths;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq.Expressions;

namespace HomeEconomics.Services
{
    public class MovementMonthResponseService : IMovementMonthResponseService
    {
        private readonly HomeEconomicsDbContext _dbContext;

        public MovementMonthResponseService(HomeEconomicsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MovementMonthResponse?> Get(Expression<Func<MovementMonth, bool>> predicate, CancellationToken cancellationToken)
        {
            var movementMonth = await _dbContext.GetMovementMonthAsync(predicate, cancellationToken);
            if (movementMonth is null)
            {
                return null;
            }

            return await Get(movementMonth, cancellationToken);
        }

        public async Task<MovementMonthResponse> Get(MovementMonth movementMonth, CancellationToken cancellationToken)
        {
            var nextMovementMonth = await GetNextMovementMonthAsync(movementMonth, cancellationToken);

            var movementMonthResponse = MovementMonthResponse.FromMovementMonth(movementMonth);
            movementMonthResponse.NextMovementMonthExists = nextMovementMonth != null;

            return movementMonthResponse;
        }

        private async Task<MovementMonth?> GetNextMovementMonthAsync(MovementMonth movementMonth, CancellationToken cancellationToken)
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

            var nextMovementMonth = await _dbContext.GetMovementMonthAsync(mm => mm.Year == nextYear && mm.Month == nextMonth,
                cancellationToken: cancellationToken);

            return nextMovementMonth;
        }
    }
}
