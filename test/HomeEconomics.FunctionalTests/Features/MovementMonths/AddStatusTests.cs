using Domain.MovementMonth;
using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.FunctionalTests.Infrastructure;
using Xunit;

namespace HomeEconomics.FunctionalTests.Features.MovementMonths;

public class AddStatusTests : FunctionalTestBase
{
    private AddStatus.Command _command = null!;

    [Fact]
    public async Task Should_Add_Status()
    {
        await CreateMovements();

        var movementMonth = await CreateMovementMonth();

        _command = new AddStatus.Command(
            movementMonth!.Year,
            (Month)movementMonth.Month,
            1000,
            50);

        var result = await Fixture.SendCommandToMediatorAsync(_command);

        result.Status.AccountAmount.Should().Be(1000);
        result.Status.CashAmount.Should().Be(50);
        result.Status.PendingTotalExpenses.Should().Be(120.00m);
        result.Status.PendingTotalIncomes.Should().Be(70.00m);
    }

    [Fact]
    public async Task Should_Throw_InvalidOperationException_If_MovementMonth_Not_Exists()
    {
        await CreateMovements();

        await CreateMovementMonth();

        _command = new AddStatus.Command(
            0,
            0,
            1000,
            50);

        Func<Task> action = async () => await Fixture.SendCommandToMediatorAsync(_command);

        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage(Properties.Messages.MovementMonthNotExists);
    }
}
