using FluentValidation;
using JetBrains.Annotations;
using LiteBus.Commands.Abstractions;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.Movements;

[UsedImplicitly]
public class Delete
{
    public record Command(int Id) : ICommand;

    [UsedImplicitly]
    public class Validator : AbstractValidator<Command>
    {
        public Validator() => RuleFor(command => command.Id).GreaterThan(0);
    }

    [UsedImplicitly]
    public class Handler(HomeEconomicsDbContext dbContext) : ICommandHandler<Command>
    {
        public async Task HandleAsync(Command request, CancellationToken cancellationToken)
        {
            var movement =
                await dbContext.GetMovementAsync(m => m.Id == request.Id, cancellationToken: cancellationToken);

            if (movement is null)
            {
                throw new InvalidOperationException(Properties.Messages.MovementNotExists);
            }

            dbContext.Movements.Remove(movement);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
