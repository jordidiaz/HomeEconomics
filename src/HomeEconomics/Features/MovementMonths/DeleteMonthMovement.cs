using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HomeEconomics.Features.MovementMonths
{
    public class DeleteMonthMovement
    {
        public class Command : IRequest<MovementMonthResponse>
        {
            public int MovementMonthId { get; set; }
            public int MonthMovementId { get; set; }
        }

        public class Handler : IRequestHandler<Command, MovementMonthResponse>
        {
            private readonly HomeEconomicsDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(HomeEconomicsDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<MovementMonthResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                var movementMonth = await _dbContext
                    .MovementMonths
                    .Include(mm => mm.MonthMovements)
                    .Include(mm => mm.Statuses)
                    .SingleOrDefaultAsync(mm => mm.Id == request.MovementMonthId, cancellationToken: cancellationToken);

                if (movementMonth is null)
                {
                    throw new InvalidOperationException(Properties.Messages.MovementMonthNotExists);
                }

                movementMonth.DeleteMonthMovement(request.MonthMovementId);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return _mapper.Map<MovementMonthResponse>(movementMonth);
            }
        }
    }
}
