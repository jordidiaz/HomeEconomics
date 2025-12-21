using FluentAssertions;
using HomeEconomics.IntegrationTests.Infrastructure;
using System.Net;
using Domain.MovementMonth;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths;

public class MonthMovementToNextMovementMonthTests : IntegrationTestBase
{
    private readonly Fixture _fixture;
    
    // ReSharper disable once ConvertToPrimaryConstructor
    public MonthMovementToNextMovementMonthTests(Fixture fixture) : base(fixture) => _fixture = fixture;
    
    [Fact]
    public async Task Should_Return_200_Ok_And_Pass_MonthMovementToNextMovementMonth_And_Return_Resume()
    {
        await CreateMovementsAsync();

        var movementMonthResponse = await CreateMovementMonthAsync(Month.Feb);
        var nextMovementMonthResponse = await CreateMovementMonthAsync(Month.Mar);

        movementMonthResponse!.MonthMovements.Length.Should().Be(2);
        movementMonthResponse.MonthMovements.SingleOrDefault(mm => mm.Name == "Amazon").Should().NotBeNull();
        nextMovementMonthResponse!.MonthMovements.Length.Should().Be(1);
        nextMovementMonthResponse.MonthMovements.SingleOrDefault(mm => mm.Name == "Amazon").Should().BeNull();

        var amazonMovementMonthId = movementMonthResponse.MonthMovements.Single(mm => mm.Name == "Amazon").Id;

        var response = await HttpClient
            .PostAsync($"api/movement-months/{movementMonthResponse.Id}/month-movements/{amazonMovementMonthId}/to-next-movement-month",null!);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var movementMonth = await _fixture.QueryDbContextAsync(async homeEconomicsDbContext =>
        {
            return await homeEconomicsDbContext
                .MovementMonths
                .Include("_monthMovements")
                .SingleOrDefaultAsync(mm => mm.Id == movementMonthResponse.Id);
        });

        var nextMovementMonth = await _fixture.QueryDbContextAsync(async homeEconomicsDbContext =>
        {
            return await homeEconomicsDbContext
                .MovementMonths
                .Include("_monthMovements")
                .SingleOrDefaultAsync(mm => mm.Id == nextMovementMonthResponse.Id);
        });

        movementMonth!.GetMonthMovements().Count().Should().Be(1);
        movementMonth.GetMonthMovements().SingleOrDefault(mm => mm.Name == "Amazon").Should().BeNull();
        nextMovementMonth!.GetMonthMovements().Count().Should().Be(2);
        nextMovementMonth.GetMonthMovements().SingleOrDefault(mm => mm.Name == "Amazon").Should().NotBeNull();
    }

    [Fact]
    public async Task Should_Return_409_Conflict_If_MovementMonth_Not_Exists()
    {
        var response = await HttpClient
            .PostAsync("api/movement-months/1/month-movements/1/to-next-movement-month",null!);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
    
    [Fact]
    public async Task Should_Return_409_Conflict_If_MonthMovement_Not_Exists()
    {
        await CreateMovementsAsync();
        var movementMonth = await CreateMovementMonthAsync();
        
        var response = await HttpClient
            .PostAsync($"api/movement-months/{movementMonth!.Id}/month-movements/99/to-next-movement-month",null!);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Should_Return_409_Conflict_If_NextMovementMonth_Not_Exists()
    {
        await CreateMovementsAsync();
        var movementMonth = await CreateMovementMonthAsync();
        
        var response = await HttpClient
            .PostAsync($"api/movement-months/{movementMonth!.Id}/month-movements/{movementMonth.MonthMovements[0].Id}/to-next-movement-month",null!);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
