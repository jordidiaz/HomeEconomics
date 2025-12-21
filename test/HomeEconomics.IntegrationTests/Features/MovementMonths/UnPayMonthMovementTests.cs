using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using System.Net;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths;

public class UnPayMonthMovementTests : IntegrationTestBase
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public UnPayMonthMovementTests(Fixture fixture) : base(fixture)
    {
        
    }
    
    [Fact]
    public async Task Should_Return_200_Ok_And_UnPay_MonthMovement_And_Return_Resume()
    {
        await CreateMovementsAsync();

        var movementMonth = await CreateMovementMonthAsync();

        await AddStatusAsync(movementMonth!.Year, movementMonth.Month, 1000, 50); 

        var response = await HttpClient
            .PostAsync($"api/movement-months/{movementMonth.Id}/month-movements/{movementMonth.MonthMovements[0].Id}/unpay", null!);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<MovementMonthResponse>();
        
        result!.Status.PendingTotalExpenses.Should().Be(120m);
        result.Status.PendingTotalIncomes.Should().Be(70m);
        result.Status.AccountAmount.Should().Be(1000m);
        result.Status.CashAmount.Should().Be(50);
    }
    
    [Fact]
    public async Task Should_Return_409_Conflict_If_MovementMonth_Not_Exists()
    {
        var response = await HttpClient
            .PostAsync($"api/movement-months/1/month-movements/1/unpay", null!);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
