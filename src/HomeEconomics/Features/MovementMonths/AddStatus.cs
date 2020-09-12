using Domain;
using Domain.MovementMonth;
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
    public class AddStatus
    {
        public class Command : IRequest<MovementMonthResponse>
        {
            public int Year { get; set; }

            public Month Month { get; set; }

            public decimal AccountAmount { get; set; }

            public decimal CashAmount { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(command => command.Year).GreaterThanOrEqualTo(DateTime.Now.Year);
                RuleFor(command => command.Month).Must(Enums.IsAValidEnumValue);
                RuleFor(command => command.AccountAmount).GreaterThanOrEqualTo(Constants.MinAmount);
                RuleFor(command => command.CashAmount).GreaterThanOrEqualTo(Constants.MinAmount);
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
                var movementMonth = await _movementMonthService.GetMovementMonthAsync(
                    mm => mm.Year == request.Year && mm.Month == request.Month, cancellationToken: cancellationToken);

                if (movementMonth is null)
                {
                    throw new InvalidOperationException(Properties.Messages.MovementMonthNotExists);
                }

                var day = IsCurrentMonth(request.Year, (int)request.Month)
                    ? DateTime.Now.Day
                    : 0;

                movementMonth.AddStatus(day, request.AccountAmount, request.CashAmount);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return await _movementMonthService.MapToMovementMonthResponseAsync(movementMonth, cancellationToken);
            }

            private static bool IsCurrentMonth(int year, int month)
            {
                return DateTime.Now.Year == year && DateTime.Now.Month == month;
            }
        }
    }
}
