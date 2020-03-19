using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.FunctionalTests.Infrastructure;
using System.Threading.Tasks;
using Xunit;

namespace HomeEconomics.FunctionalTests.Features.MovementMonths
{
    public class DetailTests : FunctionalTestBase
    {
        [Fact]
        public async Task Should_Return_MovementMonth_Detail()
        {
            await CreateMovements();

            var movementMonth = await CreateMovementMonth();

            await AddStatus(movementMonth.Year, movementMonth.Month, 1000, 50);
            await AddStatus(movementMonth.Year, movementMonth.Month, 900, 60);

            var result = await Fixture.SendToMediatRAsync(new Detail.Query
            {
                Year = movementMonth.Year,
                Month = movementMonth.Month
            });

            result.Status.PendingTotalExpenses.Should().Be(120m);
            result.Status.PendingTotalIncomes.Should().Be(70m);
            result.Status.AccountAmount.Should().Be(900);
            result.Status.CashAmount.Should().Be(60);
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
