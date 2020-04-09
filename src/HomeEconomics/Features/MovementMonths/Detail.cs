using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.MovementMonth;
using Microsoft.EntityFrameworkCore;
using Persistence;

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
            private readonly HomeEconomicsDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(HomeEconomicsDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<MovementMonthResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                var movementMonth = await _dbContext.MovementMonths
                    .Include(mm => mm.MonthMovements)
                    .Include(mm => mm.Statuses)
                    .SingleOrDefaultAsync(mm => mm.Year == request.Year && mm.Month == (Month) request.Month, cancellationToken: cancellationToken);

                return _mapper.Map<MovementMonthResponse>(movementMonth);
            }
        }
    }
}
