using System.Net;
using Domain.Movements;
using FluentAssertions;
using HomeEconomics.IntegrationTests.Infrastructure;
using Xunit;
using Index = HomeEconomics.Features.Movements.Index;

namespace HomeEconomics.IntegrationTests.Features.Movements;

public class IndexTests: IntegrationTestBase
{
    private readonly Fixture _fixture;
    
    // ReSharper disable once ConvertToPrimaryConstructor
    public IndexTests(Fixture fixture) : base(fixture) => _fixture = fixture;

    [Fact]
    public async Task Should_Return_200_And_List_Movements()
    {
        var movement1 = new Movement("Gasolina", 60m, MovementType.Expense);
        var movement2 = new Movement("EPSV", 50m, MovementType.Expense);
        var movement3 = new Movement("Crossfit", 75m, MovementType.Expense);
        object[] entities =
        [
            movement1,
            movement2,
            movement3
        ];
        await _fixture.InsertDbContextAsync(entities);

        var response = await HttpClient.GetAsync("api/movements");

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<Index.Result>();

        result!.Movements.Length.Should().Be(3);

        var movementResult1 = result.Movements.Single(m => m.Name == "Gasolina");
        movementResult1.Name.Should().Be("Gasolina");
        movementResult1.Amount.Should().Be(60m);
        movementResult1.Type.Should().Be(1);
        movementResult1.FrequencyType.Should().Be(0);
        movementResult1.FrequencyMonths.Length.Should().Be(12);
        movementResult1.FrequencyMonths.All(x => !x).Should().BeTrue();

        var movementResult2 = result.Movements.Single(m => m.Name == "EPSV");
        movementResult2.Name.Should().Be("EPSV");
        movementResult2.Amount.Should().Be(50m);
        movementResult2.Type.Should().Be(1);
        movementResult2.FrequencyType.Should().Be(0);
        movementResult2.FrequencyMonths.Length.Should().Be(12);
        movementResult2.FrequencyMonths.All(x => !x).Should().BeTrue();

        var movementResult3 = result.Movements.Single(m => m.Name == "Crossfit");
        movementResult3.Name.Should().Be("Crossfit");
        movementResult3.Amount.Should().Be(75m);
        movementResult3.Type.Should().Be(1);
        movementResult3.FrequencyType.Should().Be(0);
        movementResult3.FrequencyMonths.Length.Should().Be(12);
        movementResult3.FrequencyMonths.All(x => !x).Should().BeTrue();
    }
}
