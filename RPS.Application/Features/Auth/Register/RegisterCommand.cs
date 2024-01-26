using RPS.Application.Dto.Authentication.Register;
using RPS.Application.Services.Abstractions.Cqrs.Commands;

namespace RPS.Application.Features.Auth.Register;

public record RegisterCommand(
    string UserName,
    string Password) : ICommand<RegisterResponseDto>;