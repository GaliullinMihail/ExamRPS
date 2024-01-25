using RPS.Application.Services.Abstractions.Cqrs.Commands;

namespace RPS.Application.Features.Room.AddRoom;

public record AddRoomCommand(Domain.Entities.User Owner, int MaxRating) : ICommand<Domain.Entities.Room>;