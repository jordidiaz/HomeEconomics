using Domain;
using Domain.Movements;
using FluentValidation;
using HomeEconomics.Helpers;
using HomeEconomics.Services;
using MediatR;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HomeEconomics.Features.MovementMonths
{
    public class AddMonthMovement
    {

        public class Command : IRequest<MovementMonthResponse>
        {
            public int MovementMonthId { get; set; }

            public string Name { get; set; }

            public decimal Amount { get; set; }

            public MovementType Type { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(command => command.Name).NotNull().NotEmpty().MaximumLength(Constants.MovementNameMaxLength);
                RuleFor(command => command.Amount).GreaterThanOrEqualTo(Constants.MinAmount);
                RuleFor(command => command.Type).Must(Enums.IsAValidEnumValue);
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
                var movementMonth = await _movementMonthService.GetMovementMonthAsync(mm => mm.Id == request.MovementMonthId,
                    cancellationToken: cancellationToken);

                if (movementMonth is null)
                {
                    throw new InvalidOperationException(Properties.Messages.MovementMonthNotExists);
                }

                movementMonth.AddMonthMovement(request.Name, request.Amount, request.Type);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return await _movementMonthService.MapToMovementMonthResponseAsync(movementMonth, cancellationToken);
            }
        }
    }
}
