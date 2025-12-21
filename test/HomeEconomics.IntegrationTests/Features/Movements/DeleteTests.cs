using FluentAssertions;
using HomeEconomics.Features.Movements;
using HomeEconomics.IntegrationTests.Infrastructure;
using System.Net;
using Domain.Movements;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.Movements;

public class DeleteTests: IntegrationTestBase
{
    private readonly Fixture _fixture;
    
    // ReSharper disable once ConvertToPrimaryConstructor
    public DeleteTests(Fixture fixture) : base(fixture) => _fixture = fixture;
    
    [Fact]
    public async Task Should_Return_204_NoContent_And_Delete_A_Movement()
    {
        var movementId = await _fixture.SendCommandToMediatorAsync(new Create.Command(
            "Gasolina",
            60m,
            MovementType.Expense,
            new Create.Frequency
            {
                Type = FrequencyType.Monthly
            }));

        var movement = await _fixture.QueryDbContextAsync(async homeEconomicsDbContext =>
        {
            return await homeEconomicsDbContext
                .Movements
                .Include(m => m.Frequency)
                .SingleOrDefaultAsync(m => m.Id == movementId);
        });

        movement.Should().NotBeNull();

        var response = await HttpClient
            .DeleteAsync($"api/movements/{movementId}");

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        movement = await _fixture.QueryDbContextAsync(async homeEconomicsDbContext =>
        {
            return await homeEconomicsDbContext
                .Movements
                .Include(m => m.Frequency)
                .SingleOrDefaultAsync(m => m.Id == movementId);
        });

        movement.Should().BeNull();
    }
    
    [Fact]
    public async Task Should_Throw_InvalidOperationException_If_Movement_Not_Exists()
    {
        var action = async () => await _fixture.SendCommandToMediatorAsync(new Delete.Command(42));
    
        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage(Properties.Messages.MovementNotExists);
    }
    
}
