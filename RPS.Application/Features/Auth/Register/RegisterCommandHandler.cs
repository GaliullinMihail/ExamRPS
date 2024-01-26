using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using RPS.Application.Dto.Authentication.Register;
using RPS.Application.Dto.MediatR;
using RPS.Application.Services.Abstractions.Cqrs.Commands;
using RPS.Domain.Entities;

namespace RPS.Application.Features.Auth.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, RegisterResponseDto>
{
    private readonly UserManager<Domain.Entities.User> _userManager;

    public RegisterCommandHandler(UserManager<Domain.Entities.User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<RegisterResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new Domain.Entities.User
        {
            UserName = request.UserName
        };

        var usernameCollision = _userManager.Users.FirstOrDefault(u => u.UserName == user.UserName);
        if (usernameCollision is not null)
            return new Result<RegisterResponseDto>(
                new RegisterResponseDto(
                    RegisterResponseStatus.Fail,
                    "User with that username already exists"),
                false);
            
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return new Result<RegisterResponseDto>( 
                new RegisterResponseDto(
                    RegisterResponseStatus.Fail,
                    result.Errors.FirstOrDefault()!.Description),
                false);
                
        var userInDb = await _userManager.FindByNameAsync(user.UserName);
        if (userInDb is null)
            return new Result<RegisterResponseDto>(
                new RegisterResponseDto(
                    RegisterResponseStatus.Fail,
                    "User registration error"),
                false);
                
        await _userManager.AddClaimAsync(userInDb, new Claim(ClaimTypes.Role, "User"));
        return new Result<RegisterResponseDto>(
            new RegisterResponseDto(RegisterResponseStatus.Ok),
            true);
    }
}