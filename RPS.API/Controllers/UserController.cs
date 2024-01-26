using MediatR;
using Microsoft.AspNetCore.Mvc;
using RPS.Application.Features.User.GetUserById;

namespace RPS.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Route("/getUserNicknameById/{id}")]
    public async Task<JsonResult> GetGamesByPage([FromRoute] string id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id));

        return Json(result.Value!.UserName);
    }
}