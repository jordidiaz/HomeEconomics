using System;
using System.Threading.Tasks;
using Domain.Movements;
using FluentAssertions;
using HomeEconomics.Features.Movements;
using HomeEconomics.FunctionalTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HomeEconomics.FunctionalTests.Features.Movements
{
    public class DeleteTests : FunctionalTestBase
    {
        [Fact]
        public async Task Should_Delete_A_Movement()
        {
            var movementId = await Fixture.SendToMediatRAsync(new Create.Command
            {
                Name = "Gasolina",
                Amount = 60m,
                Type = MovementType.Expense,
                Frequency = new Create.Frequency
                {
                    Type = FrequencyType.Monthly
                }
            });

            var movement = await Fixture.QueryDbContextAsync(async homeEconomicsDbContext =>
            {
                return await homeEconomicsDbContext
                    .Movements
                    .Include(m => m.Frequency)
                    .SingleOrDefaultAsync(m => m.Id == movementId);
            });

            movement.Should().NotBeNull();

            await Fixture.SendToMediatRAsync(new Delete.Command
            {
                Id = movementId
            });

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
        public void Should_Throw_InvalidOperationException_If_Movement_Not_Exists()
        {
            Func<Task> action = async () => await Fixture.SendToMediatRAsync(new Delete.Command
            {
                Id = 42
            });

            action.Should().Throw<InvalidOperationException>().WithMessage(Properties.Messages.MovementNotExists);
        }
    }
}