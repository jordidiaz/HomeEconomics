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
    }
}
