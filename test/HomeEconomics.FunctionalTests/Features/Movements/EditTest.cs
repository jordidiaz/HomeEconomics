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
            new Create.Frequency
            {
                Type = FrequencyType.Monthly
            }));

        await Fixture.SendCommandToMediatorAsync(new Edit.Command
        {
            Id = movementId,
            Name = "EPSV",
            Amount = 50m,
            Type = MovementType.Income,
            Frequency = new Edit.Frequency
            {
                Type = FrequencyType.Custom,
                Months =
                [
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
                ]
            }
        });

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
        Func<Task> action = async () => await Fixture.SendCommandToMediatorAsync(new Edit.Command
        {
            Id = 42,
            Name = "Gasolina",
            Amount = 60m,
            Type = MovementType.Expense,
            Frequency = new Edit.Frequency
            {
                Type = FrequencyType.Monthly
            }
        });

        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage(Properties.Messages.MovementNotExists);
    }
}