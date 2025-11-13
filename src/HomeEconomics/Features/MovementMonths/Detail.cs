using Domain.MovementMonth;
using HomeEconomics.Services;
using JetBrains.Annotations;
using MediatR;

namespace HomeEconomics.Features.MovementMonths;

[UsedImplicitly]
public class Detail
{
    public record Query(int Year, int Month) : IRequest<MovementMonthResponse?>;

    public class Handler(IMovementMonthResponseService movementMonthResponseService)
        : IRequestHandler<Query, MovementMonthResponse?>
    {
        public async Task<MovementMonthResponse?> Handle(Query request, CancellationToken cancellationToken) =>
            await movementMonthResponseService.Get(
                mm => mm.Year == request.Year && mm.Month == (Month)request.Month,
                cancellationToken: cancellationToken);
    }
}
