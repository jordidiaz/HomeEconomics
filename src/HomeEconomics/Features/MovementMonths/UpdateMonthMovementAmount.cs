 using System;
 using System.Threading;
 using System.Threading.Tasks;
 using AutoMapper;
 using Domain;
 using FluentValidation;
 using MediatR;
 using Microsoft.EntityFrameworkCore;
 using Persistence;

 namespace HomeEconomics.Features.MovementMonths
{
    public class UpdateMonthMovementAmount
    {
        public class Command : IRequest<Result>
        {
            public int MovementMonthId { get; set; }
            public int MonthMovementId { get; set; }
            public decimal Amount { get; set; }
        }

        public class Result : MovementMonthResponse
        {

        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(command => command.Amount).GreaterThanOrEqualTo(Constants.MovementMinAmount);
            }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly HomeEconomicsDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(HomeEconomicsDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var movementMonth = await _dbContext
                    .MovementMonths
                    .Include(mm => mm.MonthMovements)
                    .SingleOrDefaultAsync(mm => mm.Id == request.MovementMonthId, cancellationToken: cancellationToken);

                if (movementMonth is null)
                {
                    throw new InvalidOperationException(Properties.Messages.MovementMonthNotExists);
                }

                movementMonth.UpdateMonthMovementAmount(request.MonthMovementId, request.Amount);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return _mapper.Map<Result>(movementMonth);
            }
        }
    }
}
