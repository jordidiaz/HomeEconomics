using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.FunctionalTests.Infrastructure;
using Xunit;

namespace HomeEconomics.FunctionalTests.Features.MovementMonths;

public class UnPayMonthMovementTests : FunctionalTestBase
{
    private UnPayMonthMovement.Command _command = null!;

    [Fact]
    public async Task Should_Unpay_MonthMovement_And_Return_Resume()
    {
        await CreateMovements();

        var movementMonth = await CreateMovementMonth();

        await AddStatus(movementMonth!.Year, movementMonth.Month, 1000, 50);

        await Fixture.SendCommandToMediatorAsync(new PayMonthMovement.Command(
            movementMonth.Id,
            movementMonth.MonthMovements.First().Id));

        _command = new UnPayMonthMovement.Command(
            movementMonth.Id,
            movementMonth.MonthMovements.First().Id);

        var result = await Fixture.SendCommandToMediatorAsync(_command);

        result.Status.PendingTotalExpenses.Should().Be(120m);
        result.Status.PendingTotalIncomes.Should().Be(70m);
        result.Status.AccountAmount.Should().Be(1000m);
        result.Status.CashAmount.Should().Be(50);
    }

    [Fact]
    public async Task Should_Throw_InvalidOperationException_If_MovementMonth_Not_Exists()
    {
        _command = new UnPayMonthMovement.Command(0, 0);

        Func<Task> action = async () => await Fixture.SendCommandToMediatorAsync(_command);

        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage(Properties.Messages.MovementMonthNotExists);
    }
}
