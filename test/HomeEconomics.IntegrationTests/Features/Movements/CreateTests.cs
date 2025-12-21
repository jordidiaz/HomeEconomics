using System.Globalization;
using Domain.Movements;
using FluentAssertions;
using HomeEconomics.Features.Movements;
using HomeEconomics.IntegrationTests.Infrastructure;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.Movements;

public class CreateTests: IntegrationTestBase
{
    private readonly Fixture _fixture;
    
    // ReSharper disable once ConvertToPrimaryConstructor
    public CreateTests(Fixture fixture) : base(fixture) => _fixture = fixture;

    private Create.Command _command = new("EPSV", 50m, MovementType.Expense, new Create.Frequency
    {
        Type = FrequencyType.Monthly
    });

    private const string Uri = "api/movements";
    
    [Fact]
    public async Task Should_Return_200_Ok_And_Create_A_New_Movement()
    {
        var command = new Create.Command(
            "Gasolina",
            60m,
            MovementType.Expense,
            new Create.Frequency
            {
                Type = FrequencyType.Monthly
            });
        
        var response = await HttpClient.PostAsync(Uri, command);
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var movementId = int.Parse(
            await response.Content.ReadAsStringAsync(),
            CultureInfo.InvariantCulture);

        var movement = await _fixture.QueryDbContextAsync(async homeEconomicsDbContext =>
        {
            return await homeEconomicsDbContext
                .Movements
                .Include(m => m.Frequency)
                .SingleOrDefaultAsync(m => m.Id == movementId);
        });

        movement!.Id.Should().Be(movementId);
        movement.Name.Should().Be("Gasolina");
        movement.Amount.Should().Be(60m);
        movement.Type.Should().Be(MovementType.Expense);
        movement.Frequency.Type.Should().Be(FrequencyType.Monthly);
        movement.Frequency.Months.SequenceEqual(new bool[12]).Should().BeTrue();
    }

    [Fact]
    public async Task Should_Return_400_BadRequest_If_Command_Not_Valid()
    {
        _command = new Create.Command(string.Empty, 50m, MovementType.Expense, new Create.Frequency
        {
            Type = FrequencyType.Monthly
        });

        var response = await HttpClient
            .PostAsync(Uri, _command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
