using System;
using System.Threading.Tasks;
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
            await CreateMovements();

            var movementMonth = await CreateMovementMonth();

            _command = new AddMonthMovement.Command
            {
                MovementMonthId = movementMonth.Id,
                Name = "new",
                Type = MovementType.Expense,
                Amount = 50
            };

            var result = await Fixture.SendToMediatRAsync(_command);

            result.Status.PendingTotalExpenses.Should().Be(170m);
            result.Status.PendingTotalIncomes.Should().Be(70m);
            result.MonthMovements.Length.Should().Be(4);
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
    }
}
