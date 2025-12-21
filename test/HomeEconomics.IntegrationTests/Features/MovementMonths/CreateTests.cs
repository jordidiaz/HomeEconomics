using Domain.MovementMonth;
using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using System.Net;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths;

public class CreateTests : IntegrationTestBase
{
    private readonly Fixture _fixture;
    
    // ReSharper disable once ConvertToPrimaryConstructor
    public CreateTests(Fixture fixture) : base(fixture) => _fixture = fixture;
    
    private Create.Command _command = new(
        DateTime.Now.Year,
        Month.Jan);

    private const string Uri = "api/movement-months";
    
    [Fact]
    public async Task Should_Create_A_New_MovementMonth()
    {
        await CreateMovementsAsync();
        
        var response = await HttpClient
            .PostAsync(Uri, _command);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<MovementMonthResponse>();
        
        result!.Year.Should().Be(DateTime.Now.Year);
        result.Month.Should().Be(1);
        result.Status.PendingTotalExpenses.Should().Be(120m);
        result.Status.PendingTotalIncomes.Should().Be(70m);
        result.Status.AccountAmount.Should().Be(0);
        result.Status.CashAmount.Should().Be(0);

        result.MonthMovements.Length.Should().Be(3);

        var income = result.MonthMovements.SingleOrDefault(mm => mm.Name == "Income");
        income.Should().BeNull();

        var amazon = result.MonthMovements.SingleOrDefault(mm => mm.Name == "Amazon");
        amazon.Should().BeNull();

        var gasolina = result.MonthMovements.Single(mm => mm.Name == "Gasolina");
        gasolina.Should().NotBeNull();
        gasolina.Name.Should().Be("Gasolina");
        gasolina.Amount.Should().Be(60m);
        gasolina.Type.Should().Be(1);
        gasolina.Paid.Should().BeFalse();

        var seguro = result.MonthMovements.Single(mm => mm.Name == "Seguro");
        seguro.Should().NotBeNull();
        seguro.Name.Should().Be("Seguro");
        seguro.Amount.Should().Be(70m);
        seguro.Type.Should().Be(0);
        seguro.Paid.Should().BeFalse();

        var custom = result.MonthMovements.Single(mm => mm.Name == "Custom");
        custom.Should().NotBeNull();
        custom.Name.Should().Be("Custom");
        custom.Amount.Should().Be(60m);
        custom.Type.Should().Be(1);
        custom.Paid.Should().BeFalse();
    }

    [Fact]
    public async Task Should_Return_409_Conflict_If_MovementMonth_Exists()
    {
        await CreateMovementsAsync();

        await _fixture.SendCommandToMediatorAsync(_command);

        var response = await HttpClient
            .PostAsync("api/movement-months", _command);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Should_Return_400_BadRequest()
    {
        _command = new Create.Command(
            DateTime.Now.Year - 1,
            Month.Feb);

        var response = await HttpClient
            .PostAsync(Uri, _command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
