using System;
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
    public class AddMonthMovementTests : FunctionalTestBase
    {
        private AddMonthMovement.Command _command;

        [Fact]
        public async Task Should_Add_MonthMovement_And_Return_Resume()
        {
            var monthMovementId = await CreateMonthMovement();

            _command = new AddMonthMovement.Command
            {
                MovementMonthId = monthMovementId,
                Name = "new",
                Type = MovementType.Expense,
                Amount = 50
            };

            var result = await Fixture.SendToMediatRAsync(_command);

            result.PendingTotalExpenses.Should().Be(140m);
            result.PendingTotalIncomes.Should().Be(940m);
            result.MonthMovements.Length.Should().Be(5);
        }

        [Fact]
        public void Should_Throw_InvalidOperationException_If_MovementMonth_Not_Exists()
        {
            _command = new AddMonthMovement.Command
            {
                MovementMonthId = 0,
                Name = "new",
                Type = MovementType.Expense,
                Amount = 50
            };

            Func<Task> action = async () => await Fixture.SendToMediatRAsync(_command);

            action.Should().Throw<InvalidOperationException>().WithMessage(Properties.Messages.MovementMonthNotExists);
        }

        private async Task<int> CreateMonthMovement()
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

            var createMovementMonthResult = await Fixture.SendToMediatRAsync(new global::HomeEconomics.Features.MovementMonths.Create.Command
            {
                Year = 2020,
                Month = Month.Jan
            });

            return createMovementMonthResult.Id;
        }
    }
}
