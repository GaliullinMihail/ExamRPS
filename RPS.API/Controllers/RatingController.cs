using MediatR;
using Microsoft.AspNetCore.Mvc;
using RPS.Application.Dto.PlayerRating;
using RPS.Application.Features.MongoDb.GetAllPlayersRating;
using RPS.Application.Features.MongoDb.GetPlayerRating;
using RPS.Application.Features.MongoDb.UpdatePlayerRating;

namespace RPS.API.Controllers;

[ApiController]
[Route("[controller]")]
public class RatingController : Controller
{
    private readonly IMediator _mediator;

    public RatingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("/{nickname}")]
    public async Task<JsonResult> GetPlayerRating([FromRoute] string nickname)
    {
        return Json(await _mediator.Send(new GetPlayerRatingMongoQuery(nickname)));
    }
    
    [HttpGet]
    [Route("/all")]
    public async Task<JsonResult> GetAllPlayersRating()
    {
        return Json(await _mediator.Send(new GetAllPlayersRatingQuery()));
    }
    
    [HttpPost]
    [Route("/update")]
    public async Task<IActionResult> PlayerScore([FromBody] UpdatePlayerRatingDto model)
    {
        await _mediator.Send(new UpdatePlayerRatingMongoCommand(model));
        return Ok();
    }
}