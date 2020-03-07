using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Domain.Movements;
using FluentValidation;
using HomeEconomics.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.MovementMonths
{
    public class AddMonthMovement
    {

        public class Command : IRequest<Result>
        {
            public int MovementMonthId { get; set; }

            public string Name { get; set; }

            public decimal Amount { get; set; }

            public MovementType Type { get; set; }
        }

        public class Result : MovementMonthResponse
        {

        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(command => command.Name).NotNull().NotEmpty().MaximumLength(Lengths.Name);
                RuleFor(command => command.Amount).GreaterThan(0);
                RuleFor(command => command.Type).Must(Enums.IsAValidEnumValue);
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

                movementMonth.AddMonthMovement(request.Name, request.Amount, request.Type);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return _mapper.Map<Result>(movementMonth);
            }
        }
    }
}
