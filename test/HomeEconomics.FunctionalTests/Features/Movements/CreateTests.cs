using Domain.Movements;
using FluentAssertions;
using HomeEconomics.Features.Movements;
using HomeEconomics.FunctionalTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HomeEconomics.FunctionalTests.Features.Movements
{
    public class CreateTests : FunctionalTestBase
    {
        [Fact]
        public async Task Should_Create_A_New_Movement()
        {
            var movementId = await Fixture.SendToMediatRAsync(new Create.Command(
                "Gasolina",
                60m,
                MovementType.Expense,
                new Create.Frequency
                {
                    Type = FrequencyType.Monthly
                }));

            var movement = await Fixture.QueryDbContextAsync(async homeEconomicsDbContext =>
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
        public async Task Should_Throw_InvalidOperationException_If_Movement_Exists()
        {
            var movement = new Movement("Gasolina", 60m, MovementType.Expense);
            await Fixture.InsertDbContextAsync(movement);

            Func<Task> action = async () => await Fixture.SendToMediatRAsync(new Create.Command(
                "Gasolina",
                60m,
                MovementType.Expense,
                new Create.Frequency
                {
                    Type = FrequencyType.Monthly
                }));

            action.Should().Throw<InvalidOperationException>().WithMessage(Properties.Messages.ExpenseExists);
        }
    }
}
