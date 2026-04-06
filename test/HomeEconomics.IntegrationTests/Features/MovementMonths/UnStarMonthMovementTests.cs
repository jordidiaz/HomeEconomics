using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using System.Net;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths;

public class UnStarMonthMovementTests : IntegrationTestBase
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public UnStarMonthMovementTests(Fixture fixture) : base(fixture)
    {

    }

    [Fact]
    public async Task Should_Return_200_Ok_And_UnStar_MonthMovement_And_Return_Response()
    {
        await CreateMovementsAsync();

        var movementMonth = await CreateMovementMonthAsync();

        var monthMovementId = movementMonth!.MonthMovements[0].Id;

        // First star it
        await HttpClient
            .PostAsync($"api/movement-months/{movementMonth.Id}/month-movements/{monthMovementId}/star", null!);

        // Then unstar
        var response = await HttpClient
            .PostAsync($"api/movement-months/{movementMonth.Id}/month-movements/{monthMovementId}/unstar", null!);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<MovementMonthResponse>();

        result!.MonthMovements.Should().Contain(mm => mm.Id == monthMovementId && !mm.Starred);
    }

    [Fact]
    public async Task Should_Return_409_Conflict_If_MovementMonth_Not_Exists()
    {
        var response = await HttpClient
            .PostAsync($"api/movement-months/1/month-movements/1/unstar", null!);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
