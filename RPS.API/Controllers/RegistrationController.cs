using MediatR;
using RPS.Application.Dto.Authentication.Register;
using Microsoft.AspNetCore.Mvc;
using RPS.Application.Features.Auth.Register;
using RPS.Application.Features.MongoDb.SavePlayerRating;
using RPS.Shared.Mongo;

namespace RPS.API.Controllers;

[ApiController]
[Route("[controller]")]
public class RegistrationController : Controller
{
    private readonly IMediator _mediator;
    public RegistrationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<JsonResult> Register([FromBody]RegisterRequestDto model)
    {
        if (!ModelState.IsValid) return Json(new RegisterResponseDto(RegisterResponseStatus.InvalidData));
        var result = 
            (await _mediator.Send(new RegisterCommand(model.UserName, model.Password))).Value;
        if (result!.Successful)
            await _mediator.Send(new SavePlayerRatingMongoCommand(
                new PlayerRatingDto
                {
                    Key = model.UserName,
                    Rating = 0
                }));
        return Json(result);
    }
}