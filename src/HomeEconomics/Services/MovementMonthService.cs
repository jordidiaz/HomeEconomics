using AutoMapper;
using Domain.MovementMonth;
using HomeEconomics.Features.MovementMonths;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace HomeEconomics.Services
{
    public class MovementMonthService : IMovementMonthService
    {
        private readonly HomeEconomicsDbContext _dbContext;
        private readonly IMapper _mapper;

        public MovementMonthService(HomeEconomicsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<MovementMonthResponse?> GetMovementMonthResponseAsync(Expression<Func<MovementMonth, bool>> predicate, CancellationToken cancellationToken)
        {
            var movementMonth = await GetMovementMonthAsync(predicate, cancellationToken);
            if (movementMonth is null)
            {
                return null;
            }

            var nextMovementMonth = await GetNextMovementMonthAsync(movementMonth, cancellationToken);

            var movementMonthResponse = _mapper.Map<MovementMonthResponse>(movementMonth);
            movementMonthResponse.NextMovementMonthExists = nextMovementMonth != null;

            return movementMonthResponse;
        }

        public async Task<MovementMonth?> GetMovementMonthAsync(Expression<Func<MovementMonth, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbContext.MovementMonths
                .Include("_monthMovements")
                .Include("_statuses")
                .SingleOrDefaultAsync(predicate,
                    cancellationToken: cancellationToken);
        }

        public async Task<MovementMonth?> GetNextMovementMonthAsync(MovementMonth movementMonth, CancellationToken cancellationToken)
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

            var nextMovementMonth = await GetMovementMonthAsync(mm => mm.Year == nextYear && mm.Month == nextMonth,
                cancellationToken: cancellationToken);

            return nextMovementMonth;
        }

        public async Task<MovementMonthResponse> MapToMovementMonthResponseAsync(MovementMonth movementMonth, CancellationToken cancellationToken)
        {
            var movementMonthResponse = _mapper.Map<MovementMonthResponse>(movementMonth);

            var nextMovementMonth = await GetNextMovementMonthAsync(movementMonth, cancellationToken);

            movementMonthResponse.NextMovementMonthExists = nextMovementMonth != null;

            return movementMonthResponse;

        }
    }
}
