using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using RPS.Application.Dto.Authentication.Login;
using RPS.Application.Dto.MediatR;
using RPS.Application.Helpers.JwtGenerator;
using RPS.Application.Services.Abstractions.Cqrs.Commands;
using RPS.Domain.Entities;

namespace RPS.Application.Features.Auth.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponseDto>
{
    private readonly SignInManager<Domain.Entities.User> _signInManager;
    private readonly UserManager<Domain.Entities.User> _userManager;
    private readonly IJwtGenerator _jwtGenerator;

    public LoginCommandHandler(
        SignInManager<Domain.Entities.User> signInManager,
        UserManager<Domain.Entities.User> userManager, 
        IJwtGenerator jwtGenerator)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<Result<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var signedUser = await _signInManager.UserManager.FindByNameAsync(request.UserName);
        if (signedUser is null) 
            return new Result<LoginResponseDto>(
                new LoginResponseDto(LoginResponseStatus.Fail),
                false);

        var result = await _userManager.CheckPasswordAsync(signedUser, request.Password);
        
        if (!result) 
            return new Result<LoginResponseDto>(
                new LoginResponseDto(LoginResponseStatus.Fail),
                false);
        
        try
        {
            await _userManager.RemoveClaimAsync(signedUser, new Claim("Id", signedUser.Id));
            await _userManager.RemoveClaimAsync(signedUser, new Claim("Name", signedUser.UserName!));
        }
        catch (Exception)
        {
            // ignored
        }
                
        await _signInManager.UserManager.AddClaimAsync(signedUser, new Claim("Id", signedUser.Id));
        await _signInManager.UserManager.AddClaimAsync(signedUser, new Claim("Name", signedUser.UserName!));
        
        return new Result<LoginResponseDto>(
            new LoginResponseDto(
                LoginResponseStatus.Ok,
                await _jwtGenerator.GenerateJwtToken(signedUser.Id)),
            true);
    }
}