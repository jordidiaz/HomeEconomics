using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeEconomics.Features.MovementMonths;

[ApiController]
[Route("api/movement-months")]
public class MovementMonthsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Create(Create.Command command)
    {
        var id = await mediator.Send(command);

        return Ok(id);
    }

    [HttpGet("{year:int}/{month:int}")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Detail(int year, int month)
    {
        var movementMonth = await mediator.Send(new Detail.Query(year, month));

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
        var result = await mediator.Send(new PayMonthMovement.Command(movementMonthId, monthMovementId));

        return Ok(result);
    }

    [HttpPost("{movementMonthId:int}/month-movements/{monthMovementId:int}/unpay")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> UnPayMonthMovement(int movementMonthId, int monthMovementId)
    {
        var result = await mediator.Send(new UnPayMonthMovement.Command(movementMonthId, monthMovementId));

        return Ok(result);
    }

    [HttpDelete("{movementMonthId:int}/month-movements/{monthMovementId:int}")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> DeleteMonthMovement(int movementMonthId, int monthMovementId)
    {
        var result = await mediator.Send(new DeleteMonthMovement.Command(movementMonthId, monthMovementId));

        return Ok(result);
    }

    [HttpPost("{movementMonthId:int}/month-movements/{monthMovementId:int}/update-amount")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> UpdateMonthMovementAmount(UpdateMonthMovementAmount.Command command)
    {
        var result = await mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("{movementMonthId:int}/month-movements")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> AddMonthMovement(AddMonthMovement.Command command)
    {
        var result = await mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("{movementMonthId:int}/add-status")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> AddStatus(AddStatus.Command command)
    {
        var result = await mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("{movementMonthId:int}/month-movements/{monthMovementId:int}/to-next-movement-month")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> MonthMovementToNextMovementMonth(int movementMonthId, int monthMovementId)
    {
        var result = await mediator.Send(new MonthMovementToNextMovementMonth.Command(movementMonthId, monthMovementId));

        return Ok(result);
    }
}
