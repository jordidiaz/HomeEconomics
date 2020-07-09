using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HomeEconomics.Features.MovementMonths
{
    [ApiController]
    [Route("api/movement-months")]
    public class MovementMonthsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovementMonthsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Create(Create.Command command)
        {
            var id = await _mediator.Send(command);

            return Ok(id);
        }

        [HttpGet("{year:int}/{month:int}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Detail(int year, int month)
        {
            var movementMonth = await _mediator.Send(new Detail.Query
            {
                Year = year,
                Month = month
            });

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
            var result = await _mediator.Send(new PayMonthMovement.Command
            {
                MovementMonthId = movementMonthId,
                MonthMovementId = monthMovementId
            });

            return Ok(result);
        }

        [HttpPost("{movementMonthId:int}/month-movements/{monthMovementId:int}/unpay")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UnPayMonthMovement(int movementMonthId, int monthMovementId)
        {
            var result = await _mediator.Send(new UnPayMonthMovement.Command
            {
                MovementMonthId = movementMonthId,
                MonthMovementId = monthMovementId
            });

            return Ok(result);
        }

        [HttpDelete("{movementMonthId:int}/month-movements/{monthMovementId:int}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> DeleteMonthMovement(int movementMonthId, int monthMovementId)
        {
            var result = await _mediator.Send(new DeleteMonthMovement.Command
            {
                MovementMonthId = movementMonthId,
                MonthMovementId = monthMovementId
            });

            return Ok(result);
        }

        [HttpPost("{movementMonthId:int}/month-movements/{monthMovementId:int}/update-amount")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateMonthMovementAmount(UpdateMonthMovementAmount.Command command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost("{movementMonthId:int}/month-movements")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> AddMonthMovement(AddMonthMovement.Command command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost("{movementMonthId:int}/add-status")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> AddStatus(AddStatus.Command command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
