using MassTransit;
using MediatR;
using RPS.Application.Dto.MediatR;
using RPS.Application.Services.Abstractions.Cqrs.Commands;
using RPS.Domain.Entities;

namespace RPS.Application.Features.Room.SaveMessage;

public class SaveRoomMessageCommandHandler: ICommandHandler<SaveRoomMessageCommand, Unit>
{
    private readonly IBus _bus;

    public SaveRoomMessageCommandHandler(IBus bus)
    {
        _bus = bus;
    }
    
    public async Task<Result<Unit>> Handle(
        SaveRoomMessageCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = new Message()
            {
                SenderId = request.Message.SenderId,
                ReceiverId = request.Message.ReceiverId,
                Content = request.Message.Content,
                Timestamp = request.Message.Timestamp,
                RoomId = request.Message.RoomId
            };
            await _bus.Publish(entity, cancellationToken);
            return new Result<Unit>(new Unit(), true);
        }
        catch (Exception e)
        {
            return new Result<Unit>(new Unit(), false, e.Message);
        }
        
    }
}