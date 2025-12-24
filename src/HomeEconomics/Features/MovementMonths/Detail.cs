using Domain.MovementMonth;
using HomeEconomics.Services;
using JetBrains.Annotations;
using LiteBus.Queries.Abstractions;

namespace HomeEconomics.Features.MovementMonths;

[UsedImplicitly]
public class Detail
{
    public record Query(int Year, int Month) : IQuery<MovementMonthResponse?>;

    [UsedImplicitly]
    public class Handler(IMovementMonthResponseService movementMonthResponseService)
        : IQueryHandler<Query, MovementMonthResponse?>
    {
        public async Task<MovementMonthResponse?> HandleAsync(Query request, CancellationToken cancellationToken = default) =>
            await movementMonthResponseService.Get(
                mm => mm.Year == request.Year && mm.Month == (Month)request.Month,
                cancellationToken: cancellationToken);
    }
}
