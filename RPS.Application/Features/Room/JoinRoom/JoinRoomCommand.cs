using RPS.Application.Services.Abstractions.Cqrs.Commands;

namespace RPS.Application.Features.Room.JoinRoom;

public record JoinRoomCommand(string RoomId, Domain.Entities.User NewPlayer) : ICommand;