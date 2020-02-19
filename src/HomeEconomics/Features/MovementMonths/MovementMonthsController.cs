using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> PayMonthMovement(int movementMonthId, int monthMovementId)
        {
            await _mediator.Send(new PayMonthMovement.Command
            {
                MovementMonthId = movementMonthId,
                MonthMovementId = monthMovementId
            });

            return Ok();
        }

        [HttpPost("{movementMonthId:int}/month-movements/{monthMovementId:int}/unpay")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> UnPayMonthMovement(int movementMonthId, int monthMovementId)
        {
            await _mediator.Send(new UnPayMonthMovement.Command
            {
                MovementMonthId = movementMonthId,
                MonthMovementId = monthMovementId
            });

            return Ok();
        }
    }
}
