using Domain.MovementMonth;
using Domain.Movements;
using HomeEconomics.Features.MovementMonths;
using Xunit;

namespace HomeEconomics.FunctionalTests.Infrastructure;

public class FunctionalTestBase : IAsyncLifetime
{
    public async Task InitializeAsync() => await Fixture.ResetDatabaseAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    protected static async Task CreateMovements()
    {
        await Fixture.SendToMediatRAsync(new HomeEconomics.Features.Movements.Create.Command(
            "Income",
            50m,
            MovementType.Expense,
            new HomeEconomics.Features.Movements.Create.Frequency
            {
                Type = FrequencyType.None
            }));

        await Fixture.SendToMediatRAsync(new HomeEconomics.Features.Movements.Create.Command(
            "Gasolina",
            60m,
            MovementType.Expense,
            new HomeEconomics.Features.Movements.Create.Frequency
            {
                Type = FrequencyType.Monthly
            }));

        await Fixture.SendToMediatRAsync(new HomeEconomics.Features.Movements.Create.Command(
            "Amazon",
            30m,
            MovementType.Expense,
            new HomeEconomics.Features.Movements.Create.Frequency
            {
                Type = FrequencyType.Yearly,
                Month = 2
            }));

        await Fixture.SendToMediatRAsync(new HomeEconomics.Features.Movements.Create.Command(
            "Seguro",
            70m,
            MovementType.Income,
            new HomeEconomics.Features.Movements.Create.Frequency
            {
                Type = FrequencyType.Yearly,
                Month = 1
            }));

        await Fixture.SendToMediatRAsync(new HomeEconomics.Features.Movements.Create.Command(
            "Custom",
            60m,
            MovementType.Expense,
            new HomeEconomics.Features.Movements.Create.Frequency
            {
                Type = FrequencyType.Custom,
                Months =
                [
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
                ]
            }));
    }

    protected static async Task<MovementMonthResponse> CreateMovementMonth(Month month = Month.Jan)
    {
        var year = DateTime.Now.Year;

        var movementMonth = await Fixture.SendToMediatRAsync(new Create.Command(
            year,
            month));

        return movementMonth;
    }

    protected static async Task AddStatus(int year, int month, decimal accountAmount, decimal cashAmount) =>
        await Fixture.SendToMediatRAsync(new AddStatus.Command(
            year,
            (Month)month,
            accountAmount,
            cashAmount));
}
