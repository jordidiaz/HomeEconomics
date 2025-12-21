using Domain.Movements;
using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using System.Net;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths;

public class AddMonthMovementTests : IntegrationTestBase
{
    private readonly Fixture _fixture;
    
    // ReSharper disable once ConvertToPrimaryConstructor
    public AddMonthMovementTests(Fixture fixture) : base(fixture) => _fixture = fixture;
    
    [Fact]
    public async Task Should_Return_200_Ok_And_Add_MonthMovement_And_Return_Resume()
    {
        await CreateMovementsAsync();

        var movementMonth = await CreateMovementMonthAsync();

        var command = new AddMonthMovement.Command(
            movementMonth!.Id,
            "new",
            50,
            MovementType.Expense);
        
        var response = await HttpClient
            .PostAsync($"api/movement-months/{movementMonth.Id}/month-movements", command);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<MovementMonthResponse>();

        result!.Status.PendingTotalExpenses.Should().Be(170m);
        result.Status.PendingTotalIncomes.Should().Be(70m);
        result.MonthMovements.Length.Should().Be(4);
    }

    [Fact]
    public async Task Should_Throw_InvalidOperationException_If_MovementMonth_Not_Exists()
    {
        var command = new AddMonthMovement.Command(
            0,
            "new",
            50,
            MovementType.Expense);

        Func<Task> action = async () => await _fixture.SendCommandToMediatorAsync(command);

        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage(Properties.Messages.MovementMonthNotExists);
    }

    [Fact]
    public async Task Should_Return_400_BadRequest()
    {
        var command = new AddMonthMovement.Command(
            1,
            "Gasolina",
            -0.1m,
            MovementType.Expense);

        var response = await HttpClient
            .PostAsync("api/movement-months/1/month-movements", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
