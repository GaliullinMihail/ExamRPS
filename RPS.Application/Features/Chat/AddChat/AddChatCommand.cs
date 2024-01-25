using RPS.Domain.Entities;
using RPS.Application.Services.Abstractions.Cqrs.Commands;

namespace RPS.Application.Features.Chat.AddChat;

public record AddChatCommand(string FirstUserId, string SecondUserId) : ICommand<Room>
{
}