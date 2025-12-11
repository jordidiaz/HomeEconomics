using FluentValidation;
using HomeEconomics.Services;
using Domain.Movements;
using JetBrains.Annotations;
using LiteBus.Commands.Abstractions;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.MovementMonths;

[UsedImplicitly]
public class UpdateMonthMovementAmount
{
    public record Command(int MovementMonthId, int MonthMovementId, decimal Amount) : ICommand<MovementMonthResponse>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator() => RuleFor(command => command.Amount).GreaterThanOrEqualTo(Movement.MinAmount);
    }

    public class Handler(IMovementMonthResponseService movementMonthResponseService, HomeEconomicsDbContext dbContext)
        : ICommandHandler<Command, MovementMonthResponse>
    {
        public async Task<MovementMonthResponse> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            var movementMonth = await dbContext.GetMovementMonthAsync(mm => mm.Id == request.MovementMonthId, cancellationToken: cancellationToken);

            if (movementMonth is null)
            {
                throw new InvalidOperationException(Properties.Messages.MovementMonthNotExists);
            }

            movementMonth.UpdateMonthMovementAmount(request.MonthMovementId, request.Amount);

            await dbContext.SaveChangesAsync(cancellationToken);

            return await movementMonthResponseService.Get(movementMonth, cancellationToken);
        }
    }
}
