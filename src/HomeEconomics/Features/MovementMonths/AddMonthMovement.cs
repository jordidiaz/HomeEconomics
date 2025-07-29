using Domain.Movements;
using FluentValidation;
using HomeEconomics.Helpers;
using HomeEconomics.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.MovementMonths;

public class AddMonthMovement
{

    public record Command(int MovementMonthId, string Name, decimal Amount, MovementType Type) : IRequest<MovementMonthResponse>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(command => command.Name).NotNull().NotEmpty().MaximumLength(Movement.MovementNameMaxLength);
            RuleFor(command => command.Amount).GreaterThanOrEqualTo(Movement.MinAmount);
            RuleFor(command => command.Type).Must(Enums.IsAValidEnumValue);
        }
    }

    public class Handler : IRequestHandler<Command, MovementMonthResponse>
    {
        private readonly IMovementMonthResponseService _movementMonthResponseService;
        private readonly HomeEconomicsDbContext _dbContext;

        public Handler(IMovementMonthResponseService movementMonthResponseService, HomeEconomicsDbContext dbContext)
        {
            _movementMonthResponseService = movementMonthResponseService;
            _dbContext = dbContext;
        }

        public async Task<MovementMonthResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var movementMonth = await _dbContext
                .GetMovementMonthAsync(mm => mm.Id == request.MovementMonthId,
                    cancellationToken: cancellationToken);

            if (movementMonth is null)
            {
                throw new InvalidOperationException(Properties.Messages.MovementMonthNotExists);
            }

            movementMonth.AddMonthMovement(request.Name, request.Amount, request.Type);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return await _movementMonthResponseService.Get(movementMonth, cancellationToken);
        }
    }
}