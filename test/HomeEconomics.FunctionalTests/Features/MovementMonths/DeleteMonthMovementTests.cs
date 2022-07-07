using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.FunctionalTests.Infrastructure;
using Xunit;

namespace HomeEconomics.FunctionalTests.Features.MovementMonths
{
    public class DeleteMonthMovementTests : FunctionalTestBase
    {
        private DeleteMonthMovement.Command _command = default!;

        [Fact]
        public async Task Should_Add_MonthMovement_And_Return_Resume()
        {
            await CreateMovements();

            var movementMonth = await CreateMovementMonth();

            _command = new DeleteMonthMovement.Command(movementMonth.Id, movementMonth.MonthMovements.First().Id);

            var result = await Fixture.SendToMediatRAsync(_command);

            result.Status.PendingTotalExpenses.Should().Be(60m);
            result.Status.PendingTotalIncomes.Should().Be(70m);
            result.MonthMovements.Length.Should().Be(2);
        }

        [Fact]
        public void Should_Throw_InvalidOperationException_If_MovementMonth_Not_Exists()
        {
            _command = new DeleteMonthMovement.Command(0, 0);

            Func<Task> action = async () => await Fixture.SendToMediatRAsync(_command);

            action.Should().Throw<InvalidOperationException>().WithMessage(Properties.Messages.MovementMonthNotExists);
        }
    }
}
