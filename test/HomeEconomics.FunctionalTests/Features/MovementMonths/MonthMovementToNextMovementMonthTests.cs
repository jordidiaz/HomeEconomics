using Domain.MovementMonth;
using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.FunctionalTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HomeEconomics.FunctionalTests.Features.MovementMonths;

public class MonthMovementToNextMovementMonthTests : FunctionalTestBase
{
    private MonthMovementToNextMovementMonth.Command _command = default!;

    [Fact]
    public async Task Should_Pass_MonthMovementToNextMovementMonth_And_Return_Resume()
    {
        await CreateMovements();

        var movementMonthResponse = await CreateMovementMonth(Month.Feb);
        var nextMovementMonthResponse = await CreateMovementMonth(Month.Mar);

        movementMonthResponse.MonthMovements.Length.Should().Be(2);
        movementMonthResponse.MonthMovements.SingleOrDefault(mm => mm.Name == "Amazon").Should().NotBeNull();
        nextMovementMonthResponse.MonthMovements.Length.Should().Be(1);
        nextMovementMonthResponse.MonthMovements.SingleOrDefault(mm => mm.Name == "Amazon").Should().BeNull();

        _command = new MonthMovementToNextMovementMonth.Command(
            movementMonthResponse.Id,
            movementMonthResponse.MonthMovements.Single(mm => mm.Name == "Amazon").Id);

        movementMonthResponse = await Fixture.SendToMediatRAsync(_command);

        var nextMovementMonth = await Fixture.QueryDbContextAsync(async homeEconomicsDbContext =>
        {
            return await homeEconomicsDbContext
                .MovementMonths
                .Include("_monthMovements")
                .SingleOrDefaultAsync(mm => mm.Id == nextMovementMonthResponse.Id);
        });

        movementMonthResponse.MonthMovements.Length.Should().Be(1);
        movementMonthResponse.MonthMovements.SingleOrDefault(mm => mm.Name == "Amazon").Should().BeNull();
        nextMovementMonth!.GetMonthMovements().Count().Should().Be(2);
        nextMovementMonth.GetMonthMovements().SingleOrDefault(mm => mm.Name == "Amazon").Should().NotBeNull();
    }

    [Fact]
    public async Task Should_Throw_InvalidOperationException_If_MovementMonth_Not_Exists()
    {
        Func<Task> action = async () => await Fixture.SendToMediatRAsync(new MonthMovementToNextMovementMonth.Command(1, 1));

        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage(Properties.Messages.MovementMonthNotExists);
    }

    [Fact]
    public async Task Should_Throw_InvalidOperationException_If_MonthMovement_Not_Exists()
    {
        await CreateMovements();

        var movementMonthResponse = await CreateMovementMonth();

        Func<Task> action = async () => await Fixture.SendToMediatRAsync(new MonthMovementToNextMovementMonth.Command(movementMonthResponse.Id, 99));

        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage(Properties.Messages.MonthMovementNotExists);
    }

    [Fact]
    public async Task Should_Throw_InvalidOperationException_If_NextMovementMonth_Not_Exists()
    {
        await CreateMovements();

        var movementMonthResponse = await CreateMovementMonth();

        Func<Task> action = async () => await Fixture.SendToMediatRAsync(new MonthMovementToNextMovementMonth.Command(
            movementMonthResponse.Id,
            movementMonthResponse.MonthMovements.First().Id));

        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage(Properties.Messages.NextMovementMonthNotExists);
    }
}