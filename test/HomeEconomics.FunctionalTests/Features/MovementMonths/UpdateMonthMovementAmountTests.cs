using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.MovementMonth;
using Domain.Movements;
using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.FunctionalTests.Infrastructure;
using Xunit;
using Create = HomeEconomics.Features.Movements.Create;

namespace HomeEconomics.FunctionalTests.Features.MovementMonths
{
    public class UpdateMonthMovementAmountTests : FunctionalTestBase
    {
        private UpdateMonthMovementAmount.Command _command;

        [Fact]
        public async Task Should_Update_MonthMovement_Amount_Return_Resume()
        {
            await InsertMovements();

            var createMovementMonthResult = await Fixture.SendToMediatRAsync(
                new global::HomeEconomics.Features.MovementMonths.Create.Command
                {
                    Year = 2020,
                    Month = Month.Jan
                });

            _command = new UpdateMonthMovementAmount.Command
            {
                MovementMonthId = createMovementMonthResult.Id,
                MonthMovementId = createMovementMonthResult.MonthMovements.First().Id,
                Amount = 70m
            };

            var result = await Fixture.SendToMediatRAsync(_command);

            result.PendingTotalExpenses.Should().Be(100m);
            result.PendingTotalIncomes.Should().Be(940m);
        }

        [Fact]
        public void Should_Throw_InvalidOperationException_If_MovementMonth_Not_Exists()
        {
            _command = new UpdateMonthMovementAmount.Command
            {
                MovementMonthId = 0,
                MonthMovementId = 0,
                Amount = 70m
            };

            Func<Task> action = async () => await Fixture.SendToMediatRAsync(_command);

            action.Should().Throw<InvalidOperationException>().WithMessage(Properties.Messages.MovementMonthNotExists);
        }

        private static async Task InsertMovements()
        {
            await Fixture.SendToMediatRAsync(new Create.Command
            {
                Name = "1",
                Amount = 60m,
                Type = MovementType.Expense,
                Frequency = new Create.Frequency
                {
                    Type = FrequencyType.Monthly
                }
            });

            await Fixture.SendToMediatRAsync(new Create.Command
            {
                Name = "2",
                Amount = 30m,
                Type = MovementType.Expense,
                Frequency = new Create.Frequency
                {
                    Type = FrequencyType.Monthly
                }
            });

            await Fixture.SendToMediatRAsync(new Create.Command
            {
                Name = "3",
                Amount = 930m,
                Type = MovementType.Income,
                Frequency = new Create.Frequency
                {
                    Type = FrequencyType.Monthly
                }
            });

            await Fixture.SendToMediatRAsync(new Create.Command
            {
                Name = "4",
                Amount = 10m,
                Type = MovementType.Income,
                Frequency = new Create.Frequency
                {
                    Type = FrequencyType.Monthly
                }
            });
        }
    }
}
