using Domain.Movements;
using FluentAssertions;
using HomeEconomics.FunctionalTests.Infrastructure;
using Xunit;

namespace HomeEconomics.FunctionalTests.Features.Movements;

public class IndexTests : FunctionalTestBase
{
    [Fact]
    public async Task Should_List_Movements()
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
        await Fixture.InsertDbContextAsync(entities);

        var result = await Fixture.SendToMediatRAsync(new HomeEconomics.Features.Movements.Index.Query());

        result.Movements.Length.Should().Be(3);

        var movementResult1 = result.Movements.Single(m => m.Name == "Gasolina");
        movementResult1.Name.Should().Be("Gasolina");
        movementResult1.Amount.Should().Be(60m);
        movementResult1.Type.Should().Be(1);
        movementResult1.FrequencyType.Should().Be(0);
        movementResult1.FrequencyMonths.Length.Should().Be(12);
        movementResult1.FrequencyMonths.All(x => x is false).Should().BeTrue();

        var movementResult2 = result.Movements.Single(m => m.Name == "EPSV");
        movementResult2.Name.Should().Be("EPSV");
        movementResult2.Amount.Should().Be(50m);
        movementResult2.Type.Should().Be(1);
        movementResult2.FrequencyType.Should().Be(0);
        movementResult2.FrequencyMonths.Length.Should().Be(12);
        movementResult2.FrequencyMonths.All(x => x is false).Should().BeTrue();

        var movementResult3 = result.Movements.Single(m => m.Name == "Crossfit");
        movementResult3.Name.Should().Be("Crossfit");
        movementResult3.Amount.Should().Be(75m);
        movementResult3.Type.Should().Be(1);
        movementResult3.FrequencyType.Should().Be(0);
        movementResult3.FrequencyMonths.Length.Should().Be(12);
        movementResult3.FrequencyMonths.All(x => x is false).Should().BeTrue();
    }
}