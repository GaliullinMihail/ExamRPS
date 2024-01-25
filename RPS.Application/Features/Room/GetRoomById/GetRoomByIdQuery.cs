using RPS.Application.Services.Abstractions.Cqrs.Queries;

namespace RPS.Application.Features.Room.GetRoomById;

public record GetRoomByIdQuery(string RoomId) : IQuery<Domain.Entities.Room?>
{
}