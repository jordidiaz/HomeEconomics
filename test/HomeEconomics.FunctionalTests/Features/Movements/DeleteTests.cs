using Domain.Movements;
using FluentAssertions;
using HomeEconomics.Features.Movements;
using HomeEconomics.FunctionalTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HomeEconomics.FunctionalTests.Features.Movements;

public class DeleteTests : FunctionalTestBase
{
    [Fact]
    public async Task Should_Delete_A_Movement()
    {
        var movementId = await Fixture.SendCommandToMediatorAsync(new Create.Command("Gasolina", 60m, MovementType.Expense, new Create.Frequency(FrequencyType.Monthly, 0, [])));

        var movement = await Fixture.QueryDbContextAsync(async homeEconomicsDbContext =>
        {
            return await homeEconomicsDbContext
                .Movements
                .Include(m => m.Frequency)
                .SingleOrDefaultAsync(m => m.Id == movementId);
        });

        movement.Should().NotBeNull();

        await Fixture.SendCommandToMediatorAsync(new Delete.Command(movementId));

        movement = await Fixture.QueryDbContextAsync(async homeEconomicsDbContext =>
        {
            return await homeEconomicsDbContext
                .Movements
                .Include(m => m.Frequency)
                .SingleOrDefaultAsync(m => m.Id == movementId);
        });

        movement.Should().BeNull();
    }

    [Fact]
    public async Task Should_Throw_InvalidOperationException_If_Movement_Not_Exists()
    {
        var action = async () => await Fixture.SendCommandToMediatorAsync(new Delete.Command(42));

        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage(Properties.Messages.MovementNotExists);
    }
}
