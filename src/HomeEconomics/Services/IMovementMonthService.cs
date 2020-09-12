using Domain.MovementMonth;
using HomeEconomics.Features.MovementMonths;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace HomeEconomics.Services
{
    public interface IMovementMonthService
    {
        Task<MovementMonthResponse> GetMovementMonthResponseAsync(Expression<Func<MovementMonth, bool>> predicate, CancellationToken cancellationToken);

        Task<MovementMonth> GetMovementMonthAsync(Expression<Func<MovementMonth, bool>> predicate, CancellationToken cancellationToken);

        Task<MovementMonth> GetNextMovementMonthAsync(MovementMonth movementMonth, CancellationToken cancellationToken);

        Task<MovementMonthResponse> MapToMovementMonthResponseAsync(MovementMonth movementMonth, CancellationToken cancellationToken);
    }
}
