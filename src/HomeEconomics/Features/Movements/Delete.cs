using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.Movements;

[UsedImplicitly]
public class Delete
{
    public record Command(int Id) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator() => RuleFor(command => command.Id).GreaterThan(0);
    }

    public class Handler(HomeEconomicsDbContext dbContext) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var movement =
                await dbContext.GetMovementAsync(m => m.Id == request.Id, cancellationToken: cancellationToken);

            if (movement is null)
            {
                throw new InvalidOperationException(Properties.Messages.MovementNotExists);
            }

            dbContext.Movements.Remove(movement);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
