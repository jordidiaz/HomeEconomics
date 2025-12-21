using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using System.Net;
using Domain.MovementMonth;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths;

public class DetailTests : IntegrationTestBase
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public DetailTests(Fixture fixture) : base(fixture)
    {
        
    }

    [Fact]
    public async Task Should_Return_404_NotFound()
    {
        var response = await HttpClient
            .GetAsync("api/movement-months/2019/2");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_Return_200_Ok_And_Return_MovementMonth_Detail_And_NextMovementMonthExists_False()
    {
        await CreateMovementsAsync();

        var movementMonth = await CreateMovementMonthAsync();

        await AddStatusAsync(movementMonth!.Year, movementMonth.Month, 1000, 50);
        await AddStatusAsync(movementMonth.Year, movementMonth.Month, 900, 60);

        var response = await HttpClient
            .GetAsync($"api/movement-months/{movementMonth.Year}/{movementMonth.Month}");

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<MovementMonthResponse>();

        result!.Status.PendingTotalExpenses.Should().Be(120m);
        result.Status.PendingTotalIncomes.Should().Be(70m);
        result.Status.AccountAmount.Should().Be(900);
        result.Status.CashAmount.Should().Be(60);
        result.NextMovementMonthExists.Should().BeFalse();
    }

    [Fact]
    public async Task Should_Return_MovementMonth_Detail_And_NextMovementMonthExists_True()
    {
        await CreateMovementsAsync();

        var movementMonth = await CreateMovementMonthAsync();

        await CreateMovementMonthAsync(Month.Feb);

        await AddStatusAsync(movementMonth!.Year, movementMonth.Month, 1000, 50);
        await AddStatusAsync(movementMonth.Year, movementMonth.Month, 900, 60);

        var response = await HttpClient
            .GetAsync($"api/movement-months/{movementMonth.Year}/{movementMonth.Month}");

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<MovementMonthResponse>();

        result!.Status.PendingTotalExpenses.Should().Be(120m);
        result.Status.PendingTotalIncomes.Should().Be(70m);
        result.Status.AccountAmount.Should().Be(900);
        result.Status.CashAmount.Should().Be(60);
        result.NextMovementMonthExists.Should().BeTrue();
    }
}
