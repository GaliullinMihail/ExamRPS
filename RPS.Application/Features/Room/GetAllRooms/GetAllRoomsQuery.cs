using RPS.Application.Services.Abstractions.Cqrs.Queries;

namespace RPS.Application.Features.Room.GetAllRooms;

public record GetAllRoomsQuery(int PageNumber) : IQuery<IEnumerable<Domain.Entities.Room>>;