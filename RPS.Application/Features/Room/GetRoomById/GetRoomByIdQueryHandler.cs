using RPS.Application.Dto.MediatR;
using RPS.Application.Services.Abstractions.Cqrs.Queries;
using RPS.Domain.Repositories.Abstractions;

namespace RPS.Application.Features.Room.GetRoomById;

public class GetRoomByIdQueryHandler : IQueryHandler<GetRoomByIdQuery, Domain.Entities.Room?>
{
    private readonly IRepositoryManager _repositoryManager;

    public GetRoomByIdQueryHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }
    public async Task<Result<Domain.Entities.Room?>> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var room = (await _repositoryManager.RoomRepository.GetAllAsync(default))
            .FirstOrDefault(r => r.Id == request.RoomId);
        
        if (room is null)
            return new Result<Domain.Entities.Room?>(null, false, "Room not found");
        
        return new Result<Domain.Entities.Room?>(room, true);
    }
}