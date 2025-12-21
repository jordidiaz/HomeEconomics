using Domain.MovementMonth;
using Domain.Movements;
using HomeEconomics.Features.MovementMonths;
using Npgsql;
using Respawn;
using Xunit;

namespace HomeEconomics.IntegrationTests.Infrastructure;

[Collection(Collections.IntegrationTestCollection)]
public abstract class IntegrationTestBase(Fixture fixture) : IAsyncLifetime
{
    protected HttpClient HttpClient => fixture.HttpClient;
    public async Task InitializeAsync() => await ResetDatabaseAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    private async Task ResetDatabaseAsync()
    {
        await using var npgsqlConnection = new NpgsqlConnection(fixture.ConnectionString);
        await npgsqlConnection.OpenAsync();
        var respawner = await Respawner.CreateAsync(npgsqlConnection, new RespawnerOptions
        {
            SchemasToInclude =
            [
                "public"
            ],
            DbAdapter = DbAdapter.Postgres
        });
        await respawner.ResetAsync(npgsqlConnection);
    }
    
    protected async Task CreateMovementsAsync()
    {
        await fixture.SendCommandToMediatorAsync(new HomeEconomics.Features.Movements.Create.Command(
            "Income",
            50m,
            MovementType.Expense,
            new HomeEconomics.Features.Movements.Create.Frequency
            {
                Type = FrequencyType.None
            }));

        await fixture.SendCommandToMediatorAsync(new HomeEconomics.Features.Movements.Create.Command(
            "Gasolina",
            60m,
            MovementType.Expense,
            new HomeEconomics.Features.Movements.Create.Frequency
            {
                Type = FrequencyType.Monthly
            }));

        await fixture.SendCommandToMediatorAsync(new HomeEconomics.Features.Movements.Create.Command(
            "Amazon",
            30m,
            MovementType.Expense,
            new HomeEconomics.Features.Movements.Create.Frequency
            {
                Type = FrequencyType.Yearly,
                Month = 2
            }));

        await fixture.SendCommandToMediatorAsync(new HomeEconomics.Features.Movements.Create.Command(
            "Seguro",
            70m,
            MovementType.Income,
            new HomeEconomics.Features.Movements.Create.Frequency
            {
                Type = FrequencyType.Yearly,
                Month = 1
            }));

        await fixture.SendCommandToMediatorAsync(new HomeEconomics.Features.Movements.Create.Command(
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
    
    protected async Task<MovementMonthResponse?> CreateMovementMonthAsync(Month month = Month.Jan)
    {
        var year = DateTime.Now.Year;

        var movementMonth = await fixture.SendCommandToMediatorAsync(new Create.Command(
            year,
            month));

        return movementMonth;
    }
    
    protected async Task AddStatusAsync(int year, int month, decimal accountAmount, decimal cashAmount) =>
        await fixture.SendCommandToMediatorAsync(new AddStatus.Command(
            year,
            (Month)month,
            accountAmount,
            cashAmount));
}
