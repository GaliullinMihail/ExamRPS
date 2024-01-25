using RPS.Domain.Entities;
using MassTransit;
using MediatR;
using RPS.Application.Dto.MediatR;
using RPS.Application.Services.Abstractions.Cqrs.Commands;

namespace RPS.Application.Features.Chat.SaveMessage;

public class SaveChatMessageByDtoBusHandler: ICommandHandler<SaveChatMessageByDtoBusCommand, Unit>
{
    private readonly IBus _bus;

    public SaveChatMessageByDtoBusHandler(IBus bus)
    {
        _bus = bus;
    }
    
    public async Task<Result<Unit>> Handle(
        SaveChatMessageByDtoBusCommand request,
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