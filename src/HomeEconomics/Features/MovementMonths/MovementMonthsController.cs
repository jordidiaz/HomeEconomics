using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace HomeEconomics.Features.MovementMonths;

[ApiController]
[Route("api/movement-months")]
public class MovementMonthsController(ICommandMediator commandMediator, IQueryMediator queryMediator) : ControllerBase
{
    [HttpPost]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Create(Create.Command command)
    {
        var id = await commandMediator.SendAsync(command);

        return Ok(id);
    }

    [HttpGet("{year:int}/{month:int}")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Detail(int year, int month)
    {
        var movementMonth = await queryMediator.QueryAsync(new Detail.Query(year, month));

        if (movementMonth is null)
        {
            return NotFound();
        }

        return Ok(movementMonth);
    }

    [HttpPost("{movementMonthId:int}/month-movements/{monthMovementId:int}/pay")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> PayMonthMovement(int movementMonthId, int monthMovementId)
    {
        var result = await commandMediator.SendAsync(new PayMonthMovement.Command(movementMonthId, monthMovementId));

        return Ok(result);
    }

    [HttpPost("{movementMonthId:int}/month-movements/{monthMovementId:int}/unpay")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> UnPayMonthMovement(int movementMonthId, int monthMovementId)
    {
        var result = await commandMediator.SendAsync(new UnPayMonthMovement.Command(movementMonthId, monthMovementId));

        return Ok(result);
    }

    [HttpDelete("{movementMonthId:int}/month-movements/{monthMovementId:int}")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> DeleteMonthMovement(int movementMonthId, int monthMovementId)
    {
        var result = await commandMediator.SendAsync(new DeleteMonthMovement.Command(movementMonthId, monthMovementId));

        return Ok(result);
    }

    [HttpPost("{movementMonthId:int}/month-movements/{monthMovementId:int}/update-amount")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> UpdateMonthMovementAmount(UpdateMonthMovementAmount.Command command)
    {
        var result = await commandMediator.SendAsync(command);

        return Ok(result);
    }

    [HttpPost("{movementMonthId:int}/month-movements")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> AddMonthMovement(AddMonthMovement.Command command)
    {
        var result = await commandMediator.SendAsync(command);

        return Ok(result);
    }

    [HttpPost("{movementMonthId:int}/add-status")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> AddStatus(AddStatus.Command command)
    {
        var result = await commandMediator.SendAsync(command);

        return Ok(result);
    }

    [HttpPost("{movementMonthId:int}/month-movements/{monthMovementId:int}/to-next-movement-month")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> MonthMovementToNextMovementMonth(int movementMonthId, int monthMovementId)
    {
        var result = await commandMediator.SendAsync(new MonthMovementToNextMovementMonth.Command(movementMonthId, monthMovementId));

        return Ok(result);
    }
}
