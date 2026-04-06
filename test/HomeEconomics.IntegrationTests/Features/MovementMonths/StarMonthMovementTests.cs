using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using System.Net;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths;

public class StarMonthMovementTests : IntegrationTestBase
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public StarMonthMovementTests(Fixture fixture) : base(fixture)
    {

    }

    [Fact]
    public async Task Should_Return_200_Ok_And_Star_MonthMovement_And_Return_Response()
    {
        await CreateMovementsAsync();

        var movementMonth = await CreateMovementMonthAsync();

        var monthMovementId = movementMonth!.MonthMovements[0].Id;

        var response = await HttpClient
            .PostAsync($"api/movement-months/{movementMonth.Id}/month-movements/{monthMovementId}/star", null!);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<MovementMonthResponse>();

        result!.MonthMovements.Should().Contain(mm => mm.Id == monthMovementId && mm.Starred);
    }

    [Fact]
    public async Task Should_Return_200_Ok_And_Starred_Movement_Is_First_In_Response()
    {
        await CreateMovementsAsync();

        var movementMonth = await CreateMovementMonthAsync();

        // Star the last movement (alphabetically) to verify it moves to the top
        var lastMovement = movementMonth!.MonthMovements.Last();

        await HttpClient
            .PostAsync($"api/movement-months/{movementMonth.Id}/month-movements/{lastMovement.Id}/star", null!);

        var detailResponse = await HttpClient
            .GetAsync($"api/movement-months/{movementMonth.Year}/{movementMonth.Month}");

        var result = await detailResponse.Content.ReadFromJsonAsync<MovementMonthResponse>();

        result!.MonthMovements[0].Id.Should().Be(lastMovement.Id);
        result.MonthMovements[0].Starred.Should().BeTrue();
    }

    [Fact]
    public async Task Should_Return_409_Conflict_If_MovementMonth_Not_Exists()
    {
        var response = await HttpClient
            .PostAsync($"api/movement-months/1/month-movements/1/star", null!);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
