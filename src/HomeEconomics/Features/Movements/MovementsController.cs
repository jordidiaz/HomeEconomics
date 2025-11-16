using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace HomeEconomics.Features.Movements;

[ApiController]
[Route("api/movements")]
public class MovementsController(ICommandMediator commandMediator, IQueryMediator queryMediator)
    : ControllerBase
{
    [HttpPost]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create(Create.Command command)
    {
        var id = await commandMediator.SendAsync(command);

        return Ok(id);
    }

    [HttpGet]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Index.Result>> Index()
    {
        var movements = await queryMediator.QueryAsync(new Index.Query());

        return Ok(movements);
    }

    [HttpDelete("{id:int}")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Delete(int id)
    {
        await commandMediator.SendAsync(new Delete.Command(id));

        return NoContent();
    }

    [HttpPut("{id:int}")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Edit(Edit.Command command)
    {
        await commandMediator.SendAsync(command);

        return NoContent();
    }
}
