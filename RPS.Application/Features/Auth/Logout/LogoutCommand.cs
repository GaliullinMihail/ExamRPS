using RPS.Application.Services.Abstractions.Cqrs.Commands;

namespace RPS.Application.Features.Auth.Logout;

public record LogoutCommand() : ICommand;