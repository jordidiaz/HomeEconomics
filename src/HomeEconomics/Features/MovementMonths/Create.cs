using Domain.MovementMonth;
using Domain.Movements;
using FluentValidation;
using HomeEconomics.Helpers;
using HomeEconomics.Services;
using JetBrains.Annotations;
using LiteBus.Commands.Abstractions;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.MovementMonths;

[UsedImplicitly]
public class Create
{
    public record Command(int Year, Month Month) : ICommand<MovementMonthResponse?>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(command => command.Year).GreaterThanOrEqualTo(DateTime.Now.Year);
            RuleFor(command => command.Month).Must(Enums.IsAValidEnumValue);
        }
    }

    [UsedImplicitly]
    public class Handler(HomeEconomicsDbContext dbContext, IMovementMonthResponseService movementMonthResponseService)
        : ICommandHandler<Command, MovementMonthResponse?>
    {
        public async Task<MovementMonthResponse?> HandleAsync(Command request, CancellationToken cancellationToken = default)
        {
            var movementMonthExists = await dbContext.ExistsMovementMonthAsync(
                mm => mm.Year == request.Year && mm.Month == request.Month, cancellationToken: cancellationToken);

            if (movementMonthExists)
            {
                throw new InvalidOperationException(Properties.Messages.MovementMonthExists);
            }

            var movements = dbContext
                .GetMovements()
                .AsEnumerable()
                .Where(m => UseMovement(m, request.Month))
                .ToArray();

            if (!movements.Any())
            {
                throw new InvalidOperationException(Properties.Messages.MovementsNotExists);
            }

            var movementMonth = new MovementMonth(request.Year, request.Month);
            movementMonth.AddStatus(0, 0, 0);

            foreach (var movement in movements)
            {
                movementMonth.AddMonthMovement(movement.Name, movement.Amount, movement.Type);
            }

            dbContext.MovementMonths.Add(movementMonth);

            await dbContext.SaveChangesAsync(cancellationToken);

            return await movementMonthResponseService.Get(mm => mm.Id == movementMonth.Id,
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
