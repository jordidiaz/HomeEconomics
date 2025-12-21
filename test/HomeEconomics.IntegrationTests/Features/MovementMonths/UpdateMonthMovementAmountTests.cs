using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using System.Net;
using Domain.MovementMonth;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths;

public class UpdateMonthMovementAmountTests : IntegrationTestBase 
{
    private readonly Fixture _fixture;
    
    // ReSharper disable once ConvertToPrimaryConstructor
    public UpdateMonthMovementAmountTests(Fixture fixture) : base(fixture) => _fixture = fixture;
    
    [Fact]
    public async Task Should_Return_200_Ok_And_Update_MonthMovement_Amount_Return_Resume()
    {
        await CreateMovementsAsync();

        var createMovementMonthResult = await _fixture.SendCommandToMediatorAsync(
            new Create.Command(
                DateTime.Now.Year,
                Month.Jan));

        var command = new UpdateMonthMovementAmount.Command(
            createMovementMonthResult!.Id,
            createMovementMonthResult.MonthMovements[0].Id,
            70m);

        var response = await HttpClient
            .PostAsync($"api/movement-months/{createMovementMonthResult.Id}/month-movements/{createMovementMonthResult.MonthMovements[0].Id}/update-amount", command);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<MovementMonthResponse>();

        result!.Status.PendingTotalExpenses.Should().Be(130m);
        result.Status.PendingTotalIncomes.Should().Be(70m);
    }

    [Fact]
    public async Task Should_Return_409_Conflict_If_MovementMonth_Not_Exists()
    {
        var command = new UpdateMonthMovementAmount.Command(1, 1, 70m);
        
        var response = await HttpClient
            .PostAsync("api/movement-months/1/month-movements/1/update-amount", command);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Should_Return_400_BadRequest()
    {
        var command = new UpdateMonthMovementAmount.Command(1, 1, -1);

        var response = await HttpClient
            .PostAsync("api/movement-months/1/month-movements/1/update-amount", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
