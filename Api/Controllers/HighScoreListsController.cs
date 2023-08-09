using Core.CQRS.Commands;
using Core.CQRS.Queries;
using Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Api.Controllers;

[Produces("application/json")]
[ApiController]
[Route("highscore-lists")]
public class HighScoreListsController : ControllerBase
{
    private readonly IMediator _mediator;
    public HighScoreListsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<HighScoreListReadModel>>> GetAll()
    {
        return Ok(await _mediator.Send(new GetAllHighScoreLists.Request()));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(HighScoreListReadModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<HighScoreListReadModel>> GetById(string id)
    {
        var result = await _mediator.Send(new GetHighScoreListById.Request(id));
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpGet("calculatedposition/{id}/{score}")]
    [ProducesResponseType(typeof(HighScoreListReadModel), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<HighScoreListReadModel>> GetCalculatedPositionInList(string id, int score)
    {
        var result = await _mediator.Send(new GetCalculatedPositionInList.Request(id, score));
        return Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("{id}/game-results")]
    public async Task<ActionResult> AddGameResult(string id, [FromBody] GameResult gameResult)
    {
        var result = await _mediator.Send(new AddGameResult.Request(id, gameResult));
        if (result.NoMatchingListId)
        {
            return BadRequest($"The high score list with id '{id}' does not exist");
        }
        return Ok();
    }

#if DEBUG
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<HighScoreListReadModel>> CreateHighScoreList([FromBody] HighScoreListWriteModel input)
    {
        var result = await _mediator.Send(new CreateHighScoreList.Request(input));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
#endif
}