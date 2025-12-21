using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using System.Net;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths;

public class DeleteTests: IntegrationTestBase
{
    private readonly Fixture _fixture;
    
    // ReSharper disable once ConvertToPrimaryConstructor
    public DeleteTests(Fixture fixture) : base(fixture) => _fixture = fixture;
    
    [Fact]
    public async Task Should_Return_200_Ok_And_Add_MonthMovement_And_Return_Resume()
    {
        await CreateMovementsAsync();

        var movementMonth = await CreateMovementMonthAsync();

        var response = await HttpClient
            .DeleteAsync($"api/movement-months/{movementMonth!.Id}/month-movements/{movementMonth.MonthMovements[0].Id}");

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<MovementMonthResponse>();

        result!.Status.PendingTotalExpenses.Should().Be(60m);
        result.Status.PendingTotalIncomes.Should().Be(70m);
        result.MonthMovements.Length.Should().Be(2);
    }

    [Fact]
    public async Task Should_Throw_InvalidOperationException_If_MovementMonth_Not_Exists()
    {
        var command = new DeleteMonthMovement.Command(0, 0);

        Func<Task> action = async () => await _fixture.SendCommandToMediatorAsync(command);

        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage(Properties.Messages.MovementMonthNotExists);
    }
    
    [Fact]
    public async Task Should_Return_409_Conflict_If_MovementMonth_Not_Exists()
    {
        var response = await HttpClient
            .DeleteAsync($"api/movement-months/1/month-movements/1");

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
