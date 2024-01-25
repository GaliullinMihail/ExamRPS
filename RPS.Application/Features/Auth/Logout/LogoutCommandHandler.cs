using Microsoft.AspNetCore.Identity;
using RPS.Application.Dto.MediatR;
using RPS.Application.Services.Abstractions.Cqrs.Commands;
using RPS.Domain.Entities;

namespace RPS.Application.Features.Auth.Logout;

public class LogoutCommandHandler : ICommandHandler<LogoutCommand>
{
    private readonly SignInManager<User> _signInManager;

    public LogoutCommandHandler(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _signInManager.SignOutAsync();
        }
        catch (Exception e)
        {
            return new Result(false, e.Message);
        }

        return new Result(true);
    }
}