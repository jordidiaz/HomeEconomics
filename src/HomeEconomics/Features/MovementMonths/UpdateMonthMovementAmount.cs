using FluentValidation;
using HomeEconomics.Services;
using MediatR;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Movements;

namespace HomeEconomics.Features.MovementMonths
{
    public class UpdateMonthMovementAmount
    {
        public record Command(int MovementMonthId, int MonthMovementId, decimal Amount) : IRequest<MovementMonthResponse>;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(command => command.Amount).GreaterThanOrEqualTo(Movement.MinAmount);
            }
        }

        public class Handler : IRequestHandler<Command, MovementMonthResponse>
        {
            private readonly IMovementMonthService _movementMonthService;
            private readonly HomeEconomicsDbContext _dbContext;

            public Handler(HomeEconomicsDbContext dbContext, IMovementMonthService movementMonthService)
            {
                _dbContext = dbContext;
                _movementMonthService = movementMonthService;
            }

            public async Task<MovementMonthResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                var movementMonth = await _movementMonthService.GetMovementMonthAsync(mm => mm.Id == request.MovementMonthId, cancellationToken: cancellationToken);

                if (movementMonth is null)
                {
                    throw new InvalidOperationException(Properties.Messages.MovementMonthNotExists);
                }

                movementMonth.UpdateMonthMovementAmount(request.MonthMovementId, request.Amount);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return await _movementMonthService.MapToMovementMonthResponseAsync(movementMonth, cancellationToken);
            }
        }
    }
}
