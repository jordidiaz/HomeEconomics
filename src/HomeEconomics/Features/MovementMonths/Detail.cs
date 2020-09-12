using Domain.MovementMonth;
using HomeEconomics.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HomeEconomics.Features.MovementMonths
{
    public class Detail
    {
        public class Query : IRequest<MovementMonthResponse>
        {
            public int Year { get; set; }

            public int Month { get; set; }
        }

        public class Handler : IRequestHandler<Query, MovementMonthResponse>
        {
            private readonly IMovementMonthService _movementMonthService;

            public Handler(IMovementMonthService movementMonthService)
            {
                _movementMonthService = movementMonthService;
            }

            public async Task<MovementMonthResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _movementMonthService.GetMovementMonthResponseAsync(
                    mm => mm.Year == request.Year && mm.Month == (Month)request.Month,
                    cancellationToken: cancellationToken);
            }
        }
    }
}
