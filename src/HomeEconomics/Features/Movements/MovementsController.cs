using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace HomeEconomics.Features.Movements
{
    [Route("api/movements")]
    [ApiController]
    public class MovementsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovementsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Create(Create.Command command)
        {
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
