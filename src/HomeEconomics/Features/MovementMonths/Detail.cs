using Domain.MovementMonth;
using HomeEconomics.Services;
using MediatR;

namespace HomeEconomics.Features.MovementMonths;

public class Detail
{
    public record Query(int Year, int Month) : IRequest<MovementMonthResponse>;

    public class Handler : IRequestHandler<Query, MovementMonthResponse?>
    {
        private readonly IMovementMonthResponseService _movementMonthResponseService;

        public Handler(IMovementMonthResponseService movementMonthResponseService)
        {
            _movementMonthResponseService = movementMonthResponseService;
        }

        public async Task<MovementMonthResponse?> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _movementMonthResponseService.Get(
                mm => mm.Year == request.Year && mm.Month == (Month)request.Month,
                cancellationToken: cancellationToken);
        }
    }
}