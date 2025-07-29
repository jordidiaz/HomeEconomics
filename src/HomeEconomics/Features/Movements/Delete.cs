using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.Movements;

public class Delete
{
    public record Command(int Id) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(command => command.Id).GreaterThan(0);
        }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly HomeEconomicsDbContext _dbContext;

        public Handler(HomeEconomicsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var movement =
                await _dbContext.GetMovementAsync(m => m.Id == request.Id, cancellationToken: cancellationToken);

            if (movement is null)
            {
                throw new InvalidOperationException(Properties.Messages.MovementNotExists);
            }

            _dbContext.Movements.Remove(movement);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}