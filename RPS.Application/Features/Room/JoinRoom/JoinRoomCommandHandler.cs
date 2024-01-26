using RPS.Application.Dto.MediatR;
using RPS.Application.Services.Abstractions.Cqrs.Commands;
using RPS.Domain.Repositories.Abstractions;

namespace RPS.Application.Features.Room.JoinRoom;

public class JoinRoomCommandHandler : ICommandHandler<JoinRoomCommand>
{
    private readonly IRepositoryManager _repositoryManager;

    public JoinRoomCommandHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<Result> Handle(JoinRoomCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _repositoryManager.RoomRepository.JoinRoomByIdAsync(request.RoomId, request.NewPlayer);
            return new Result(true);
        }
        catch (Exception e)
        {
            return new Result(false, e.Message);
        }
    }
}