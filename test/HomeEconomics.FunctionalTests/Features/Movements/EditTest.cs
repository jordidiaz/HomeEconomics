using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Movements;
using FluentAssertions;
using HomeEconomics.Features.Movements;
using HomeEconomics.FunctionalTests.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HomeEconomics.FunctionalTests.Features.Movements
{
    public class EditTest : FunctionalTestBase
    {
        [Fact]
        public async Task Should_Edit_The_Movement()
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

            await Fixture.SendToMediatRAsync<Unit>(new Edit.Command
            {
                Id = movementId,
                Name = "EPSV",
                Amount = 50m,
                Type = MovementType.Income,
                Frequency = new Create.Frequency
                {
                    Type = FrequencyType.Custom,
                    Months = new []
                    {
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
                    }
                }
            });

            var movement = await Fixture.QueryDbContextAsync(async homeEconomicsDbContext =>
            {
                return await homeEconomicsDbContext
                    .Movements
                    .Include(m => m.Frequency)
                    .SingleOrDefaultAsync(m => m.Id == movementId);
            });

            movement.Id.Should().Be(movementId);
            movement.Name.Should().Be("EPSV");
            movement.Amount.Should().Be(50m);
            movement.Type.Should().Be(MovementType.Income);
            movement.Frequency.Type.Should().Be(FrequencyType.Custom);
            movement.Frequency.Months.SequenceEqual(new[]
            {
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
            }).Should().BeTrue();
        }

        [Fact]
        public void Should_Throw_InvalidOperationException_If_Movement_Not_Exists()
        {
            Func<Task> action = async () => await Fixture.SendToMediatRAsync<Unit>(new Edit.Command
            {
                Id = 42,
                Name = "Gasolina",
                Amount = 60m,
                Type = MovementType.Expense,
                Frequency = new Create.Frequency
                {
                    Type = FrequencyType.Monthly
                }
            });

            action.Should().Throw<InvalidOperationException>().WithMessage(Properties.Messages.MovementNotExists);
        }
    }
}