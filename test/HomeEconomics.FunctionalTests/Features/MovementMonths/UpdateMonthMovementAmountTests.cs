using Domain.MovementMonth;
using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.FunctionalTests.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HomeEconomics.FunctionalTests.Features.MovementMonths
{
    public class UpdateMonthMovementAmountTests : FunctionalTestBase
    {
        private UpdateMonthMovementAmount.Command _command;

        public UpdateMonthMovementAmountTests()
        {
            _command = new UpdateMonthMovementAmount.Command();
        }

        [Fact]
        public async Task Should_Update_MonthMovement_Amount_Return_Resume()
        {
            await CreateMovements();

            var createMovementMonthResult = await Fixture.SendToMediatRAsync(
                new Create.Command
                {
                    Year = DateTime.Now.Year,
                    Month = Month.Jan
                });

            _command = new UpdateMonthMovementAmount.Command
            {
                MovementMonthId = createMovementMonthResult.Id,
                MonthMovementId = createMovementMonthResult.MonthMovements.First().Id,
                Amount = 70m
            };

            var result = await Fixture.SendToMediatRAsync(_command);

            result.Status.PendingTotalExpenses.Should().Be(130m);
            result.Status.PendingTotalIncomes.Should().Be(70m);
        }

        [Fact]
        public void Should_Throw_InvalidOperationException_If_MovementMonth_Not_Exists()
        {
            _command = new UpdateMonthMovementAmount.Command
            {
                MovementMonthId = 0,
                MonthMovementId = 0,
                Amount = 70m
            };

            Func<Task> action = async () => await Fixture.SendToMediatRAsync(_command);

            action.Should().Throw<InvalidOperationException>().WithMessage(Properties.Messages.MovementMonthNotExists);
        }
    }
}
