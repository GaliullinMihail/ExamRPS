using RPS.Domain.Entities;
using RPS.Domain.Repositories.Abstractions;
using RPS.Application.Dto.MediatR;
using RPS.Application.Services.Abstractions.Cqrs.Commands;

namespace RPS.Application.Features.Chat.AddChat;

public class AddChatHandler : ICommandHandler<AddChatCommand, Room>
{
    private readonly IRepositoryManager _repositoryManager;

    public AddChatHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }
    public async Task<Result<Room>> Handle(AddChatCommand request, CancellationToken cancellationToken)
    {
        var room = new Room
        {
            Id = Guid.NewGuid().ToString(),
            FirstUserId = request.FirstUserId,
            SecondUserId = request.SecondUserId,
            Name = Guid.NewGuid().ToString()
        };
        try
        {
            await _repositoryManager.RoomRepository.AddAsync(
                room);
            return new Result<Room>(room, true);
        }
        catch (Exception e)
        {
            return new Result<Room>(new Room(), false, e.Message);
        }
    }
}