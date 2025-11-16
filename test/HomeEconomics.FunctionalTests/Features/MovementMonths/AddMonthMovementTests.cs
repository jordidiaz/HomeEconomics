using Domain.Movements;
using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.FunctionalTests.Infrastructure;
using Xunit;

namespace HomeEconomics.FunctionalTests.Features.MovementMonths;

public class AddMonthMovementTests : FunctionalTestBase
{
    private AddMonthMovement.Command _command = null!;

    [Fact]
    public async Task Should_Add_MonthMovement_And_Return_Resume()
    {
        await CreateMovements();

        var movementMonth = await CreateMovementMonth();

        _command = new AddMonthMovement.Command(
            movementMonth!.Id,
            "new",
            50,
            MovementType.Expense);

        var result = await Fixture.SendCommandToMediatorAsync(_command);

        result.Status.PendingTotalExpenses.Should().Be(170m);
        result.Status.PendingTotalIncomes.Should().Be(70m);
        result.MonthMovements.Length.Should().Be(4);
    }

    [Fact]
    public async Task Should_Throw_InvalidOperationException_If_MovementMonth_Not_Exists()
    {
        _command = new AddMonthMovement.Command(
            0,
            "new",
            50,
            MovementType.Expense);

        Func<Task> action = async () => await Fixture.SendCommandToMediatorAsync(_command);

        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage(Properties.Messages.MovementMonthNotExists);
    }
}
