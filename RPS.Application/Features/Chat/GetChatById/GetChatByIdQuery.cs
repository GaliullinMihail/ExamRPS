using RPS.Domain.Entities;
using RPS.Application.Services.Abstractions.Cqrs.Queries;

namespace RPS.Application.Features.Chat.GetChatById;

public record GetChatByIdQuery(string CurrUserId, string UserId) : IQuery<Room?>
{
}