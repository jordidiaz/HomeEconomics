using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.MovementMonth;
using Domain.Movements;
using FluentAssertions;
using HomeEconomics.Features.Movements;
using MovementMonth = HomeEconomics.Features.MovementMonths;
using HomeEconomics.FunctionalTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HomeEconomics.FunctionalTests.Features.MovementMonths
{
    public class CreateTests : FunctionalTestBase
    {
        private readonly MovementMonth.Create.Command _command;

        public CreateTests()
        {
            _command = new MovementMonth.Create.Command
            {
                Year = 2020,
                Month = Month.Jan
            };
        }

        [Fact]
        public async Task Should_Create_A_New_MovementMonth()
        {
            await InsertMovements();
            
            var movementMonthId = await Fixture.SendToMediatRAsync(_command);

            var movementMonth = await Fixture.QueryDbContextAsync(async homeEconomicsDbContext =>
            {
                return await homeEconomicsDbContext
                    .MovementMonths
                    .Include(mm => mm.MonthMovements)
                    .SingleOrDefaultAsync(mm => mm.Id == movementMonthId);
            });

            movementMonth.Id.Should().Be(movementMonthId);
            movementMonth.Year.Should().Be(2020);
            movementMonth.Month.Should().Be(Month.Jan);

            movementMonth.MonthMovements.Count.Should().Be(3);

            var income = movementMonth.MonthMovements.SingleOrDefault(mm => mm.Name == "Income");
            income.Should().BeNull();

            var amazon = movementMonth.MonthMovements.SingleOrDefault(mm => mm.Name == "Amazon");
            amazon.Should().BeNull();

            var gasolina = movementMonth.MonthMovements.Single(mm => mm.Name == "Gasolina");
            gasolina.Should().NotBeNull();
            gasolina.Name.Should().Be("Gasolina");
            gasolina.Amount.Should().Be(60m);
            gasolina.Type.Should().Be(MovementType.Expense);

            var seguro = movementMonth.MonthMovements.Single(mm => mm.Name == "Seguro");
            seguro.Should().NotBeNull();
            seguro.Name.Should().Be("Seguro");
            seguro.Amount.Should().Be(70m);
            seguro.Type.Should().Be(MovementType.Income);

            var custom = movementMonth.MonthMovements.Single(mm => mm.Name == "Custom");
            custom.Should().NotBeNull();
            custom.Name.Should().Be("Custom");
            custom.Amount.Should().Be(60m);
            custom.Type.Should().Be(MovementType.Expense);
        }

        [Fact]
        public async Task Should_Throw_InvalidOperationException_If_MovementMonth_Exists()
        {
            await InsertMovements();

            await Fixture.SendToMediatRAsync(_command);

            Func<Task> action = async () => await Fixture.SendToMediatRAsync(_command);

            action.Should().Throw<InvalidOperationException>().WithMessage(Properties.Messages.MovementMonthExists);
        }

        [Fact]
        public void Should_Throw_InvalidOperationException_If_No_Movements()
        {
            Func<Task> action = async () => await Fixture.SendToMediatRAsync(_command);

            action.Should().Throw<InvalidOperationException>().WithMessage(Properties.Messages.MovementsNotExists);
        }

        private static async Task InsertMovements()
        {
            await Fixture.SendToMediatRAsync(new Create.Command
            {
                Name = "Income",
                Amount = 50m,
                Type = MovementType.Income,
                Frequency = new Create.Frequency
                {
                    Type = FrequencyType.None
                }
            });

            await Fixture.SendToMediatRAsync(new Create.Command
            {
                Name = "Gasolina",
                Amount = 60m,
                Type = MovementType.Expense,
                Frequency = new Create.Frequency
                {
                    Type = FrequencyType.Monthly
                }
            });

            await Fixture.SendToMediatRAsync(new Create.Command
            {
                Name = "Amazon",
                Amount = 30m,
                Type = MovementType.Expense,
                Frequency = new Create.Frequency
                {
                    Type = FrequencyType.Yearly,
                    Month = 2
                }
            });

            await Fixture.SendToMediatRAsync(new Create.Command
            {
                Name = "Seguro",
                Amount = 70m,
                Type = MovementType.Income,
                Frequency = new Create.Frequency
                {
                    Type = FrequencyType.Yearly,
                    Month = 1
                }
            });

            await Fixture.SendToMediatRAsync(new Create.Command
            {
                Name = "Custom",
                Amount = 60m,
                Type = MovementType.Expense,
                Frequency = new Create.Frequency
                {
                    Type = FrequencyType.Custom,
                    Months = new []
                    {
                        true,
                        false,
                        false,
                        true,
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
        }
    }
}
