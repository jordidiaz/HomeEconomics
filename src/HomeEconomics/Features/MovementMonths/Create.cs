using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.MovementMonth;
using Domain.Movements;
using FluentValidation;
using HomeEconomics.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.MovementMonths
{
    public class Create
    {
        public class Command : IRequest<Result>
        {
            public int Year { get; set; }

            public Month Month { get; set; }
        }

        public class Result : MovementMonthResponse
        {

        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(command => command.Year).GreaterThanOrEqualTo(DateTime.Now.Year);
                RuleFor(command => command.Month).Must(Enums.IsAValidEnumValue);
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
                    .SingleOrDefaultAsync(mm => mm.Year == request.Year && mm.Month == request.Month, cancellationToken: cancellationToken);

                if (movementMonth != null)
                {
                    throw new InvalidOperationException(Properties.Messages.MovementMonthExists);
                }

                var movements = _dbContext
                    .Movements
                    .Include(m => m.Frequency)
                    .AsEnumerable()
                    .Where(m => UseMovement(m, request.Month))
                    .ToArray();

                if (!movements.Any())
                {
                    throw new InvalidOperationException(Properties.Messages.MovementsNotExists);
                }

                movementMonth = new MovementMonth(request.Year, request.Month);
                movementMonth.AddStatus(0, 0, 0);

                foreach (var movement in movements)
                {
                    movementMonth.AddMonthMovement(movement.Name, movement.Amount, movement.Type);
                }

                _dbContext.MovementMonths.Add(movementMonth);

                await _dbContext.SaveChangesAsync(cancellationToken);

                var result = await _dbContext.MovementMonths
                    .Include(mm => mm.MonthMovements)
                    .Include(mm => mm.Statuses)
                    .SingleOrDefaultAsync(mm => mm.Id == movementMonth.Id, cancellationToken: cancellationToken);

                return _mapper.Map<Result>(result);
            }

            private static bool UseMovement(Movement movement, Month month)
            {
                if (movement.GetFrequencyType() == FrequencyType.None)
                {
                    return false;
                }

                return movement.GetFrequencyType() == FrequencyType.Monthly || movement.HasMonthInFrequency(month);
            }
        }
    }
}
