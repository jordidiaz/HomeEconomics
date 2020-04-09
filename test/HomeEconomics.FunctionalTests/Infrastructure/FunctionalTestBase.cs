using System;
using System.Threading.Tasks;
using Domain.MovementMonth;
using Domain.Movements;
using HomeEconomics.Features.MovementMonths;
using Xunit;

namespace HomeEconomics.FunctionalTests.Infrastructure
{
    public class FunctionalTestBase : IAsyncLifetime
    {
        public async Task InitializeAsync()
        {
            await Fixture.ResetCheckpointAsync();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public static async Task CreateMovements()
        {
            await Fixture.SendToMediatRAsync(new HomeEconomics.Features.Movements.Create.Command
            {
                Name = "Income",
                Amount = 50m,
                Type = MovementType.Income,
                Frequency = new HomeEconomics.Features.Movements.Create.Frequency
                {
                    Type = FrequencyType.None
                }
            });

            await Fixture.SendToMediatRAsync(new HomeEconomics.Features.Movements.Create.Command
            {
                Name = "Gasolina",
                Amount = 60m,
                Type = MovementType.Expense,
                Frequency = new HomeEconomics.Features.Movements.Create.Frequency
                {
                    Type = FrequencyType.Monthly
                }
            });

            await Fixture.SendToMediatRAsync(new HomeEconomics.Features.Movements.Create.Command
            {
                Name = "Amazon",
                Amount = 30m,
                Type = MovementType.Expense,
                Frequency = new HomeEconomics.Features.Movements.Create.Frequency
                {
                    Type = FrequencyType.Yearly,
                    Month = 2
                }
            });

            await Fixture.SendToMediatRAsync(new HomeEconomics.Features.Movements.Create.Command
            {
                Name = "Seguro",
                Amount = 70m,
                Type = MovementType.Income,
                Frequency = new HomeEconomics.Features.Movements.Create.Frequency
                {
                    Type = FrequencyType.Yearly,
                    Month = 1
                }
            });

            await Fixture.SendToMediatRAsync(new HomeEconomics.Features.Movements.Create.Command
            {
                Name = "Custom",
                Amount = 60m,
                Type = MovementType.Expense,
                Frequency = new HomeEconomics.Features.Movements.Create.Frequency
                {
                    Type = FrequencyType.Custom,
                    Months = new[]
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

        public static async Task<MovementMonthResponse> CreateMovementMonth(Month month = Month.Jan)
        {
            var year = DateTime.Now.Year;

            var movementMonth = await Fixture.SendToMediatRAsync(new Create.Command
            {
                Year = year,
                Month = month
            });

            return movementMonth;
        }

        public static async Task AddStatus(int year, int month, decimal accountAmount, decimal cashAmount)
        {
            await Fixture.SendToMediatRAsync(new AddStatus.Command
            {
                Year = year,
                Month = (Month)month,
                AccountAmount = accountAmount,
                CashAmount = cashAmount
            });
        }
    }
}
