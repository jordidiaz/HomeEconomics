using Domain.MovementMonth;
using FluentValidation;
using HomeEconomics.Helpers;
using HomeEconomics.Services;
using MediatR;
using Domain.Movements;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.MovementMonths;

[UsedImplicitly]
public class AddStatus
{
    public record Command(int Year, Month Month, decimal AccountAmount, decimal CashAmount) : IRequest<MovementMonthResponse>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(command => command.Year).GreaterThanOrEqualTo(DateTime.Now.Year);
            RuleFor(command => command.Month).Must(Enums.IsAValidEnumValue);
            RuleFor(command => command.AccountAmount).GreaterThanOrEqualTo(Movement.MinAmount);
            RuleFor(command => command.CashAmount).GreaterThanOrEqualTo(Movement.MinAmount);
        }
    }

    public class Handler(IMovementMonthResponseService movementMonthResponseService, HomeEconomicsDbContext dbContext)
        : IRequestHandler<Command, MovementMonthResponse>
    {
        public async Task<MovementMonthResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var movementMonth = await dbContext.GetMovementMonthAsync(mm => mm.Year == request.Year && mm.Month == request.Month, cancellationToken: cancellationToken);

            if (movementMonth is null)
            {
                throw new InvalidOperationException(Properties.Messages.MovementMonthNotExists);
            }

            var day = IsCurrentMonth(request.Year, (int)request.Month)
                ? DateTime.Now.Day
                : 0;

            movementMonth.AddStatus(day, request.AccountAmount, request.CashAmount);

            await dbContext.SaveChangesAsync(cancellationToken);

            return await movementMonthResponseService.Get(movementMonth, cancellationToken);
        }

        private static bool IsCurrentMonth(int year, int month) => DateTime.Now.Year == year && DateTime.Now.Month == month;
    }
}
