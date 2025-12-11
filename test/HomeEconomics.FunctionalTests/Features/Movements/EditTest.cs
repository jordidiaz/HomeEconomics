using Domain.Movements;
using FluentAssertions;
using HomeEconomics.Features.Movements;
using HomeEconomics.FunctionalTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HomeEconomics.FunctionalTests.Features.Movements;

public class EditTest : FunctionalTestBase
{
    [Fact]
    public async Task Should_Edit_The_Movement()
    {
        var movementId = await Fixture.SendCommandToMediatorAsync(new Create.Command(
            "Gasolina",
            60m,
            MovementType.Expense,
            new Create.Frequency(FrequencyType.Monthly, 0, [])));

        await Fixture.SendCommandToMediatorAsync(new Edit.Command(movementId, "EPSV", 50m, MovementType.Income,
            new Edit.Frequency(FrequencyType.Custom, 0, [
                true,
                false,
                true,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false
            ])));

        var movement = await Fixture.QueryDbContextAsync(async homeEconomicsDbContext =>
        {
            return await homeEconomicsDbContext
                .Movements
                .Include(m => m.Frequency)
                .SingleOrDefaultAsync(m => m.Id == movementId);
        });

        movement!.Id.Should().Be(movementId);
        movement.Name.Should().Be("EPSV");
        movement.Amount.Should().Be(50m);
        movement.Type.Should().Be(MovementType.Income);
        movement.Frequency.Type.Should().Be(FrequencyType.Custom);
        movement.Frequency.Months.SequenceEqual([
            true,
            false,
            true,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false
        ]).Should().BeTrue();
    }

    [Fact]
    public async Task Should_Throw_InvalidOperationException_If_Movement_Not_Exists()
    {
        var action = async () => await Fixture.SendCommandToMediatorAsync(new Edit.Command(42, "Gasolina", 60m, MovementType.Expense, new Edit.Frequency(FrequencyType.Monthly, 0, [])));

        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage(Properties.Messages.MovementNotExists);
    }
}
