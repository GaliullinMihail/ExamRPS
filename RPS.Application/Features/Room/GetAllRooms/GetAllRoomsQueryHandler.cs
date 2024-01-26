using RPS.Application.Dto.MediatR;
using RPS.Application.Services.Abstractions.Cqrs.Queries;
using RPS.Domain.Repositories.Abstractions;
using RPS.Shared.StaticData;

namespace RPS.Application.Features.Room.GetAllRooms;

public class GetAllRoomsQueryHandler : IQueryHandler<GetAllRoomsQuery, IEnumerable<Domain.Entities.Room>>
{
    private readonly IRepositoryManager _repositoryManager;

    public GetAllRoomsQueryHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<Result<IEnumerable<Domain.Entities.Room>>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        return new Result<IEnumerable<Domain.Entities.Room>>(
            (await _repositoryManager.RoomRepository.GetAllAsync(cancellationToken))
            .OrderBy(r => r.SecondPlayerId)
            .ThenByDescending(r => r.CreationTime)
            .Skip(request.PageNumber * StaticData.GamesPerPage)
            .Take(StaticData.GamesPerPage)
            .ToList(),
            true);
    }
}