using MediatR;
using RPS.Application.Dto.Chat;
using RPS.Application.Services.Abstractions.Cqrs.Commands;

namespace RPS.Application.Features.Room.SaveMessage;

public record SaveRoomMessageCommand(ChatMessageDto Message) : ICommand<Unit>, ICommand<ChatMessageDto>;
