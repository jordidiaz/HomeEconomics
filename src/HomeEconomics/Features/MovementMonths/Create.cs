using Domain.MovementMonth;
using Domain.Movements;
using FluentValidation;
using HomeEconomics.Helpers;
using HomeEconomics.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HomeEconomics.Features.MovementMonths
{
    public class Create
    {
        public class Command : IRequest<MovementMonthResponse>
        {
            public int Year { get; set; }

            public Month Month { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(command => command.Year).GreaterThanOrEqualTo(DateTime.Now.Year);
                RuleFor(command => command.Month).Must(Enums.IsAValidEnumValue);
            }
        }

        public class Handler : IRequestHandler<Command, MovementMonthResponse?>
        {
            private readonly IMovementMonthService _movementMonthService;
            private readonly HomeEconomicsDbContext _dbContext;

            public Handler(HomeEconomicsDbContext dbContext, IMovementMonthService movementMonthService)
            {
                _dbContext = dbContext;
                _movementMonthService = movementMonthService;
            }

            public async Task<MovementMonthResponse?> Handle(Command request, CancellationToken cancellationToken)
            {
                var movementMonth = await _movementMonthService.GetMovementMonthAsync(
                    mm => mm.Year == request.Year && mm.Month == request.Month, cancellationToken: cancellationToken);

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

                return await _movementMonthService.GetMovementMonthResponseAsync(mm => mm.Id == movementMonth.Id,
                    cancellationToken: cancellationToken);
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
