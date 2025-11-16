using Domain.MovementMonth;
using FluentAssertions;
using HomeEconomics.FunctionalTests.Infrastructure;
using Xunit;
using MovementMonth = HomeEconomics.Features.MovementMonths;

namespace HomeEconomics.FunctionalTests.Features.MovementMonths;

public class CreateTests : FunctionalTestBase
{
    private readonly MovementMonth.Create.Command _command = new(
        DateTime.Now.Year,
        Month.Jan);

    [Fact]
    public async Task Should_Create_A_New_MovementMonth()
    {
        await CreateMovements();
            
        var result = await Fixture.SendCommandToMediatorAsync(_command);

        result!.Id.Should().Be(result.Id);
        result.Year.Should().Be(DateTime.Now.Year);
        result.Month.Should().Be(1);
        result.Status.PendingTotalExpenses.Should().Be(120m);
        result.Status.PendingTotalIncomes.Should().Be(70m);
        result.Status.AccountAmount.Should().Be(0);
        result.Status.CashAmount.Should().Be(0);

        result.MonthMovements.Length.Should().Be(3);

        var income = result.MonthMovements.SingleOrDefault(mm => mm.Name == "Income");
        income.Should().BeNull();

        var amazon = result.MonthMovements.SingleOrDefault(mm => mm.Name == "Amazon");
        amazon.Should().BeNull();

        var gasolina = result.MonthMovements.Single(mm => mm.Name == "Gasolina");
        gasolina.Should().NotBeNull();
        gasolina.Name.Should().Be("Gasolina");
        gasolina.Amount.Should().Be(60m);
        gasolina.Type.Should().Be(1);
        gasolina.Paid.Should().BeFalse();

        var seguro = result.MonthMovements.Single(mm => mm.Name == "Seguro");
        seguro.Should().NotBeNull();
        seguro.Name.Should().Be("Seguro");
        seguro.Amount.Should().Be(70m);
        seguro.Type.Should().Be(0);
        seguro.Paid.Should().BeFalse();

        var custom = result.MonthMovements.Single(mm => mm.Name == "Custom");
        custom.Should().NotBeNull();
        custom.Name.Should().Be("Custom");
        custom.Amount.Should().Be(60m);
        custom.Type.Should().Be(1);
        custom.Paid.Should().BeFalse();
    }

    [Fact]
    public async Task Should_Throw_InvalidOperationException_If_MovementMonth_Exists()
    {
        await CreateMovements();

        await Fixture.SendCommandToMediatorAsync(_command);

        Func<Task> action = async () => await Fixture.SendCommandToMediatorAsync(_command);

        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage(Properties.Messages.MovementMonthExists);
    }

    [Fact]
    public async Task Should_Throw_InvalidOperationException_If_No_Movements()
    {
        Func<Task> action = async () => await Fixture.SendCommandToMediatorAsync(_command);

        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage(Properties.Messages.MovementsNotExists);
    }
}
