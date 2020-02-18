using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.MovementMonth;
using Domain.Movements;
using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.FunctionalTests.Infrastructure;
using Xunit;
using CreateMovement = HomeEconomics.Features.Movements.Create;
using CreateMovementMonth = HomeEconomics.Features.MovementMonths.Create.Command;


namespace HomeEconomics.FunctionalTests.Features.MovementMonths
{
    public class UnPayMonthMovementTests
    {
        private UnPayMonthMovement.Command _command;

        [Fact]
        public async Task Should_Unpay_MonthMovement_And_Return_Resume()
        {
            await InsertMovements();

            var createMovementMonthResult = await Fixture.SendToMediatRAsync(new CreateMovementMonth
            {
                Year = 2020,
                Month = Month.Jan
            });

            await Fixture.SendToMediatRAsync(new PayMonthMovement.Command
            {
                MovementMonthId = createMovementMonthResult.Id,
                MonthMovementId = createMovementMonthResult.MonthMovements.First().Id
            });

            _command = new UnPayMonthMovement.Command
            {
                MovementMonthId = createMovementMonthResult.Id,
                MonthMovementId = createMovementMonthResult.MonthMovements.First().Id
            };

            var result = await Fixture.SendToMediatRAsync<UnPayMonthMovement.Result>(_command);

            result.PendingTotalExpenses.Should().Be(90m);
            result.PendingTotalIncomes.Should().Be(940m);
        }

        [Fact]
        public void Should_Throw_InvalidOperationException_If_MovementMonth_Not_Exists()
        {
            _command = new UnPayMonthMovement.Command
            {
                MovementMonthId = 0,
                MonthMovementId = 0
            };

            Func<Task> action = async () => await Fixture.SendToMediatRAsync<UnPayMonthMovement.Result>(_command);

            action.Should().Throw<InvalidOperationException>().WithMessage(Properties.Messages.MovementMonthNotExists);
        }

        private static async Task InsertMovements()
        {
            await Fixture.SendToMediatRAsync(new CreateMovement.Command
            {
                Name = "1",
                Amount = 60m,
                Type = MovementType.Expense,
                Frequency = new CreateMovement.Frequency
                {
                    Type = FrequencyType.Monthly
                }
            });

            await Fixture.SendToMediatRAsync(new CreateMovement.Command
            {
                Name = "2",
                Amount = 30m,
                Type = MovementType.Expense,
                Frequency = new CreateMovement.Frequency
                {
                    Type = FrequencyType.Monthly
                }
            });

            await Fixture.SendToMediatRAsync(new CreateMovement.Command
            {
                Name = "3",
                Amount = 930m,
                Type = MovementType.Income,
                Frequency = new CreateMovement.Frequency
                {
                    Type = FrequencyType.Monthly
                }
            });

            await Fixture.SendToMediatRAsync(new CreateMovement.Command
            {
                Name = "4",
                Amount = 10m,
                Type = MovementType.Income,
                Frequency = new CreateMovement.Frequency
                {
                    Type = FrequencyType.Monthly
                }
            });
        }
    }
}
