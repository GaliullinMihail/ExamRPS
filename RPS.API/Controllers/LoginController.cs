using MediatR;
using RPS.Application.Dto.Authentication.Login;
using Microsoft.AspNetCore.Mvc;
using RPS.Application.Features.Auth.Login;
using RPS.Application.Features.Auth.Logout;

namespace RPS.API.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : Controller
{
    private readonly IMediator _mediator;

    public LoginController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<JsonResult> Login([FromBody] LoginRequestDto model)
    {
        if (!ModelState.IsValid) return Json(new LoginResponseDto(LoginResponseStatus.Fail));
        return Json((await _mediator.Send(
            new LoginCommand(
                model.UserName,
                model.Password,
                model.RememberMe))).Value);
    }

    [HttpGet("/logout")]
    public async Task<IActionResult> Logout()
    {
        await _mediator.Send(new LogoutCommand());
        return RedirectToAction("Login", "Login");
    }
}



