using Domain.MovementMonth;
using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.FunctionalTests.Infrastructure;
using Xunit;

namespace HomeEconomics.FunctionalTests.Features.MovementMonths;

public class DetailTests : FunctionalTestBase
{
    [Fact]
    public async Task Should_Return_MovementMonth_Detail_And_NextMovementMonthExists_False()
    {
        await CreateMovements();

        var movementMonth = await CreateMovementMonth();

        await AddStatus(movementMonth!.Year, movementMonth.Month, 1000, 50);
        await AddStatus(movementMonth.Year, movementMonth.Month, 900, 60);

        var result = await Fixture.SendQueryToMediatorAsync(new Detail.Query(movementMonth.Year, movementMonth.Month));

        result!.Status.PendingTotalExpenses.Should().Be(120m);
        result.Status.PendingTotalIncomes.Should().Be(70m);
        result.Status.AccountAmount.Should().Be(900);
        result.Status.CashAmount.Should().Be(60);
        result.NextMovementMonthExists.Should().BeFalse();
    }

    [Fact]
    public async Task Should_Return_MovementMonth_Detail_And_NextMovementMonthExists_True()
    {
        await CreateMovements();

        var movementMonth = await CreateMovementMonth();

        await CreateMovementMonth(Month.Feb);

        await AddStatus(movementMonth!.Year, movementMonth.Month, 1000, 50);
        await AddStatus(movementMonth.Year, movementMonth.Month, 900, 60);

        var result = await Fixture.SendQueryToMediatorAsync(new Detail.Query(movementMonth.Year, movementMonth.Month));

        result!.Status.PendingTotalExpenses.Should().Be(120m);
        result.Status.PendingTotalIncomes.Should().Be(70m);
        result.Status.AccountAmount.Should().Be(900);
        result.Status.CashAmount.Should().Be(60);
        result.NextMovementMonthExists.Should().BeTrue();
    }

    [Fact]
    public async Task Should_Return_Null_If_MovementMonth_Not_Exist()
    {
        var movementMonthDetail = await Fixture.SendQueryToMediatorAsync(new Detail.Query(2020, 8));

        movementMonthDetail.Should().BeNull();
    }
}
