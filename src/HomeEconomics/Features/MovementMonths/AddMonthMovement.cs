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
public class AddMonthMovement
{

    public record Command(int MovementMonthId, string Name, decimal Amount, MovementType Type) : ICommand<MovementMonthResponse>;

    [UsedImplicitly]
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(command => command.Name).NotNull().NotEmpty().MaximumLength(Movement.MovementNameMaxLength);
            RuleFor(command => command.Amount).GreaterThanOrEqualTo(Movement.MinAmount);
            RuleFor(command => command.Type).Must(Enums.IsAValidEnumValue);
        }
    }

    [UsedImplicitly]
    public class Handler(IMovementMonthResponseService movementMonthResponseService, HomeEconomicsDbContext dbContext)
        : ICommandHandler<Command, MovementMonthResponse>
    {
        public async Task<MovementMonthResponse> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            var movementMonth = await dbContext
                .GetMovementMonthAsync(mm => mm.Id == request.MovementMonthId,
                    cancellationToken: cancellationToken);

            if (movementMonth is null)
            {
                throw new InvalidOperationException(Properties.Messages.MovementMonthNotExists);
            }

            movementMonth.AddMonthMovement(request.Name, request.Amount, request.Type);

            await dbContext.SaveChangesAsync(cancellationToken);

            return await movementMonthResponseService.Get(movementMonth, cancellationToken);
        }
    }
}
