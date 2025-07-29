using Domain.MovementMonth;
using HomeEconomics.Features.MovementMonths;
using System.Linq.Expressions;

namespace HomeEconomics.Services;

public interface IMovementMonthResponseService
{
    Task<MovementMonthResponse?> Get(
        Expression<Func<MovementMonth, bool>> predicate,
        CancellationToken cancellationToken);
        
    Task<MovementMonthResponse> Get(
        MovementMonth movementMonth,
        CancellationToken cancellationToken);
}