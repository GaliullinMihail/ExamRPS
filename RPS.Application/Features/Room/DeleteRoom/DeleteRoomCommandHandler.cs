using RPS.Application.Dto.MediatR;
using RPS.Application.Services.Abstractions.Cqrs.Commands;
using RPS.Domain.Repositories.Abstractions;

namespace RPS.Application.Features.Room.DeleteRoom;

public class DeleteRoomCommandHandler : ICommandHandler<DeleteRoomCommand>
{
    private readonly IRepositoryManager _repositoryManager;

    public DeleteRoomCommandHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<Result> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _repositoryManager.RoomRepository.RemoveRoomByIdAsync(request.RoomId);

            return new Result(true);
        }
        catch (Exception e)
        {
            return new Result(false, e.Message);
        }
    }
}