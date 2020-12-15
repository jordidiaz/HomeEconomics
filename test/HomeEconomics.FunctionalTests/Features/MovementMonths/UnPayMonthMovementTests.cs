using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.FunctionalTests.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HomeEconomics.FunctionalTests.Features.MovementMonths
{
    public class UnPayMonthMovementTests : FunctionalTestBase
    {
        private UnPayMonthMovement.Command _command;

        public UnPayMonthMovementTests()
        {
            _command = new UnPayMonthMovement.Command();
        }

        [Fact]
        public async Task Should_Unpay_MonthMovement_And_Return_Resume()
        {
            await CreateMovements();

            var movementMonth = await CreateMovementMonth();

            await AddStatus(movementMonth.Year, movementMonth.Month, 1000, 50);

            await Fixture.SendToMediatRAsync(new PayMonthMovement.Command
            {
                MovementMonthId = movementMonth.Id,
                MonthMovementId = movementMonth.MonthMovements.First().Id
            });

            _command = new UnPayMonthMovement.Command
            {
                MovementMonthId = movementMonth.Id,
                MonthMovementId = movementMonth.MonthMovements.First().Id
            };

            var result = await Fixture.SendToMediatRAsync(_command);

            result.Status.PendingTotalExpenses.Should().Be(120m);
            result.Status.PendingTotalIncomes.Should().Be(70m);
            result.Status.AccountAmount.Should().Be(1000m);
            result.Status.CashAmount.Should().Be(50);
        }

        [Fact]
        public void Should_Throw_InvalidOperationException_If_MovementMonth_Not_Exists()
        {
            _command = new UnPayMonthMovement.Command
            {
                MovementMonthId = 0,
                MonthMovementId = 0
            };

            Func<Task> action = async () => await Fixture.SendToMediatRAsync(_command);

            action.Should().Throw<InvalidOperationException>().WithMessage(Properties.Messages.MovementMonthNotExists);
        }
    }
}
