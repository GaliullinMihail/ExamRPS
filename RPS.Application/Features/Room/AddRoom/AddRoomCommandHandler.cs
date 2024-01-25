using RPS.Application.Dto.MediatR;
using RPS.Application.Services.Abstractions.Cqrs.Commands;
using RPS.Domain.Repositories.Abstractions;

namespace RPS.Application.Features.Room.AddRoom;

public class AddRoomCommandHandler : ICommandHandler<AddRoomCommand, Domain.Entities.Room>
{
    private readonly IRepositoryManager _repositoryManager;

    public AddRoomCommandHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }
    public async Task<Result<Domain.Entities.Room>> Handle(AddRoomCommand request, CancellationToken cancellationToken)
    {
        var room = new Domain.Entities.Room
        {
            Id = Guid.NewGuid().ToString(),
            Owner = request.Owner.UserName!,
            MaxRating = request.MaxRating,
            CreationTime = DateTime.Now,
            FirstPlayerId = request.Owner.Id
        };
        try
        {
            await _repositoryManager.RoomRepository.AddAsync(
                room);
            return new Result<Domain.Entities.Room>(room, true);
        }
        catch (Exception e)
        {
            return new Result<Domain.Entities.Room>(new Domain.Entities.Room(), false, e.Message);
        }
    }
}