using Domain.Movements;
using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using System.Net;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths;

public class UpdateMonthMovementTests : IntegrationTestBase
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public UpdateMonthMovementTests(Fixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Should_Return_200_Ok_And_Update_MonthMovement_And_Return_Resume()
    {
        await CreateMovementsAsync();

        var movementMonth = await CreateMovementMonthAsync();

        var command = new UpdateMonthMovement.Command(
            movementMonth!.Id,
            movementMonth.MonthMovements[0].Id,
            "Nombre actualizado",
            99m,
            MovementType.Income);

        var response = await HttpClient
            .PostAsync($"api/movement-months/{movementMonth.Id}/month-movements/{movementMonth.MonthMovements[0].Id}/update", command);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<MovementMonthResponse>();

        var updated = result!.MonthMovements.First(mm => mm.Id == movementMonth.MonthMovements[0].Id);
        updated.Name.Should().Be("Nombre actualizado");
        updated.Amount.Should().Be(99m);
        updated.Type.Should().Be((int)MovementType.Income);
    }

    [Fact]
    public async Task Should_Return_409_Conflict_If_MovementMonth_Not_Exists()
    {
        var command = new UpdateMonthMovement.Command(1, 1, "Test", 10m, MovementType.Expense);

        var response = await HttpClient
            .PostAsync("api/movement-months/1/month-movements/1/update", command);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Should_Return_400_BadRequest()
    {
        var command = new UpdateMonthMovement.Command(1, 1, "", -1m, MovementType.Expense);

        var response = await HttpClient
            .PostAsync("api/movement-months/1/month-movements/1/update", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
