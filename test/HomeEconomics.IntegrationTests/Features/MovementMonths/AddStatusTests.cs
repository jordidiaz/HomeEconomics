using Domain.MovementMonth;
using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using System.Net;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths;

public class AddStatusTests: IntegrationTestBase
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public AddStatusTests(Fixture fixture) : base(fixture)
    {
        
    }
    
    [Fact]
    public async Task Should_Return_200_OkAnd_Add_Status()
    {
        await CreateMovementsAsync();

        var movementMonth = await CreateMovementMonthAsync();

        var command = new AddStatus.Command(
            movementMonth!.Year,
            (Month)movementMonth.Month,
            1000,
            50);

        var response = await HttpClient
            .PostAsync($"api/movement-months/{movementMonth.Id}/add-status", command);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<MovementMonthResponse>();
        
        result!.Status.AccountAmount.Should().Be(1000);
        result.Status.CashAmount.Should().Be(50);
        result.Status.PendingTotalExpenses.Should().Be(120.00m);
        result.Status.PendingTotalIncomes.Should().Be(70.00m);
    }

    [Fact]
    public async Task Should_Return_409_Conflict_If_MovementMonth_Not_Exists()
    {
        var command = new AddStatus.Command(
            DateTime.Now.Year,
            Month.Apr,
            1000,
            50);
        
        var response = await HttpClient
            .PostAsync($"api/movement-months/1/add-status", command);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Should_Return_400_BadRequest()
    {
        var command = new AddStatus.Command(
            DateTime.Now.Year - 1,
            0,
            900,
            50);

        var response = await HttpClient
            .PostAsync("api/movement-months/1/add-status", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
}
