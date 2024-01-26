using RPS.Application.Dto.Authentication.Login;
using RPS.Application.Services.Abstractions.Cqrs.Commands;

namespace RPS.Application.Features.Auth.Login;

public record LoginCommand(
    string UserName,
    string Password,
    bool RememberMe) : ICommand<LoginResponseDto>;