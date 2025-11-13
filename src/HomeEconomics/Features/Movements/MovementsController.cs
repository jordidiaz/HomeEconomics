using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeEconomics.Features.Movements;

[ApiController]
[Route("api/movements")]
public class MovementsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MovementsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create(Create.Command command)
    {
        var id = await _mediator.Send(command);

        return Ok(id);
    }

    [HttpGet]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Index.Result>> Index()
    {
        var movements = await _mediator.Send(new Index.Query());

        return Ok(movements);
    }

    [HttpDelete("{id:int}")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Delete(int id)
    {
        await _mediator.Send(new Delete.Command(id));

        return NoContent();
    }

    [HttpPut("{id:int}")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Edit(Edit.Command command)
    {
        await _mediator.Send(command);

        return NoContent();
    }
}