using MassTransit;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using RPS.Application.Features.Room.DeleteRoom;
using RPS.Application.Features.Room.GetRoomById;
using RPS.Application.Features.User.GetUserById;
using RPS.Domain.Enums;
using RPS.Shared.GameResult;
using RPS.Shared.StaticData;

namespace RPS.API.Hubs
{
    public class GameHub : Hub
    {
        private readonly IBus _bus;
        private readonly IMediator _mediator;

        public GameHub(IMediator mediator, IBus bus)
        {
            _mediator = mediator;
            _bus = bus;
        }

        public async Task ConnectToRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            if (!StaticData.GameHubConnections.ContainsKey(roomId))
                StaticData.GameHubConnections[roomId] = new List<string>();
            StaticData.GameHubConnections[roomId].Add(Context.ConnectionId);
            var room = (await _mediator.Send(new GetRoomByIdQuery(roomId))).Value;
            if (StaticData.GameHubConnections[roomId].Count >= 2 && room!.SecondPlayerId is not null)
                await Clients.Group(roomId).SendAsync("StartGame");
        }

        public async Task LeaveFromRoom(string userId, string roomId)
        {
            var roomResult = await _mediator.Send(new GetRoomByIdQuery(roomId));

            if (!roomResult.IsSuccess)
                return;
            var room = roomResult.Value;
            if (room!.FirstPlayerId != userId && room.SecondPlayerId != userId)
                return;
            
            await Clients.Group(roomId).SendAsync("EndGame");
            
            foreach (var connectionId in StaticData.GameHubConnections[roomId])
            {
                await Groups.RemoveFromGroupAsync(connectionId, roomId);
            }
            
            StaticData.GameHubConnections.Remove(roomId);

            await _mediator.Send(new DeleteRoomCommand(roomId));
        }
        
        public async Task SendGameMessage(
            string senderUserId, 
            string sign,
            string roomId)
        {
            var room = await _mediator.Send(new GetRoomByIdQuery(roomId));
            
            var sender = await _mediator.Send(new GetUserByIdQuery(senderUserId));
            
            if (!sender.IsSuccess || !room.IsSuccess || !Enum.TryParse<GameSigns>(sign, out _))
                return;
            
            await Clients.Group(roomId).SendAsync("ReceiveGameMessage", senderUserId, 
                sign);
        }
        
        public async Task SendResultMessage(
            int result,
            string roomId)
        {
            var roomResult = await _mediator.Send(new GetRoomByIdQuery(roomId));

            var room = roomResult.Value;

            var firstPlayer =
                (await _mediator.Send(new GetUserByIdQuery(room!.FirstPlayerId))).Value;
            var secondPlayer =
                (await _mediator.Send(new GetUserByIdQuery(room.SecondPlayerId!))).Value;
            GameResultDto gameResult;
            switch (result)
            {
                case -1:
                    gameResult = new GameResultDto
                    {
                        Draw = false,
                        WinnerNickName = secondPlayer!.UserName!,
                        LooserNickName = firstPlayer!.UserName!
                    };
                    break;
                case 0:
                    gameResult = new GameResultDto
                    {
                        Draw = true,
                        WinnerNickName = secondPlayer!.UserName!,
                        LooserNickName = firstPlayer!.UserName!
                    };
                    break;
                default:
                    gameResult = new GameResultDto
                    {
                        Draw = false,
                        WinnerNickName = firstPlayer!.UserName!,
                        LooserNickName = secondPlayer!.UserName!
                    };
                    break;
            }
            
            await _bus.Publish(gameResult);
            
            await Clients.Group(roomId).SendAsync("ReceiveResultMessage", firstPlayer!.UserName,
                secondPlayer!.UserName,
                result);
        }
        
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}