using Domain.Movements;
using FluentAssertions;
using HomeEconomics.Features.Movements;
using HomeEconomics.IntegrationTests.Infrastructure;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.Movements;

public class EditTest : IntegrationTestBase
{
    private readonly Fixture _fixture;
    
    // ReSharper disable once ConvertToPrimaryConstructor
    public EditTest(Fixture fixture) : base(fixture) => _fixture = fixture;

    [Fact]
    public async Task Should_Return_204_NoContent_Edit_The_Movement()
    {
        var movementId = await _fixture.SendCommandToMediatorAsync(new Create.Command(
            "Gasolina",
            60m,
            MovementType.Expense,
            new Create.Frequency
            {
                Type = FrequencyType.Monthly
            }));

        var command = new Edit.Command
        {
            Id = movementId,
            Name = "EPSV",
            Amount = 50m,
            Type = MovementType.Income,
            Frequency = new Edit.Frequency
            {
                Type = FrequencyType.Custom,
                Months =
                [
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
                ]
            }
        };
        
        var response = await HttpClient
            .PutAsync($"api/movements/{movementId}", command);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var movement = await _fixture.QueryDbContextAsync(async homeEconomicsDbContext =>
        {
            return await homeEconomicsDbContext
                .Movements
                .Include(m => m.Frequency)
                .SingleOrDefaultAsync(m => m.Id == movementId);
        });

        movement!.Id.Should().Be(movementId);
        movement.Name.Should().Be("EPSV");
        movement.Amount.Should().Be(50m);
        movement.Type.Should().Be(MovementType.Income);
        movement.Frequency.Type.Should().Be(FrequencyType.Custom);
        movement.Frequency.Months.SequenceEqual([
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
        ]).Should().BeTrue();
    }
    
    [Fact]
    public async Task Should_Throw_InvalidOperationException_If_Movement_Not_Exists()
    {
        var action = async () => await _fixture.SendCommandToMediatorAsync(new Edit.Command
        {
            Id = 42,
            Name = "Gasolina",
            Amount = 60m,
            Type = MovementType.Expense,
            Frequency = new Edit.Frequency
            {
                Type = FrequencyType.Monthly
            }
        });

        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage(Properties.Messages.MovementNotExists);
    }

    [Fact]
    public async Task Should_Update_Yearly_Frequency_Month_And_Clear_Previous_Selection()
    {
        var movementId = await _fixture.SendCommandToMediatorAsync(new Create.Command(
            "Seguro anual",
            120m,
            MovementType.Expense,
            new Create.Frequency
            {
                Type = FrequencyType.Yearly,
                Month = 3,
            }));

        var command = new Edit.Command
        {
            Id = movementId,
            Name = "Seguro anual",
            Amount = 120m,
            Type = MovementType.Expense,
            Frequency = new Edit.Frequency
            {
                Type = FrequencyType.Yearly,
                Month = 7,
            },
        };

        var response = await HttpClient.PutAsync($"api/movements/{movementId}", command);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var movement = await _fixture.QueryDbContextAsync(async homeEconomicsDbContext =>
            await homeEconomicsDbContext
                .Movements
                .Include(m => m.Frequency)
                .SingleOrDefaultAsync(m => m.Id == movementId));

        movement.Should().NotBeNull();
        movement.Frequency.Type.Should().Be(FrequencyType.Yearly);
        movement.Frequency.Months.Count(m => m).Should().Be(1);
        movement.Frequency.Months[6].Should().BeTrue();

        var indexResponse = await HttpClient.GetAsync("api/movements");
        indexResponse.EnsureSuccessStatusCode();

        var result = await indexResponse.Content.ReadFromJsonAsync<HomeEconomics.Features.Movements.Index.Result>();
        result.Should().NotBeNull();
        var editedMovement = result.Movements.Single(m => m.Id == movementId);
        editedMovement.FrequencyType.Should().Be((int)FrequencyType.Yearly);
        editedMovement.FrequencyMonth.Should().Be(7);
    }
}
