using Domain.Movements;
using FluentValidation;
using HomeEconomics.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.Movements;

public class Create
{
    public record Command(string Name, decimal Amount, MovementType Type, Frequency Frequency) : IRequest<int>;

    public record Frequency
    {
        public Frequency()
        {
            Months = new List<bool>().ToArray();
        }

        public FrequencyType Type { get; init; }

        public int Month { get; init; }

        public bool[] Months { get; init; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(command => command.Name).NotNull().NotEmpty().MaximumLength(Movement.MovementNameMaxLength);
            RuleFor(command => command.Amount).GreaterThanOrEqualTo(Movement.MinAmount);
            RuleFor(command => command.Type).Must(Enums.IsAValidEnumValue);
            RuleFor(command => command.Frequency).SetValidator(new FrequencyValidator());
        }
    }

    public class FrequencyValidator : AbstractValidator<Frequency>
    {
        public FrequencyValidator()
        {
            RuleFor(frequency => frequency.Type).Must(Enums.IsAValidEnumValue);
            When(frequency => frequency.Type == FrequencyType.Yearly,
                () =>
                {
                    RuleFor(frequency => frequency.Month).Must(month => month >= 1 && month <= 12);
                });
            When(frequency => frequency.Type == FrequencyType.Custom,
                () =>
                {
                    RuleFor(frequency => frequency.Months)
                        .Must(months => months.Length == 12 && months.Count(m => m) >= 2);
                });
        }
    }

    public class Handler : IRequestHandler<Command, int>
    {
        private readonly HomeEconomicsDbContext _dbContext;

        public Handler(HomeEconomicsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            var movement = await _dbContext.GetMovementAsync(m => m.Name == request.Name,
                cancellationToken: cancellationToken);

            if (movement != null)
            {
                throw new InvalidOperationException(
                    movement.Type == MovementType.Expense
                        ? Properties.Messages.ExpenseExists
                        : Properties.Messages.IncomeExists
                );
            }

            movement = new Movement(request.Name, request.Amount, request.Type);

            switch (request.Frequency.Type)
            {
                case FrequencyType.None:
                    break;
                case FrequencyType.Monthly:
                    movement.SetMonthlyFrequency();
                    break;
                case FrequencyType.Yearly:
                    movement.SetYearlyFrequency(request.Frequency.Month);
                    break;
                case FrequencyType.Custom:
                    movement.SetCustomFrequency(request.Frequency.Months);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(request));
            }

            _dbContext.Movements.Add(movement);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return movement.Id;
        }
    }
}