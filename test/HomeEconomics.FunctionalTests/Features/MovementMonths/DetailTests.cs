using System.Threading.Tasks;
using Domain.MovementMonth;
using Domain.Movements;
using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.FunctionalTests.Infrastructure;
using Xunit;

namespace HomeEconomics.FunctionalTests.Features.MovementMonths
{
    public class DetailTests : FunctionalTestBase
    {
        [Fact]
        public async Task Should_Return_MovementMonth_Detail()
        {
            var movement = new Movement("Gasolina", 60m, MovementType.Expense);
            movement.SetMonthlyFrequency();

            object[] entities = {
                movement
            };

            await Fixture.InsertDbContextAsync(entities);

            var movementMonth = await Fixture.SendToMediatRAsync(new Create.Command
            {
                Year = 2020,
                Month = Month.Aug
            });

            var movementMonthDetail = await Fixture.SendToMediatRAsync(new Detail.Query
            {
                Year = 2020,
                Month = 8
            });

            movementMonth.Should().BeEquivalentTo(movementMonthDetail);
        }

        [Fact]
        public async Task Should_Return_Null_If_MovementMonth_Not_Exist()
        {
            var movementMonthDetail = await Fixture.SendToMediatRAsync(new Detail.Query
            {
                Year = 2020,
                Month = 8
            });

            movementMonthDetail.Should().BeNull();
        }
    }
}
