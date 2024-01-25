using MediatR;
using RPS.Application.Dto.Chat;
using RPS.Application.Services.Abstractions.Cqrs.Commands;

namespace RPS.Application.Features.Chat.SaveMessage;

public record SaveChatMessageByDtoBusCommand(ChatMessageDto Message) : ICommand<Unit>, ICommand<ChatMessageDto>;
