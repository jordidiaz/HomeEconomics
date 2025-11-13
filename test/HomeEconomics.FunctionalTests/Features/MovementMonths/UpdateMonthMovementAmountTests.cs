using Domain.MovementMonth;
using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.FunctionalTests.Infrastructure;
using Xunit;

namespace HomeEconomics.FunctionalTests.Features.MovementMonths;

public class UpdateMonthMovementAmountTests : FunctionalTestBase
{
    private UpdateMonthMovementAmount.Command _command = null!;

    [Fact]
    public async Task Should_Update_MonthMovement_Amount_Return_Resume()
    {
        await CreateMovements();

        var createMovementMonthResult = await Fixture.SendToMediatRAsync(
            new Create.Command(
                DateTime.Now.Year,
                Month.Jan));

        _command = new UpdateMonthMovementAmount.Command(
            createMovementMonthResult.Id,
            createMovementMonthResult.MonthMovements.First().Id,
            70m);

        var result = await Fixture.SendToMediatRAsync(_command);

        result.Status.PendingTotalExpenses.Should().Be(130m);
        result.Status.PendingTotalIncomes.Should().Be(70m);
    }

    [Fact]
    public async Task Should_Throw_InvalidOperationException_If_MovementMonth_Not_Exists()
    {
        _command = new UpdateMonthMovementAmount.Command(
            0,
            0,
            70m);

        Func<Task> action = async () => await Fixture.SendToMediatRAsync(_command);

        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage(Properties.Messages.MovementMonthNotExists);
    }
}