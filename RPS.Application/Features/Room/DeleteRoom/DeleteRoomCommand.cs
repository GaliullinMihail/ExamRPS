using RPS.Application.Services.Abstractions.Cqrs.Commands;

namespace RPS.Application.Features.Room.DeleteRoom;

public record DeleteRoomCommand(string RoomId) : ICommand;