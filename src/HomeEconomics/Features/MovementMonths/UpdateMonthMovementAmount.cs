using FluentValidation;
using HomeEconomics.Services;
using MediatR;
using Domain.Movements;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.MovementMonths;

public class UpdateMonthMovementAmount
{
    public record Command(int MovementMonthId, int MonthMovementId, decimal Amount) : IRequest<MovementMonthResponse>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator() => RuleFor(command => command.Amount).GreaterThanOrEqualTo(Movement.MinAmount);
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
            var movementMonth = await _dbContext.GetMovementMonthAsync(mm => mm.Id == request.MovementMonthId, cancellationToken: cancellationToken);

            if (movementMonth is null)
            {
                throw new InvalidOperationException(Properties.Messages.MovementMonthNotExists);
            }

            movementMonth.UpdateMonthMovementAmount(request.MonthMovementId, request.Amount);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return await _movementMonthResponseService.Get(movementMonth, cancellationToken);
        }
    }
}